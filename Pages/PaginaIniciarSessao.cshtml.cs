using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Services;

namespace UnstoreProjectADS.Pages;

[AllowAnonymous]
public class PaginaIniciarSessao : PageModel
{
    [BindProperty] public LoginRequest Form { get; set; }
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync([FromServices] ServicoConta servicoConta)
    {
        var respostaIniciarSessao = await servicoConta.IniciarSessao(Form.Email, Form.Password);

        if (!respostaIniciarSessao.Ok)
        {
            TempData["ErrosInicioSessao"] = string.Join('\n', respostaIniciarSessao.MensagensErro);
            return Page();
        }

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, respostaIniciarSessao.Dados!);

        return Redirect("/Index");
    }
}