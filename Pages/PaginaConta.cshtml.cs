using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Dados.Repositorios.Usuarios;

namespace UnstoreProjectADS.Pages;

[Authorize]
public class PaginaConta : PageModel
{
    public Usuario? Usuario { get; set; }
    public bool EstaEmSessao => Usuario != null;

    private readonly RepositorioUsuario _repositorioUsuario;
    
    public PaginaConta(RepositorioUsuario repositorioUsuario)
    {
        _repositorioUsuario =  repositorioUsuario;
    }
    
    public async Task OnGetAsync()
    {
        var nomeUsuario = User.ColetarEmail();
        
        if (nomeUsuario != null)
            Usuario = await _repositorioUsuario.ObterPorEmailAsync(nomeUsuario);
    }
}