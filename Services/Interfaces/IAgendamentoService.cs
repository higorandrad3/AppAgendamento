using AgendamentoApp.Enums;
using AgendamentoApp.Models;
using AgendamentoApp.ViewModel;

namespace AgendamentoApp.Services.Interfaces
{
    public interface IAgendamentoService
    {
        Task<List<AgendamentoViewModel>> ListarTodosAsync();
        Task<Agendamento> CriarAgendamentoAsync(AgendamentoViewModel agendamento);
    }
}
