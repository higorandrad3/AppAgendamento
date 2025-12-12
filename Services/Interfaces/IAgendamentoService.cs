using AgendamentoApp.Enums;
using AgendamentoApp.Models;
using AgendamentoApp.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgendamentoApp.Services.Interfaces
{
    public interface IAgendamentoService
    {
        Task<List<Agendamento>> ListarTodosAsync();
        Task CriarAgendamentoAsync(Agendamento agendamento);
        Task ConfirmacaoDeletarAsync(int id);
        Task EditarAsync(Agendamento agendamento);
        Task VerificarHorarioAgendadoAsync(Agendamento agendamento);
        Task<Agendamento> DetalhesAsync(int? id);
        Task<List<SelectListItem>> ObterMunicipiosAsync();
    }
}
