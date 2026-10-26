using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.Pages.Requests;
using UnstoreProjectADS.Services;

namespace UnstoreProjectADS.Pages;

public class RegisterPage : PageModel
{
    [BindProperty] public bool IsInvalid { get; set; } = false;
    [BindProperty] public RegisterRequest Form { get; set; }
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync([FromServices] AppDbContext dbContext,
        [FromServices] JwtTokenService tokenService)
    {
        Cliente cliente = new();
        
        cliente.Nome = Form.Nome;
        cliente.Email = Form.Email;
        cliente.Senha = Form.Senha;

        if (cliente.Nome is null || cliente.Email is null || cliente.Senha is null)
        {
            IsInvalid = true;
            return BadRequest();
        }

        var endereco = new Endereco();

        endereco.Rua = Form.Rua;
        endereco.CEP = Form.CEP;
        endereco.Complemento = Form.Complemento;
        endereco.Estado = Form.Estado;
        endereco.Municipio = Form.Municipio;
        endereco.Numero = Form.Numero;
        
        if (endereco.Estado is null || endereco.Municipio is null 
                                    || endereco.Rua is null ||
                                    endereco.Complemento is null ||
                                    endereco.CEP is null ||
                                    endereco.Numero < 1)
        {
            IsInvalid = true;
            return BadRequest();
        }
        
        cliente.Enderecos.Add(endereco);

        await dbContext.Clientes.AddAsync(cliente);
        await dbContext.SaveChangesAsync();
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, Form.Email),
        };
        
        claims.Add(new Claim(ClaimTypes.Role, "Cliente"));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        
        return RedirectToPage("/Index");
    }
}