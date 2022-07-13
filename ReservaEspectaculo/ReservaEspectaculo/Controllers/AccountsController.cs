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
using ReservaEspectaculo.ViewModels;

namespace ReservaEspectaculo.Controllers
{
    public class AccountsController : Controller

    {
        private readonly MiContexto _context;
        private readonly UserManager<Usuario> _usrmgr;
        private readonly SignInManager<Usuario> _signinmgr;
        public AccountsController(MiContexto context, UserManager<Usuario> usrmgr, SignInManager<Usuario> signinmgr)
        {
            _context = context;
            _usrmgr = usrmgr;
            _signinmgr = signinmgr;
        }

        public async Task<IActionResult> EmailDisponible(string email)
        {
            var usuario = await _usrmgr.FindByEmailAsync(email);
            if (usuario == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"El email {email} ya esta en uso.");
            }
        }
        //get
        public IActionResult RegistrarCliente()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegistrarCliente(RegistroCliente modelo)
        {
            if (ModelState.IsValid)
            {

                Cliente cliente = new Cliente();

                cliente.UserName = modelo.Email;
                cliente.NormalizedUserName = modelo.Email.ToUpper();
                cliente.Email = modelo.Email;
                cliente.NormalizedEmail = modelo.Email.ToUpper();                
                cliente.Nombre = modelo.Nombre;
                cliente.Apellido = modelo.Apellido;
                cliente.Direccion = modelo.Direccion;
                cliente.Dni = modelo.Dni;
                cliente.FechaAlta = DateTime.Now;

                var resultadoCreacion = await _usrmgr.CreateAsync(cliente, modelo.Password);

                if (resultadoCreacion.Succeeded)
                {
                    var resultado = await _usrmgr.AddToRoleAsync(cliente, "Cliente");
                    var resultado1 = await _usrmgr.AddClaimAsync(cliente, new Claim(ClaimTypes.GivenName, cliente.Apellido + " " + cliente.Nombre));
                    var resultado2 = await _usrmgr.AddClaimAsync(cliente, new Claim(ClaimTypes.PrimarySid, cliente.Id.ToString()));
                    //logoneamos al usuario que se acaba de registrar
                    var resultado3 = await _signinmgr.PasswordSignInAsync(cliente.Email, modelo.Password, true, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var err in resultadoCreacion.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                }
            }
            return View(modelo);
        }

        [Authorize(Roles = ("Cliente"))]
        // GET: Clientes/Edit/5
        public async Task<IActionResult> DatosPersonales()
        {
            var cliente = await _context.Clientes.FindAsync(getClienteId());
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Cliente"))]
        public async Task<IActionResult> DatosPersonales(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                //var usuario = await _usrmgr.FindByEmailAsync(cliente.Email);
                //if (usuario != null )
                //{
                //    if (usuario.Id != cliente.Id)
                //    {
                //        ModelState.AddModelError(string.Empty, "Ese mail ya existe, use otro");
                //        return View(cliente);
                //    }
                //    _context.Entry(usuario).State = EntityState.Detached;
                //}

                try
                {
                    //cliente.UserName = cliente.Email;
                    //cliente.NormalizedUserName = cliente.Email.ToUpper();
                    //cliente.NormalizedEmail = cliente.Email.ToUpper();

                    _context.Update(cliente);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
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
            }
            return View(cliente);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signinmgr.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login(string returnurl)
        {

            TempData["returnurl"] = returnurl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login modelo)
        {

            var returnurl = TempData["returnurl"] as string;

            if (ModelState.IsValid)
            {
                var resultado = await _signinmgr.PasswordSignInAsync(modelo.Email,  modelo.Password, modelo.Rememberme,  false);

                if (resultado.Succeeded)
                {
                    if (returnurl != null)
                    {
                        //Tengo que volver a otro lado.
                        return Redirect(returnurl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Inicio de sesión inválido.");
            }

            return View();
        }
        public IActionResult AccesoDenegado(string returnurl)
        {
            TempData["returnurl"] = returnurl;

            return View();
        }
        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }

        private int getClienteId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var clienteIdSt = claimsIdentity.FindFirst(ClaimTypes.PrimarySid);
            return int.Parse(clienteIdSt.Value);
        }

    }
}
