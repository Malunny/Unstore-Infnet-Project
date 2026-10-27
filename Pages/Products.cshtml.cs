using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;

namespace UnstoreProjectADS.Pages;

public class ProductsPage : PageModel
{
    public ICollection<Produto> Produtos { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; } = 15;
    public async Task OnGetAsync([FromServices] AppDbContext appDbContext, [FromQuery] int pagina = 0)
    {
        Page = pagina;
        Console.WriteLine(pagina);
        Produtos = await appDbContext.Produtos.Skip(pagina * PageSize).Take(PageSize).ToListAsync();
        Console.WriteLine(Produtos.Count);
    }

    public async Task<IActionResult> OnPostAddPageAsync([FromServices] AppDbContext appDbContext)
    {
        var pagina = Page + 1;
        var produtos = await appDbContext.Produtos.Skip(pagina * PageSize).Take(PageSize).ToListAsync();
        if (produtos.Count == 0)
        {
            return Redirect("/Products?pagina=" + Page);
        }
        
        Produtos = produtos;
        Page += 1;
        return Redirect("/Products?pagina=" + Page);
    }
    
    public async Task<IActionResult> OnPostReturnPageAsync([FromServices] AppDbContext appDbContext)
    {
        var pagina = Page - 1;
        
        if (pagina < 0)
            return Redirect("/Products?pagina=" + Page);
        
        var produtos = await appDbContext.Produtos.Skip(pagina * PageSize).Take(PageSize).ToListAsync();
        
        if (produtos.Count == 0)
            return Redirect("/Products?pagina=" + Page);
        
        Produtos = produtos;
        Page -= 1;
        
        return Redirect("/Products?pagina=" + Page);
    }
}