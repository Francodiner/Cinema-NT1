using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservaEspectaculo.Models;
using Microsoft.AspNetCore.Authorization;

namespace ReservaEspectaculo.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<Rol> _rolmgr;

        public RolesController(RoleManager<Rol> rolmgr)
        {
            this._rolmgr = rolmgr;
        }
        public IActionResult Index()
        {
            ViewBag.Roles = _rolmgr.Roles.ToList();
            return View(ViewBag.Roles);
        }

        public async Task<IActionResult> CrearRolesBasicos()
        {
            Rol rol1 = new Rol() { Name="Empleado"};
            Rol rol2 = new Rol() { Name = "Cliente" };
            Rol rol3 = new Rol() { Name = "SuperUsuario" };

            var resultado1 = await _rolmgr.CreateAsync(rol1);
            var resultado2 = await _rolmgr.CreateAsync(rol2);
            var resultado3 = await _rolmgr.CreateAsync(rol3);            

            return RedirectToAction("Index");
        }
    }
}