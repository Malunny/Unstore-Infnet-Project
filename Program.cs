using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UnstoreProjectADS;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<JwtTokenService>();
EnviromentVariables.AdminPassord = builder.Configuration["Admin:Password"] ?? throw new ArgumentNullException("Admin:Passord");
EnviromentVariables.AdminName = builder.Configuration["Admin:Name"] ?? throw new ArgumentNullException("Admin:Name");
EnviromentVariables.AdminEmail = builder.Configuration["Admin:Email"] ??  throw new ArgumentNullException("Admin:Email");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db").UseSeeding((db, _) =>
    {
        if (!db.Set<Cargo>().Any())
        {
            var cargos = new List<Cargo>
            {
                new Cargo { Nome = CargosNome.AdministradorEstoque.PegarNomeCargo() },
                new Cargo { Nome = CargosNome.Repositor.PegarNomeCargo() },
                new Cargo { Nome = CargosNome.Financeiro.PegarNomeCargo() },
                new Cargo { Nome = CargosNome.Logistica.PegarNomeCargo() },
                new Cargo { Nome = CargosNome.Vendas.PegarNomeCargo() },
                new Cargo { Nome = CargosNome.SAC.PegarNomeCargo() },
                new Cargo { Nome = CargosNome.AdministradorSistema.PegarNomeCargo() }
            };
            db.Set<Cargo>().AddRange(cargos);
            db.SaveChanges();

            var administradorSistema = new Funcionario();
            administradorSistema.Nome = EnviromentVariables.AdminName;
            administradorSistema.Email = EnviromentVariables.AdminEmail;
            administradorSistema.Senha = EnviromentVariables.AdminPassord;

            administradorSistema.Cargo =
                cargos.First(cargo => cargo.Nome == CargosNome.AdministradorSistema.PegarNomeCargo());

            db.Set<Funcionario>().Add(administradorSistema);
            db.SaveChanges();
        }
    }));

var secretKey = Encoding.ASCII.GetBytes(builder.Configuration["JwtToken"] ?? throw new ArgumentNullException("JwtToken:SecretKey"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
