using AgendamentoApp.Context;
using AgendamentoApp.Extensions;
using AgendamentoApp.Models;
using AgendamentoApp.Services;
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

        public AgendamentosController(AppDbContext context, ILogger<AgendamentosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var agendamentos = _context.Agendamentos.Include(m => m.Municipio);

                var agendamentosVM = await agendamentos.Select(a => a.ToViewModel()).ToListAsync();

                return View(agendamentosVM);
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
            try
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

                var existeAgendamento = await _context.Agendamentos.AnyAsync(a => a.Agendado == agendamentoVM.Agendado);

                if (existeAgendamento)
                {
                    ModelState.AddModelError(nameof(agendamentoVM.Agendado), "Já existe um agendamento para este horário. Tente outro horário.");

                    return View(agendamentoVM);
                }
                var agendamento = agendamentoVM.ToEntity();

                agendamento.DefinirSituacao();

                _context.Agendamentos.Add(agendamento);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Agendamento criado com sucesso!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o agendamento.");

                TempData["ErrorMessage"] = "Não foi possível criar o agendamento";

                return RedirectToAction("Index");
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
                var agendamento = await _context.Agendamentos.FindAsync(id);

                if (agendamento == null)
                    NotFound();

                _context.Agendamentos.Remove(agendamento);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Agendamento deletado com sucesso!";

                return RedirectToAction(nameof(Index));
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
                var existeAgendamento = await _context.Agendamentos.AnyAsync(a =>
                a.Agendado == agendamentoVM.Agendado && a.Id != agendamentoVM.Id);

                if (existeAgendamento)
                {
                    ModelState.AddModelError(nameof(agendamentoVM.Agendado), "Já existe um agendamento para este horário. Tente outro horário.");

                    return View(agendamentoVM);
                }


                var agendamento = agendamentoVM.ToEntity();

                agendamento.DefinirSituacao();

                _context.Agendamentos.Update(agendamento);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Agendamento editado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (DBConcurrencyException ex)
            {
                if (!_context.Agendamentos.Any(a => a.Id == agendamentoVM.Id))
                    return NotFound();

                throw;
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest("Id invalido.");

            var agendamento = await _context.Agendamentos
                .Include(a => a.Municipio)
                .SingleOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
                return NotFound();

            var agendamentoVM = agendamento.ToViewModel();

            return View(agendamentoVM);
        }

    }
}
