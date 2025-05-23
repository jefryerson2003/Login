using EmpleadosApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// 1) Servicios MVC y repositorios
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<EmpleadoRepository>();
builder.Services.AddScoped<UsuarioRepository>();

// 2) Cookie Authentication
builder.Services
  .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options => {
      options.LoginPath  = "/Account/Login";
      options.LogoutPath = "/Account/Logout";
      options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
  });

// 3) Authorization por roles
builder.Services.AddAuthorization(options => {
    options.AddPolicy("RequireAdmin", 
        policy => policy.RequireRole("Administrator"));
    options.AddPolicy("RequireView", 
        policy => policy.RequireRole("Administrator","Coordinator"));
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// 4) Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();   // **antes** de Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Empleados}/{action=Index}/{id?}");

app.Run();
