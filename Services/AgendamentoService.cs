using AgendamentoApp.Context;
using AgendamentoApp.Extensions;
using AgendamentoApp.Models;
using AgendamentoApp.Services.Interfaces;
using AgendamentoApp.ViewModel;
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
        public async Task<List<AgendamentoViewModel>> ListarTodosAsync()
        {
            var agendamentos = _context.Agendamentos.Include(m => m.Municipio);

            var agendamentosVM = await agendamentos.Select(a => a.ToViewModel()).ToListAsync();

            return agendamentosVM;
        }
        public async Task CriarAgendamentoAsync(AgendamentoViewModel agendamentoVM)
        {
            await VerificarHorarioAgendadoAsync(agendamentoVM);

            var agendamento = agendamentoVM.ToEntity();

            agendamento.DefinirSituacao();

            await _context.Agendamentos.AddAsync(agendamento);
            await _context.SaveChangesAsync();
        }
        public async Task DeletarAsync(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);

            if (agendamento == null)
                throw new NullReferenceException("Agendamento não encontrado");

            _context.Agendamentos.Remove(agendamento);
            await _context.SaveChangesAsync();
        }
        public async Task EditarAsync(AgendamentoViewModel agendamentoVM)
        {
            await VerificarHorarioAgendadoAsync(agendamentoVM);

            var agendamento = agendamentoVM.ToEntity();

            agendamento.DefinirSituacao();

            _context.Agendamentos.Update(agendamento);
            await _context.SaveChangesAsync();
        }
        public async Task<AgendamentoViewModel> DetalhesAsync(int? id)
        {
            if (id == null)
                throw new NullReferenceException("ID invalido");

            var agendamento = await _context.Agendamentos
                .Include(a => a.Municipio)
                .SingleOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
                throw new NullReferenceException("Agendamento não encontrado");

            var agendamentoVM = agendamento.ToViewModel();

            return agendamentoVM;
        }
        public async Task VerificarHorarioAgendadoAsync(AgendamentoViewModel agendamentoVM)
        {
            var existeAgendamento = await _context.Agendamentos.AnyAsync(a => a.Agendado == agendamentoVM.Agendado && a.Id != agendamentoVM.Id);

            if (existeAgendamento)
                throw new ArgumentException("Já existe um agendamento para este horário. Tente outro horário.");
        }
        
    }
}
