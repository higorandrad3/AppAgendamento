using AgendamentoApp.Context;
using AgendamentoApp.Models;
using AgendamentoApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace AgendamentoApp.Controllers
{
    public class MunicipiosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MunicipiosController> _logger;

        public MunicipiosController(AppDbContext context, ILogger<MunicipiosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var municipios = _context.Municipios;

            var municipioVM = municipios.Select(m => new MunicipiosViewModel()
            {
                Id = m.Id,
                Nome = m.Nome
            }).ToList();

            return View(municipioVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MunicipiosViewModel municipioVM)
        {

            if (!ModelState.IsValid)
                return View(municipioVM);

            var existeMunicipio = await _context.Municipios.AnyAsync(
                m => m.Nome.ToLower().Equals(municipioVM.Nome.ToLower()));

            if (existeMunicipio)
            {
                ModelState.AddModelError(nameof(municipioVM.Nome), "Um município já foi cadastrado com este nome");

                return View(municipioVM);
            }

            var municipio = new Municipio()
            {
                Nome = municipioVM.Nome
            };
            try
            {
                await _context.Municipios.AddAsync(municipio);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Município criado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar formulário");

                ModelState.AddModelError(string.Empty, "Erro ao salvar o município. Tente novamente mais tarde.");

                return View(municipioVM);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest("Id invalido");

            var municipio = await _context.Municipios.FindAsync(id);

            if (municipio == null)
                return NotFound();

            var municipioVM = new MunicipiosViewModel()
            {
                Nome = municipio.Nome
            };

            return View(municipioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return BadRequest("ID invalido");

            var municipio = await _context.Municipios.FindAsync(id);

            if (municipio == null)
                return NotFound();

            try
            {
                _context.Municipios.Remove(municipio);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Município deletado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao excluir registro");

                TempData["ErrorMessage"] = "Falha ao deletar município, tente novamente mais tarde.";

                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest("ID invalido");

            var municipio = await _context.Municipios.FindAsync(id);

            if (municipio == null)
                return NotFound();

            var municipioVM = new MunicipiosViewModel()
            {
                Id = municipio.Id,
                Nome = municipio.Nome
            };

            return View(municipioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, MunicipiosViewModel municipioVM)
        {
            if (id == null)
                return BadRequest("ID invalido");

            if (id != municipioVM.Id)
                return NotFound();

            if(!ModelState.IsValid)
                return View(municipioVM);

            try
            {
                var municipio = new Municipio()
                {
                    Id = municipioVM.Id,
                    Nome = municipioVM.Nome
                };

                _context.Municipios.Update(municipio);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Município editado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            catch(DBConcurrencyException ex)
            {
                _logger.LogError(ex, "Não foi possível atualizar o registro");

                TempData["ErrorMessage"] = "Não foi possível realizar a edição, tente novamente mais tarde";

                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest("ID invalido");

            var municipio = await _context.Municipios.FindAsync(id);

            if (municipio == null)
                return NotFound();

            var municipioVM = new MunicipiosViewModel()
            {
                Id = municipio.Id,
                Nome = municipio.Nome
            };

            return View(municipioVM);
        }
    }
}
