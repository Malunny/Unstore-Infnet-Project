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

public class PaginaIniciarSessao : PageModel
{
    [BindProperty]
    public LoginRequest Form { get; set; }
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync([FromServices] ServicoConta servicoConta)
    {
        var identidadeUsuario = await servicoConta.IniciarSessao(Form.Email, Form.Password);

        if (identidadeUsuario == null)
            return BadRequest();
        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identidadeUsuario);

        return Redirect("/Index");
    }
}