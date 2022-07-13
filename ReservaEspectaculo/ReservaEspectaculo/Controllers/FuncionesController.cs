using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservaEspectaculo.Data;
using ReservaEspectaculo.Models;

namespace ReservaEspectaculo.Controllers
{
    public class FuncionesController : Controller
    {
        private readonly MiContexto _context;
        private readonly UserManager<Usuario> _usrmgr;

        public FuncionesController(MiContexto context, UserManager<Usuario> usrmgr)
        {
            _context = context;
            _usrmgr = usrmgr;
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Funciones
        public async Task<IActionResult> Index()
        {

            var Funciones = _context.Funciones.Include(p => p.Pelicula).Include(p => p.Sala).Include(p => p.Reservas)
              .Select(f => new Funcion
              {
                  Id = f.Id,
                  Fecha = f.Fecha,
                  Descripcion = f.Descripcion,
                  ButacarDisponibles = f.ButacarDisponibles,
                  Confirmada = f.Confirmada,
                  PeliculaId = f.PeliculaId,
                  Pelicula = f.Pelicula,
                  SalaId = f.SalaId,
                  Sala = f.Sala,
                  Reservas = (List<Reserva>)f.Reservas.Where(p => p.Activa == true && p.FuncionId == f.Id)
              })
              .OrderBy(p => p.Pelicula.Titulo).ThenBy(p => p.Fecha);

            return View(await Funciones.ToListAsync());
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Funciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Funciones/Create
        public IActionResult Create()
        {
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo");
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero");
            return View();
        }

        // POST: Funciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> Create(Funcion funcion)
        {
            if (ModelState.IsValid)
            {

                var sala = _context.Salas.Include(p => p.TipoSala).Where(p => p.Id == funcion.SalaId).FirstOrDefault();

                if (sala.CapacidadButacas < funcion.ButacarDisponibles)
                {
                    ModelState.AddModelError(string.Empty, "No puede superar la cantidad de butacas disponibles en la sala");
                }
                else
                {
                    _context.Add(funcion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero", funcion.SalaId);
            return View(funcion);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Funciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones.FirstOrDefaultAsync(m => m.Id == id);

            if (funcion == null)
            {
                return NotFound();
            }
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero", funcion.SalaId);
            return View(funcion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> Edit(int id, Funcion funcion)
        {
            if (id != funcion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sala = _context.Salas.Where(p => p.Id == funcion.SalaId).FirstOrDefault();
                    if (sala.CapacidadButacas < funcion.ButacarDisponibles)
                    {
                        ModelState.AddModelError(string.Empty, "No puede superar la cantidad de butacas disponibles en la sala");
                        ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo", funcion.PeliculaId);
                        ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero", funcion.SalaId);
                        return View(funcion);
                    }

                    //chequeo de vuelta por si me dieron de alta una reserva mientras el tipo pensaba la modificación
                    var reservas = _context.Reservas.Where(r => r.Funcion.Id == funcion.Id && r.Activa == true);
                    if (reservas.Count() != 0)
                    {
                        ModelState.AddModelError(string.Empty, "No se puede modificar, tiene reservas");
                        ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo", funcion.PeliculaId);
                        ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero", funcion.SalaId);
                        return View(funcion);
                    }

                    _context.Update(funcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionExists(funcion.Id))
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
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Descripcion", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Id", funcion.SalaId);
            return View(funcion);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Funciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var funcion = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .Include(f => f.Reservas)
                .FirstOrDefaultAsync(m => m.Id == id);

            var reservas = _context.Reservas.Where(r => r.Funcion.Id == id && r.Activa == true);

            if (funcion == null)
            {
                return NotFound();
            }
            if (reservas.Count() > 0)
            {
                ModelState.AddModelError(string.Empty, "No se puede dar de baja, tiene reservas");
            }
            return View(funcion);
        }

        // POST: Funciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var funcion = await _context.Funciones.FindAsync(id);
            var funcion = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .Include(f => f.Reservas)
                .FirstOrDefaultAsync(m => m.Id == id);

            var reservas = _context.Reservas.Where(r => r.Funcion.Id == id && r.Activa == true);

            if (reservas.Count() > 0)
            {
                ModelState.AddModelError(string.Empty, "No se puede dar de baja, tiene reservas");
                return View(funcion);
            }
            _context.Funciones.Remove(funcion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuncionExists(int id)
        {
            return _context.Funciones.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ListarFunciones(int id)
        {

            var funciones = _context.Funciones.Include(p => p.Pelicula).Include(p => p.Sala.TipoSala).Include(p => p.Reservas).Where(p => p.PeliculaId == id);
 
            if (!User.IsInRole("Empleado") && !User.IsInRole("SuperUsuario"))
            {
                DateTime fechaDesde = DateTime.Now.Date;
                DateTime fechaHasta = DateTime.Now.Date.AddDays(7);
                funciones = funciones.Where(p => p.Confirmada == true && p.Fecha.Date >= fechaDesde && p.Fecha.Date <= fechaHasta);
            }

            // Esto no va a pasar mas porque ahora ponemos el vínculo que llama a este método solo si la película tiene funciones
            if (funciones.Count() == 0)
            {
                TempData["Mensaje"] = "No hay funciones disponibles para esta pelicula";
                return RedirectToAction(nameof(Index));
            }

            funciones = funciones.OrderBy(P => P.Fecha);

            return View(await funciones.ToListAsync());
        }


    }
}
