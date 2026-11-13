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

    public async Task<IActionResult> OnPostAsync([FromServices] ServicoConta servicoConta)
    {
        var usuarioCriado = await servicoConta.Registrar(Form);

        if (!usuarioCriado)
            return BadRequest();

        var identidadeUsuario = await servicoConta.IniciarSessao(Form.Email, Form.Senha);

        if (identidadeUsuario == null)
            return BadRequest("Alguma problema ocorreu ao entrar na conta, tente iniciar sessão com email e senha.");

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identidadeUsuario);
        
        return RedirectToPage("./Index");
    }
}