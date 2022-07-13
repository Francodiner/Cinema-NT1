using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaEspectaculo.Data;
using ReservaEspectaculo.Models;

namespace ReservaEspectaculo.Controllers
{
    public class ClientesController : Controller
    {
        private readonly MiContexto _context;
        private readonly UserManager<Usuario> _usermgr;

        public ClientesController(UserManager<Usuario> usrmgr, MiContexto micontexto)
        {
            _context = micontexto;
            _usermgr = usrmgr;
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {

                var usuario = await _usermgr.FindByEmailAsync(cliente.Email);
                if (usuario != null)
                {
                    ModelState.AddModelError(string.Empty, "Ese mail ya existe, use otro");
                    return View(cliente);
                }

                cliente.UserName = cliente.Email;
                cliente.NormalizedUserName = cliente.Email.ToUpper();
                cliente.NormalizedEmail = cliente.Email.ToUpper();
                cliente.FechaAlta = DateTime.Now;

                var resultadoCreacion = await _usermgr.CreateAsync(cliente, "Prueba_123");

                if (resultadoCreacion.Succeeded)
                {
                    var resultado = await _usermgr.AddToRoleAsync(cliente, "Cliente");
                    var resultado1 = await _usermgr.AddClaimAsync(cliente, new Claim(ClaimTypes.GivenName, cliente.Apellido + " " + cliente.Nombre));
                    var resultado2 = await _usermgr.AddClaimAsync(cliente, new Claim(ClaimTypes.PrimarySid, cliente.Id.ToString()));
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var err in resultadoCreacion.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                }
            }
            return View(cliente);

        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                 }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
