using AgendamentoApp.Context;
using AgendamentoApp.Extensions;
using AgendamentoApp.Models;
using AgendamentoApp.Services.Interfaces;
using AgendamentoApp.ViewModel;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Agendamento> CriarAgendamentoAsync(AgendamentoViewModel agendamentoVM)
        {
            var existeAgendamento = await _context.Agendamentos.AnyAsync(a => a.Agendado == agendamentoVM.Agendado);

            if (existeAgendamento)
                throw new ArgumentException("Já existe um agendamento para este horário. Tente outro horário.");

            var agendamento = agendamentoVM.ToEntity();

            agendamento.DefinirSituacao();

            return agendamento;
        }
    }
}
