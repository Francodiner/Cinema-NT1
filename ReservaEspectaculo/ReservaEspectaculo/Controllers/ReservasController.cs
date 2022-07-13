using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservaEspectaculo.Data;
using ReservaEspectaculo.Models;
using ReservaEspectaculo.ViewModels;

namespace ReservaEspectaculo.Controllers
{
    public class ReservasController : Controller
    {
        private readonly MiContexto _context;

        public ReservasController(MiContexto context)
        {
            _context = context;
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Reservas
        public async Task<IActionResult> Index(string modo)
        {
            if (modo == null || modo.Equals("adelante"))
            {
                ViewBag.Consulta = "Consulta de reservas futuras";
                var reservas = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcion.Pelicula)
                               .Where( r => r.Funcion.Fecha.Date >= DateTime.Now.Date)
                               .OrderBy(p => p.Funcion.Pelicula.Titulo).ThenBy(p => p.Funcion.Fecha).ThenBy(p => p.Cliente.Apellido);
                return View(await reservas.ToListAsync());
            }
            else
            {
                ViewBag.Consulta = "Consulta de reservas pasadas";
                var reservas = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcion.Pelicula)
                    .Where(r => r.Funcion.Fecha.Date < DateTime.Now.Date)
                    .OrderBy(p => p.Funcion.Pelicula.Titulo).ThenBy(p => p.Funcion.Fecha).ThenBy(p => p.Cliente.Apellido);
                return View(await reservas.ToListAsync());
            }
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente).Include(r => r.Funcion.Pelicula)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

     
        //get
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> CancelarPorEmpleado(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.Include(p => p.Cliente).Include(p => p.Funcion.Pelicula).Where(p => p.Id == id).FirstOrDefaultAsync();
            if (reserva == null)
            {
                ModelState.AddModelError(string.Empty, "Reserva inexistente o no cancelable");
                return View(reserva);
            }
            if (!reserva.Activa || (reserva.Funcion.Fecha <= DateTime.Now))
            {
                ModelState.AddModelError(string.Empty, "Reserva no es cancelable");
                return View(reserva);
            }

            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> CancelarPorEmpleado(int id,Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }
            //libero el id de la reserva porque sino identity framework me da error cuando quiero tomar otra vez el mismo
            _context.Entry(reserva).State = EntityState.Detached;

            var reserva1 = await _context.Reservas.Include(p => p.Funcion.Pelicula).Where(p => p.Id == id && p.Activa == true && p.Funcion.Fecha > DateTime.Now).FirstOrDefaultAsync();
            if (reserva1 == null)
            {
                ModelState.AddModelError(string.Empty, "Reserva inexistente o no cancelable");
                return View(reserva);
            }

            reserva1.Activa = false;
            var funcion = _context.Funciones.Where(p => p.Id == reserva1.FuncionId).FirstOrDefault();
            funcion.ButacarDisponibles += reserva1.CantidadButacas;

            try
            {
                _context.Update(reserva1);
                _context.Update(funcion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(reserva.Id))
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

        [Authorize(Roles = ("Cliente"))]
        // GET: Reservas/Cancelar/5
        public IActionResult Cancelar(int? id)
        {

            Reserva reserva = _context.Reservas.Include(p => p.Funcion.Pelicula).Include(p => p.Funcion.Sala)
                             .Where(p => p.Id == id && p.ClienteId == getClienteId() && p.Activa == true).FirstOrDefault();

            if (reserva == null)
            {
                ModelState.AddModelError(string.Empty, "Reserva inexistente o dada de baja");
                return View();
            }

            if (reserva.Funcion.Fecha < DateTime.Now.AddHours(24))
            {
                ModelState.AddModelError(string.Empty, "usted a excedido el tiempo limite para cancelar");
            }

            return View(reserva);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Cliente"))]
        public async Task<IActionResult> Cancelar(Reserva reservaPant)
        {
            // como recibimos de la pantalla solo datos parciales descartamos el control sobre ese elemento y lo volvemos a leer
            _context.Entry(reservaPant).State = EntityState.Detached;
            Reserva reserva = _context.Reservas.Where(p => p.Id == reservaPant.Id && p.ClienteId == getClienteId() && p.Activa == true).FirstOrDefault();

            if (reserva == null)
            {
                ModelState.AddModelError(string.Empty, "Reserva inexistente o dada de baja");
                return View(reservaPant);
            }

            //leo la función por separado porque la tengo que actualizar
            Funcion funcion = _context.Funciones.Where(p => p.Id == reserva.FuncionId && p.Confirmada == true && p.Fecha > DateTime.Now.AddHours(24)).FirstOrDefault();

            if (funcion == null)
                {
                    ModelState.AddModelError(string.Empty, "Ya no se puede cancelar la funcion faltan menos de 24 hs.");
                    return View(reservaPant);
                }

            reserva.Activa = false;
            funcion.ButacarDisponibles += reserva.CantidadButacas;

            try
            {
                _context.Update(reserva);
                _context.Update(funcion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(reservaPant.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(ListarReservasCliente));

        }

        [Authorize(Roles = ("Cliente"))]
        //recibe el id de una película
        public IActionResult Reservar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //necesito el nombre de la pelicula para mostrarla así que la leo
            var pelicula = _context.Peliculas.Where(f => f.Id == id).FirstOrDefault();
            if (pelicula == null)
            {
                return NotFound();
            }

            DateTime fechaDesde = DateTime.Now.Date;
            DateTime fechaHasta = DateTime.Now.Date.AddDays(7);
            var fechas = _context.Funciones.Where(p => p.Confirmada == true && p.Fecha.Date >= fechaDesde && p.Fecha.Date <= fechaHasta && p.PeliculaId == id)
                          //no se porqué este distinct no anda
                          .Select(p => new { Fecha = p.Fecha.ToString("dd/MM/yyyy") }).Distinct().ToList();
                          //.Select(p => new { Fecha = p.Fecha.Date}).Distinct();

            //selecciona las fechas para mostrar en el combo
            ViewData["Fechas"] = new SelectList(
                 // vuelvo a hacer el distinct que acá si anda
                 fechas.Distinct()
                , "Fecha", "Fecha");

            ReservaCliente reservaCliente = new ReservaCliente() {PeliculaId = (int)id, PeliculaTitulo = pelicula.Titulo};

            //si me llamaron por redirect de otro lado y me pasaron error lo muestro
            if (!string.IsNullOrEmpty(((string)TempData["error"])))
            {
                ModelState.AddModelError(string.Empty, (string)TempData["error"]);
            }

            return View(reservaCliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Cliente"))]
        public IActionResult Reservar(ReservaCliente reservaCliente)
        {

            if (ModelState.IsValid)
            {
                var funciones = _context.Funciones.Include(p => p.Sala).Include(p => p.Pelicula)
                               .Where(p => p.PeliculaId == reservaCliente.PeliculaId && p.Confirmada == true && p.Fecha.Date == reservaCliente.Fecha.Date 
                                && p.ButacarDisponibles >= reservaCliente.CantidadButacas);
                ViewData["Butacas"] = reservaCliente.CantidadButacas;
                if (funciones.Count() == 0)
                {
                    TempData["error"] = "No hay funciones disponibles para la cantidad de butacas que desea reservar";
                    //hago redirect para que arranque de vuelta con el get y no tener que hacer de vuelta el tema de las fechas acá
                    return RedirectToAction(nameof(Reservar), new { id = reservaCliente.PeliculaId });
                }
                return View(nameof(Confirmar), funciones.ToList());
            }

            return View(reservaCliente);
        }

        [Authorize(Roles = ("Cliente"))]
        public async Task<IActionResult> Confirmar(int IdFuncion, int CantButacas)
        {

            Funcion funcion = _context.Funciones.Where(p => p.Id == IdFuncion).FirstOrDefault();

            if (hayReservasActivas(getClienteId()))
            {
                TempData["error"] = "Ya tiene reservas activas, cancele para otra reserva";
                return RedirectToAction(nameof(Reservar), new { id = funcion.PeliculaId });
            }

            if (funcion.ButacarDisponibles < CantButacas)
            {
                TempData["error"] = "No hay tantas butacas disponibles";
                return RedirectToAction(nameof(Reservar), new { id = funcion.PeliculaId });
            }

            funcion.ButacarDisponibles -= CantButacas;

            Reserva reserva = new Reserva()
            {
                FechaAlta = DateTime.Now,
                ClienteId = getClienteId(),
                FuncionId = IdFuncion
                                            ,
                CantidadButacas = CantButacas,
                Activa = true
            };

            try
            {
                _context.Reservas.Add(reserva);
                _context.Update(funcion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(reserva.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ListarReservasCliente));
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public IActionResult ListarReservasEmpleado(int id)
        {

            var reserva = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcion.Pelicula).Include(p => p.Funcion.Sala)
                          .Where(p => p.FuncionId == id).OrderByDescending(p => p.Activa).ThenBy(p => p.Cliente.Apellido);

            //esto ya no va a pasar porque ahora ponemos el vínculo que lo llama solo si la función tiene reservas
            if (reserva.Count() == 0)
            {
                // recupero la película de la que la función depende
                var funcion = _context.Funciones.Where(p => p.Id == id).FirstOrDefault();
                TempData["Mensaje"] = "No hay Reservas disponibles para esta funcion";
                return RedirectToAction("ListarFunciones", "Funciones", new { id = funcion.PeliculaId });
            }

            return View(reserva.ToList());
        }

        [Authorize(Roles = ("Cliente"))]
        public IActionResult ListarReservasCliente()
        {

            var reserva = _context.Reservas.Include(r => r.Cliente).Include(r=> r.Funcion.Pelicula).Include(p => p.Funcion.Sala).Where(p => p.ClienteId == getClienteId())
                .OrderBy(p => p.Funcion.Fecha).ThenBy(p => p.Funcion.Pelicula.Titulo); 

            return View(reserva.ToList());
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public IActionResult ListarRecaudacion(int id)
        {

           
            var recaudaciones = _context.Reservas.Where(p => p.Funcion.PeliculaId == id && p.Activa == true)
                    .Select(cl => new Recaudacion
                    {
                        TituloPelicula = cl.Funcion.Pelicula.Titulo,
                        Mes = cl.Funcion.Fecha.ToString("MMM", CultureInfo.CurrentCulture),
                        Anio = cl.Funcion.Fecha.Year,
                        Importe = cl.Funcion.Sala.TipoSala.Precio * cl.CantidadButacas
                    }).ToList();

            // agrupo por año y mes (pongo el título para que me viaje a la pantalla)
            List<Recaudacion> salida = ((from s in recaudaciones
                              group s by new { s.TituloPelicula, s.Mes, s.Anio } into g
                              select new Recaudacion { TituloPelicula =g.Key.TituloPelicula
                                                     , Mes = g.Key.Mes
                                                     , Anio = g.Key.Anio
                                                     , Importe = g.Sum(s => s.Importe) })).ToList();

            //armo el total general
            var total = (from s in recaudaciones
                         select s.Importe).Sum();
            salida.Add(new Recaudacion() {TituloPelicula= "",  Mes = "Total", Anio = null, Importe = total });

            return View(salida);
        }

      
        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }

        private bool hayReservasActivas(int id)
        {
            return _context.Reservas.Any(p => p.Cliente.Id == id && p.Activa == true && p.Funcion.Fecha > DateTime.Now);

        }

        private int getClienteId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var clienteIdSt = claimsIdentity.FindFirst(ClaimTypes.PrimarySid);
            return int.Parse(clienteIdSt.Value);
        }
    }
}
