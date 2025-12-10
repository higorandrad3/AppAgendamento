using AgendamentoApp.Enums;
using AgendamentoApp.Models;
using AgendamentoApp.ViewModel;

namespace AgendamentoApp.Services.Interfaces
{
    public interface IAgendamentoService
    {
        Task<Agendamento> CriarAgendamentoAsync(AgendamentoViewModel agendamento);
    }
}
