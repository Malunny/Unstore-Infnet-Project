using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.Services;

namespace UnstoreProjectADS.Pages;

public class ProdutoPage : PageModel
{
    public Produto? Produto { get; set; }
    public async Task OnGetAsync([FromRoute] int produtoId, [FromServices] ServicoProduto servicoProduto)
    {
        var produto = await servicoProduto.ObterPorId(produtoId);
        
        if (produto == null)
            return;
        
        Produto = produto;
    }
    
    public async Task<IActionResult> OnPostAdicionarCarrinhoAsync([FromRoute] int produtoId,
        [FromServices] AppDbContext appDbContext,
        [FromServices] ServicoProduto servicoProduto)
    {
        var usuarioEmail = User.FindFirstValue(ClaimTypes.Email);
        if (usuarioEmail == null)
            return Redirect("/Login");

        Console.WriteLine(produtoId);
        var produto = await servicoProduto.ObterPorId(produtoId);
        if (produto == null)
            return NotFound();
        
        Produto = produto;
        
        var cliente = await appDbContext.Clientes
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
        
        await appDbContext.SaveChangesAsync();
        return Redirect("/");
    }
}