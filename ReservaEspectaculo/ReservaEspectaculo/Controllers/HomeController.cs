using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservaEspectaculo.Data;
using ReservaEspectaculo.Models;

namespace ReservaEspectaculo.Controllers
{
    public class HomeController : Controller
    {
        private readonly MiContexto _context;
        private readonly UserManager<Usuario> _usrmgr;
        private readonly SignInManager<Usuario> _signinmgr;

        public HomeController(MiContexto context, UserManager<Usuario> usrmgr, SignInManager<Usuario> signinmgr)
        {
            _context = context;
            _usrmgr = usrmgr;
            _signinmgr = signinmgr;
        }

        public async System.Threading.Tasks.Task<IActionResult> Index()
        {
            if (User.IsInRole("Cliente"))
            {
                ViewBag.HayReservas = _context.Reservas.Any(r => r.Activa == true && r.Funcion.Fecha > DateTime.Now && r.ClienteId == getClienteId());
            }
            
            if (!User.IsInRole("Empleado") && !User.IsInRole("SuperUsuario"))
            {
                DateTime fechaDesde = DateTime.Now.Date;
                DateTime fechaHasta = DateTime.Now.Date.AddDays(7);
                var miContexto = _context.Peliculas.Include(p => p.Funciones).Include(p => p.Genero)
                          .Select(f => new Pelicula
                          {
                              Id = f.Id,
                              FechaLanzamiento = f.FechaLanzamiento,
                              Titulo = f.Titulo,
                              Descripcion = f.Descripcion,
                              GeneroId = f.GeneroId,
                              Genero = f.Genero,
                              Funciones = (System.Collections.Generic.List<Funcion>)f.Funciones
                                                   .Where(p => p.Confirmada == true && p.Fecha.Date >= fechaDesde && p.Fecha.Date <= fechaHasta)
                          })
                          .OrderBy(p => p.Titulo);
                return View(await miContexto.ToListAsync());
            }
            else
            {
                var miContexto = _context.Peliculas.Include(p => p.Funciones).Include(p => p.Genero).OrderBy(p => p.Titulo);
                return View(await miContexto.ToListAsync());
            }
        }

        private int getClienteId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var clienteIdSt = claimsIdentity.FindFirst(ClaimTypes.PrimarySid);
            return int.Parse(clienteIdSt.Value);
        }
    }
}
