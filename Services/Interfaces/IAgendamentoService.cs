using AgendamentoApp.Enums;
using AgendamentoApp.Models;

namespace AgendamentoApp.Services.Interfaces
{
    public interface IAgendamentoService
    {
        public SituacaoAgendamento VerificarSituacao(Agendamento agendamento);
        public SituacaoAgendamento VerificarSituacao(TimeSpan? entrada, TimeSpan agendado);
    }
}
