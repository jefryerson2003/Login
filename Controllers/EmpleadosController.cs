using EmpleadosApp.Data;
using EmpleadosApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpleadosApp.Controllers
{
    // Solo Administrators y Coordinators pueden ver el listado
    [Authorize(Policy = "RequireView")]
    public class EmpleadosController : Controller
    {
        private readonly EmpleadoRepository _repo;
        public EmpleadosController(EmpleadoRepository repo) => _repo = repo;

        // GET: /Empleados
        public IActionResult Index()
        {
            var lista = _repo.GetAll();
            return View(lista);
        }

        // GET: /Empleados/Create
        // Solo Administrators pueden acceder al formulario de creación
        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Create() => View();

        // POST: /Empleados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Create(Empleado e)
        {
            if (!ModelState.IsValid) 
                return View(e);

            _repo.Add(e);
            return RedirectToAction(nameof(Index));
        }
    }
}
