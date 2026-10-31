using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;

namespace UnstoreProjectADS.Pages;

public class ProdutoPage : PageModel
{
    public Produto? Produto { get; set; }
    public async Task OnGetAsync([FromRoute] int id, [FromServices] AppDbContext appDbContext)
    {
        var produto = await appDbContext.Produtos.FirstOrDefaultAsync(prod => prod.Id == id);
        if (produto == null)
            return;
        
        Produto = produto;
    }

    public async Task<IActionResult> OnPostAdicionarCarrinhoAsync([FromServices] AppDbContext appDbContext)
    {
        var usuarioEmail = User.FindFirstValue(ClaimTypes.Email);
        Console.WriteLine("\n \n \n OI \n \n \n");
        if (usuarioEmail == null)
            return Redirect("/Login");
        Console.WriteLine("\n \n \n OI \n \n \n");
        var cliente = await appDbContext.Clientes
            .Include(u => u.Carrinho)
            .FirstOrDefaultAsync(u => u.Email == usuarioEmail);
        
        if (cliente == null)
            return Redirect("/Login");
        Console.WriteLine("\n \n \n OI \n \n \n");

        if (Produto == null)
            return Page();
        
        cliente.Carrinho.Produtos.Add(Produto);
        
        await appDbContext.SaveChangesAsync();
        return Redirect("/");
    }
}