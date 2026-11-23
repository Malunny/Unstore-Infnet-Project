using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Core.Services.Compra;
using UnstoreProjectADS.Core.Services.Compra.Respostas;
using UnstoreProjectADS.Dados.Repositorios.Compras;
using UnstoreProjectADS.Dados.Repositorios.Usuarios;

namespace UnstoreProjectADS.Pages;

public class PaginaClienteCompras : PageModel
{
    public List<RespostaCompraDetalhada> Compras { get; set; } = new();
    public bool UsuarioValido { get; set; }
    private readonly RepositorioCompra _repositorioCompra;
    private readonly RepositorioUsuario _repositorioUsuario;
    private readonly ServicoCompra _servicoCompra;

    public PaginaClienteCompras(RepositorioCompra repositorioCompra,
        RepositorioUsuario repositorioUsuario,
        ServicoCompra servicoCompra)
    {
        _repositorioCompra = repositorioCompra;
        _repositorioUsuario = repositorioUsuario;
        _servicoCompra = servicoCompra;
    }
    
    public async Task OnGetAsync()
    {
        var emailUsuario = User.ColetarEmail();

        if (emailUsuario == null)
        {
            UsuarioValido = false;
            return;
        }

        UsuarioValido = true;
        
        var usuario = await _repositorioUsuario.ObterPorEmailAsync(emailUsuario);

        if (usuario == null)
        {
            UsuarioValido = false;
            return;
        }
        
        var compras = await _repositorioCompra.ObterPorClienteIdAsync(usuario.Id);

        Compras = (await _servicoCompra.PegarVariosDadosDetalhadasCompra(compras.Select(c => c.Id))).ToList();
    }
}