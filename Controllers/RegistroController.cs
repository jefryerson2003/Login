using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpleadosApp.Controllers
{
    [Authorize(Roles = "Coordinator")]
    public class RegistroController : Controller
    {
        public IActionResult Formulario()
        {
            return View();
        }
    }
}
