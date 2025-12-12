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
                var agendamentoVM = await _service.ListarTodosAsync();

                return View(agendamentoVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar agendamentos");
                TempData["ErrorMessage"] = "Os agendamentos não foram carregados corretamente. Tente novamente mais tarde.";

                return View(new List<AgendamentoViewModel>());
            }
        }
        public IActionResult Create()
        {
            var municipios = new AgendamentoViewModel()
            {
                Municipios = _context.Municipios
                .Select(m => new SelectListItem()
                {
                    Value = m.Id.ToString(),
                    Text = m.Nome,
                })
                .ToList()
            };

            return View(municipios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AgendamentoViewModel agendamentoVM)
        {
            if (!ModelState.IsValid)
            {
                agendamentoVM.Municipios = _context.Municipios
                    .Select(m => new SelectListItem()
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nome,
                    })
                    .ToList();

                return View(agendamentoVM);
            }
            try
            {
                await _service.CriarAgendamentoAsync(agendamentoVM);

                TempData["SuccessMessage"] = "Agendamento criado com sucesso!";

                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(Agendamento.Agendado), ex.Message);

                agendamentoVM.Municipios = _context.Municipios
                    .Select(m => new SelectListItem()
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nome,
                    })
                    .ToList();

                return View(agendamentoVM);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest("ID invalido.");

            var agendamento = await _context.Agendamentos
                .Include(a => a.Municipio)
                .SingleOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
                return NotFound();

            var agendamentoVM = agendamento.ToViewModel();

            return View(agendamentoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.DeletarAsync(id);

                TempData["SuccessMessage"] = "Agendamento deletado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Agendamento não encontrado no banco de dados");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir o agendamento.");
                TempData["ErrorMessage"] = "Não foi possível excluir o agendamento, tente novamente mais tarde.";

                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
                return BadRequest("ID invalido");

            var agendamento = await _context.Agendamentos
                .Include(a => a.Municipio)
                .SingleOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
                return NotFound();

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
            try
            {
                await _service.EditarAsync(agendamentoVM);

                TempData["SuccessMessage"] = "Agendamento editado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(Agendamento.Agendado), ex.Message);

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
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar no banco de dados");

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
        }

        public async Task<IActionResult> Details(int? id)
        {
            var agendamentoVM = await _service.DetalhesAsync(id);

            return View(agendamentoVM);
        }
    }
}
