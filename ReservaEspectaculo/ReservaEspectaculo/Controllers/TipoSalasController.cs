using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaEspectaculo.Data;
using ReservaEspectaculo.Models;

namespace ReservaEspectaculo.Controllers
{
    public class TipoSalasController : Controller
    {
        private readonly MiContexto _context;

        public TipoSalasController(MiContexto context)
        {
            _context = context;
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: TipoSalas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoSalas.ToListAsync());
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: TipoSalas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoSala == null)
            {
                return NotFound();
            }

            return View(tipoSala);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: TipoSalas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoSalas/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio")] TipoSala tipoSala)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoSala);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoSala);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: TipoSalas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas.FindAsync(id);
            if (tipoSala == null)
            {
                return NotFound();
            }
            return View(tipoSala);
        }

        // POST: TipoSalas/Edit/5
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio")] TipoSala tipoSala)
        {
            if (id != tipoSala.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoSala);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoSalaExists(tipoSala.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipoSala);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: TipoSalas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoSala == null)
            {
                return NotFound();
            }

            return View(tipoSala);
        }

        // POST: TipoSalas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoSala = await _context.TipoSalas.FindAsync(id);
            _context.TipoSalas.Remove(tipoSala);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoSalaExists(int id)
        {
            return _context.TipoSalas.Any(e => e.Id == id);
        }
    }
}
