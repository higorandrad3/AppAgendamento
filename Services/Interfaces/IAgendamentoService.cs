using AgendamentoApp.Enums;
using AgendamentoApp.Models;
using AgendamentoApp.ViewModel;

namespace AgendamentoApp.Services.Interfaces
{
    public interface IAgendamentoService
    {
        Task<List<AgendamentoViewModel>> ListarTodosAsync();
        Task CriarAgendamentoAsync(AgendamentoViewModel agendamento);
        Task DeletarAsync(int id);
        Task EditarAsync(AgendamentoViewModel agendamentoVM);
        Task VerificarHorarioAgendadoAsync(AgendamentoViewModel agendamentoVM);
        Task<AgendamentoViewModel> DetalhesAsync(int? id);
    }
}
