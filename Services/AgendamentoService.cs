using AgendamentoApp.Context;
using AgendamentoApp.Extensions;
using AgendamentoApp.Models;
using AgendamentoApp.Services.Interfaces;
using AgendamentoApp.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace AgendamentoApp.Services
{
    public class AgendamentoService : IAgendamentoService
    {
        private readonly AppDbContext _context;

        public AgendamentoService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Agendamento>> ListarTodosAsync()
        {
            var agendamentos = await _context.Agendamentos.Include(m => m.Municipio).ToListAsync();

            return agendamentos;
        }
        public async Task CriarAgendamentoAsync(Agendamento agendamento)
        {
            await VerificarHorarioAgendadoAsync(agendamento);

            agendamento.DefinirSituacao();

            await _context.Agendamentos.AddAsync(agendamento);
            await _context.SaveChangesAsync();
        }
        public async Task ConfirmacaoDeletarAsync(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);

            if (agendamento == null)
                throw new NullReferenceException("Agendamento não encontrado");

            _context.Agendamentos.Remove(agendamento);
            await _context.SaveChangesAsync();
        }
        public async Task EditarAsync(Agendamento agendamento)
        {
            await VerificarHorarioAgendadoAsync(agendamento);

            agendamento.DefinirSituacao();

            _context.Agendamentos.Update(agendamento);
            await _context.SaveChangesAsync();
        }
        public async Task<Agendamento> DetalhesAsync(int? id)
        {
            if (id == null)
                throw new ArgumentException("ID invalido");

            var agendamento = await _context.Agendamentos
                .Include(a => a.Municipio)
                .SingleOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
                throw new NullReferenceException("Agendamento não encontrado");

            return agendamento;
        }
        public async Task VerificarHorarioAgendadoAsync(Agendamento agendamento)
        {
            var existeAgendamento = await _context.Agendamentos.AnyAsync(a => a.Agendado == agendamento.Agendado && a.Id != agendamento.Id);

            if (existeAgendamento)
                throw new ArgumentException("Já existe um agendamento para este horário. Tente outro horário.");
        }
        public Task<List<SelectListItem>> ObterMunicipiosAsync()
        {
            return _context.Municipios
                .Select(m => new SelectListItem()
                {
                    Value = m.Id.ToString(),
                    Text = m.Nome
                })
                .ToListAsync();
        }
    }
}
