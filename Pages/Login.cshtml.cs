using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.Services;

namespace UnstoreProjectADS.Pages;

public class LoginPage : PageModel
{
    [BindProperty]
    public LoginRequest Form { get; set; }
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync([FromServices] JwtTokenService jwtTokenService, [FromServices] AppDbContext appDbContext)
    {
        Usuario? user = await appDbContext.Clientes.FirstOrDefaultAsync(cliente => cliente.Email == Form.Email);
        
        if (user == null)
            user = await appDbContext.Funcionarios
                .Include(funcionario => funcionario.Cargo)
                .FirstOrDefaultAsync(funcionario => funcionario.Email == Form.Email);

        if (user == null)
            return BadRequest();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, Form.Email),
        };

        var role = "Cliente";

        if (user is Funcionario funcionario)
            role = funcionario.Cargo.Nome;
        
        claims.Add(new Claim(ClaimTypes.Role, role));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return Redirect("/Index");
    }
}