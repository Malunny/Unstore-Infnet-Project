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
        
        if (db.Set<Produto>().ToList().Count < 15)
        {
            db.Set<Produto>().AddRange(
                new Produto 
                {
                    Nome = "Cortina Blackout de Tecido 2,80x2,30m", 
                    Descricao = "Cortina corta-luz em tecido encorpado, ideal para bloquear 90% da claridade no quarto ou sala.", 
                    Valor = 249.90m, 
                    QuantidadeEstoque = 30,
                },
                new Produto 
                {
                    Nome = "Persiana Vertical em PVC Branco", 
                    Descricao = "Persiana prática e de fácil limpeza para ambientes corporativos e residenciais, medida 1,50x1,60m.", 
                    Valor = 189.00m, 
                    QuantidadeEstoque = 20,
                },
                new Produto 
                {
                    Nome = "Piso Laminado Carvalho Creta (Caixa 2,2m²)", 
                    Descricao = "Piso laminado de alta resistência com sistema de encaixe click, fácil instalação e visual amadeirado.", 
                    Valor = 145.50m, 
                    QuantidadeEstoque = 50,
                },
                new Produto 
                {
                    Nome = "Cortina Voil com Forro de Microfibra", 
                    Descricao = "Cortina elegante com tecido leve e translúcido (voil) acompanhada de forro protetor.", 
                    Valor = 199.90m, 
                    QuantidadeEstoque = 25,
                },
                new Produto 
                {
                    Nome = "Persiana Rolô Tela Solar 5% Cinza", 
                    Descricao = "Protege contra raios UV e reduz o calor do ambiente mantendo a visibilidade externa.", 
                    Valor = 320.00m, 
                    QuantidadeEstoque = 15,
                },
                new Produto 
                {
                    Nome = "Piso Vinílico Régua Colado (Caixa 3,3m²)", 
                    Descricao = "Piso vinílico durável, confortável ao caminhar e resistente à umidade para áreas internas.", 
                    Valor = 210.00m, 
                    QuantidadeEstoque = 40,
                },
                new Produto 
                {
                    Nome = "Rodapé de Poliestireno Branco 10cm (Barra 2m)", 
                    Descricao = "Rodapé resistente a água, imune a pragas e com acabamento moderno fosco.", 
                    Valor = 39.90m, 
                    QuantidadeEstoque = 120,
                },
                new Produto 
                {
                    Nome = "Papel de Parede Adesivo Texturizado Cimento Queimado", 
                    Descricao = "Rolo de papel de parede autocolante com efeito industrial moderno, fácil aplicação.", 
                    Valor = 79.90m, 
                    QuantidadeEstoque = 60,
                },
                new Produto 
                {
                    Nome = "Argamassa AC-II para Porcelanatos e Cerâmicas (20kg)", 
                    Descricao = "Argamassa colante de alta aderência para assentamento de pisos e revestimentos internos e externos.", 
                    Valor = 32.50m, 
                    QuantidadeEstoque = 100,
                },
                new Produto 
                {
                    Nome = "Rejunte Acrílico Cinza Platinano (5kg)", 
                    Descricao = "Rejunte pronto para uso, impermeável, flexível e resistente a mofo e manchas.", 
                    Valor = 48.00m, 
                    QuantidadeEstoque = 80,
                },
                new Produto 
                {
                    Nome = "Cortina Romana em Linho Natural", 
                    Descricao = "Cortina sofisticada com gomos estruturados que recolhem verticalmente, conferindo elegância ao ambiente.", 
                    Valor = 450.00m, 
                    QuantidadeEstoque = 10,
                },
                new Produto 
                {
                    Nome = "Porcelanato Retificado Polido 60x60cm (Caixa 2,1m²)", 
                    Descricao = "Placa cerâmica de alto brilho com bordas retas que permitem juntas menores e acabamento impecável.", 
                    Valor = 115.00m, 
                    QuantidadeEstoque = 45,
                },
                new Produto 
                {
                    Nome = "Perfil de Transparência / Acabamento para Piso (2m)", 
                    Descricao = "Perfil em alumínio ou PVC para união de ambientes com diferentes tipos de pisos.", 
                    Valor = 25.00m, 
                    QuantidadeEstoque = 90,
                },
                new Produto 
                {
                    Nome = "Persiana Horizontal de Alumínio 25mm", 
                    Descricao = "Lâminas em alumínio duráveis com controle preciso de luminosidade, medida 1,20x1,40m.", 
                    Valor = 149.90m, 
                    QuantidadeEstoque = 22,
                },
                new Produto 
                {
                    Nome = "Kit Instalação Completo para Varão de Cortina", 
                    Descricao = "Kit contendo suportes, parafusos, buchas e ponteiras decorativas para fixação firme de varões duplos ou simples.", 
                    Valor = 59.90m, 
                    QuantidadeEstoque = 55,
                }
            );
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
