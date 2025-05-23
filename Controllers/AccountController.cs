using EmpleadosApp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EmpleadosApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuarioRepository _repo;
        public AccountController(UsuarioRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
            => View(model: returnUrl ?? "/Empleados");

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl)
        {
            var (user, roles) = _repo.Validate(username, password);
            if (user == null)
            {
                ModelState.AddModelError("", "Usuario o contraseña inválida");
                return View(model: returnUrl);
            }

            // Crea claims
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
