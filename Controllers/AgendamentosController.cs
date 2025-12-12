using AgendamentoApp.Context;
using AgendamentoApp.Extensions;
using AgendamentoApp.Models;
using AgendamentoApp.Services.Interfaces;
using AgendamentoApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AgendamentoApp.Controllers
{
    public class AgendamentosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AgendamentosController> _logger;
        private readonly IAgendamentoService _service;

        public AgendamentosController(AppDbContext context, ILogger<AgendamentosController> logger, IAgendamentoService service)
        {
            _context = context;
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var agendamentos = await _service.ListarTodosAsync();

                var agendamentosVM = agendamentos.Select(a => a.ToViewModel()).ToList();

                return View(agendamentosVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar agendamentos");
                TempData["ErrorMessage"] = "Os agendamentos não foram carregados corretamente. Tente novamente mais tarde.";

                return View(new List<AgendamentoViewModel>());
            }
        }
        public async Task<IActionResult> Create()
        {
            var agendamentoVM = new AgendamentoViewModel();

            agendamentoVM.Municipios = await _service.ObterMunicipiosAsync();

            return View(agendamentoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AgendamentoViewModel agendamentoVM)
        {
            if (!ModelState.IsValid)
            {
                agendamentoVM.Municipios = await _service.ObterMunicipiosAsync();

                return View(agendamentoVM);
            }
            try
            {
                var agendamento = agendamentoVM.ToEntity();

                await _service.CriarAgendamentoAsync(agendamento);

                TempData["SuccessMessage"] = "Agendamento criado com sucesso!";

                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(Agendamento.Agendado), ex.Message);

                agendamentoVM.Municipios = await _service.ObterMunicipiosAsync();

                return View(agendamentoVM);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var agendamento = await _service.DetalhesAsync(id);

                var agendamentoVM = agendamento.ToViewModel();

                return View(agendamentoVM);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "ID invalido");

                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Agendamento não encontrado");

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.ConfirmacaoDeletarAsync(id);

                TempData["SuccessMessage"] = "Agendamento deletado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Agendamento não encontrado no banco de dados");
                return NotFound();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao excluir o agendamento.");

                TempData["ErrorMessage"] = "Não foi possível excluir o agendamento, tente novamente mais tarde.";

                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {

            var agendamento = await _service.DetalhesAsync(id);

            var agendamentoVM = agendamento.ToViewModel();

            agendamentoVM.Municipios = await _context.Municipios
                .Select(m =>
                new SelectListItem()
                {
                    Value = m.Id.ToString(),
                    Text = m.Nome
                })
                .ToListAsync();

            return View(agendamentoVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AgendamentoViewModel agendamentoVM)
        {
            if (id != agendamentoVM.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                agendamentoVM.Municipios = await _service.ObterMunicipiosAsync();

                return View(agendamentoVM);
            }
            try
            {
                var agendamento = agendamentoVM.ToEntity();

                await _service.EditarAsync(agendamento);

                TempData["SuccessMessage"] = "Agendamento editado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(Agendamento.Agendado), ex.Message);

                agendamentoVM.Municipios = await _service.ObterMunicipiosAsync();

                return View(agendamentoVM);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar no banco de dados");

                agendamentoVM.Municipios = await _service.ObterMunicipiosAsync();

                return View(agendamentoVM);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var agendamento = await _service.DetalhesAsync(id);

                var agendamentoVM = agendamento.ToViewModel();

                return View(agendamentoVM);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "ID invalido");

                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Agendamento não encontrado");

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
