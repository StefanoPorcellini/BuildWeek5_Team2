using ClinicaVeterinaria.Service.Intertface;
using ClinicaVeterinaria.Intertface;
using ClinicaVeterinaria.Services;
using ClinicaVeterinaria.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrazione dei servizi
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<VeterinaryClinicContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProprietarioService, ProprietarioService>();
builder.Services.AddScoped<IAnimaleService, AnimaleService>();
builder.Services.AddScoped<IVisitaService, VisitaService>();



// Registrazione dei servizi UtenteService e AuthenticationService
builder.Services.AddScoped<IUtenteService, UtenteService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/User/Login";
        opt.LogoutPath = "/User/Logout";
        opt.AccessDeniedPath = "/Home/AccessDenied";
    });

builder.Services.AddScoped<IUtenteService, UtenteService>();


// Configura il logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
