using AgendamentoApp.Context;
using AgendamentoApp.Enums;
using AgendamentoApp.Models;
using AgendamentoApp.Services.Interfaces;

namespace AgendamentoApp.Services
{
    public class AgendamentoService : IAgendamentoService
    {
        private readonly AppDbContext _context;

        public AgendamentoService(AppDbContext context)
        {
            _context = context;
        }
        public SituacaoAgendamento VerificarSituacao(Agendamento agendamento)
        {
            if (agendamento.HorarioAtendimento == null)
                return SituacaoAgendamento.Pendente;

            if (agendamento.Entrada.HasValue && agendamento.Entrada.Value > agendamento.Agendado)
                return SituacaoAgendamento.Atrasado;

            else
                return SituacaoAgendamento.NoHorario;
        }

        public SituacaoAgendamento VerificarSituacao(TimeSpan? entrada, TimeSpan agendado)
        {
            if (entrada == null)
                return SituacaoAgendamento.Pendente;

            return entrada <= agendado ? SituacaoAgendamento.NoHorario : SituacaoAgendamento.Atrasado;
        }
    }
}
