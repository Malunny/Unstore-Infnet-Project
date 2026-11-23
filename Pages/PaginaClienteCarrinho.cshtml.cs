using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Dados;

namespace UnstoreProjectADS.Pages;

[Authorize(Roles = "Cliente")]
public class PaginaClienteCarrinho : PageModel
{
    public Cliente? Cliente { get; set; }
    public Carrinho? Carrinho { get; set; }
    public async Task OnGetAsync([FromServices] ContextoDados contextoDados)
    {
        var clienteEmail = User.FindFirstValue(ClaimTypes.Email);

        if (clienteEmail == null)
            return;
        
        var cliente = await contextoDados.Clientes.FirstOrDefaultAsync(cliente => cliente.Email == clienteEmail);
        
        var carrinho = await contextoDados.Carrinhos
            .Include(carrinho => carrinho.ProdutosCarrinho)
            .ThenInclude(produto => produto.Produto)
            .FirstOrDefaultAsync(carrinho => carrinho.Id == cliente.CarrinhoId);
        
        Cliente = cliente;
        Carrinho = cliente?.Carrinho;
    }
}