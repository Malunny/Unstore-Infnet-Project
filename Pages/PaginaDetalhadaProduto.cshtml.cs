using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Core.Services;
using UnstoreProjectADS.Dados;

namespace UnstoreProjectADS.Pages;

public class PaginaDetalhadaProduto : PageModel
{
    public Produto? Produto { get; set; }
    public async Task OnGetAsync([FromRoute] int produtoId, [FromServices] ServicoProduto servicoProduto)
    {
        var produto = await servicoProduto.ObterPorId(produtoId);
        
        if (produto == null)
            return;
        
        Produto = produto;
    }

    public IActionResult OnPostRetornar()
    {
        return RedirectToPage("/PaginaListagemProdutos");
    }
    
    public async Task<IActionResult> OnPostAdicionarCarrinhoAsync([FromRoute] int produtoId,
        [FromServices] ContextoDados contextoDados,
        [FromServices] ServicoProduto servicoProduto)
    {
        var usuarioEmail = User.FindFirstValue(ClaimTypes.Email);
        if (usuarioEmail == null)
            return Redirect("/iniciarSessao");

        Console.WriteLine(produtoId);
        var produto = await servicoProduto.ObterPorId(produtoId);
        if (produto == null)
            return NotFound();
        
        Produto = produto;
        
        var cliente = await contextoDados.Clientes
            .Include(u => u.Carrinho)
            .ThenInclude(c => c.Produtos)
            .FirstOrDefaultAsync(u => u.Email == usuarioEmail);

        if (cliente == null)
        {
            TempData["ProblemaAdicionarCarrinho"] = "Você não é um cliente para adicionar ao carrinho.";
            return Page();
        }

        if (Produto == null)
            return Page();
        
        cliente.Carrinho.Produtos.Add(Produto);
        await contextoDados.SaveChangesAsync();

        TempData["StatusCarrinho"] = "Adicionado ao carrinho!";
        
        return Page();
    }
}