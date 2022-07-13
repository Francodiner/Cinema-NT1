using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class EmpleadosController : Controller
    {
        private readonly MiContexto _context;
        private readonly UserManager<Usuario> _usermgr;

        public EmpleadosController(UserManager<Usuario> usrmgr, MiContexto context)
        {
            _context = context;
            _usermgr = usrmgr;
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Empleados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empleados.ToListAsync());
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {

                var usuario = await _usermgr.FindByEmailAsync(empleado.Email);
                if (usuario != null)
                {
                    ModelState.AddModelError(string.Empty, "Ese mail ya existe, use otro");
                    return View(empleado);
                }

                empleado.UserName = empleado.Email;
                empleado.NormalizedUserName = empleado.Email.ToUpper();
                empleado.NormalizedEmail = empleado.Email.ToUpper();
                empleado.FechaAlta = DateTime.Now;

                var resultadoCreacion = await _usermgr.CreateAsync(empleado, "Prueba_123");
                
                if (resultadoCreacion.Succeeded)
                {
                    var resultado = await _usermgr.AddToRoleAsync(empleado, "empleado");
                    var resultado1 = await _usermgr.AddClaimAsync(empleado, new Claim(ClaimTypes.GivenName, empleado.Apellido + " " + empleado.Nombre));
                    var resultado2 = await _usermgr.AddClaimAsync(empleado, new Claim(ClaimTypes.PrimarySid, empleado.Id.ToString()));
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
            return View(empleado);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var usuario = await _usermgr.FindByEmailAsync(empleado.Email);
                if (usuario != null)
                {
                    if (usuario.Id != empleado.Id)
                    {
                        ModelState.AddModelError(string.Empty, "Ese mail ya existe, use otro");
                        return View(empleado);
                    }
                    _context.Entry(usuario).State = EntityState.Detached;
                }

                try
                {
                    empleado.UserName = empleado.Email;
                    empleado.NormalizedUserName = empleado.Email.ToUpper();
                    empleado.NormalizedEmail = empleado.Email.ToUpper();

                    _context.Update(empleado);
                    await _context.SaveChangesAsync();

                    var claims = await _usermgr.GetClaimsAsync(empleado);
                    foreach (Claim c in claims)
                    {
                        if (c.Type == ClaimTypes.GivenName)
                        {
                            var respuesta = await _usermgr.RemoveClaimAsync(empleado, c);
                        }
                    }
                    var respuesta1 = await _usermgr.AddClaimAsync(empleado, new Claim(ClaimTypes.GivenName, empleado.Apellido + " " + empleado.Nombre));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
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
            return View(empleado);
        }

        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("SuperUsuario, Empleado"))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
