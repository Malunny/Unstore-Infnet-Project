using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Core.Services;
using UnstoreProjectADS.Core.Services.Compartilhado;
using UnstoreProjectADS.Pages.Pedidos;
using UnstoreProjectADS.Pages.Pedidos.Registrar;

namespace UnstoreProjectADS.Pages;

public class PaginaRegistrarUsuario : PageModel
{
    [BindProperty] public bool IsInvalid { get; set; } = false;
    [BindProperty] public PedidoDeRegistroCliente Form { get; set; } = new();
    [BindProperty] public RespostaServico RespostaValidacao { get; set; } = new();
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync([FromServices] ServicoConta servicoConta)
    {
        if (!ModelState.IsValid)
            return Page();
        
        RespostaValidacao = await servicoConta.RegistrarClienteAsync(Form);

        if (!RespostaValidacao.Ok)
            return Page();

        var identidadeUsuario = await servicoConta.IniciarSessao(Form.Email, Form.Senha);

        if (!identidadeUsuario.Ok)
        {
            RespostaValidacao.AdicionarErro("Alguma problema ocorreu ao entrar na conta, tente iniciar sessão com email e senha.");
            return Page();
        }

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identidadeUsuario.Dados!);
        
        return Redirect("/Index");
    }
}