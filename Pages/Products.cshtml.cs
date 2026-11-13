using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.Services;

namespace UnstoreProjectADS.Pages;

public class ProductsPage : PageModel
{
    public ICollection<Produto> Produtos { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 15;
    public async Task OnGetAsync([FromServices] ServicoProduto servicoProduto, [FromQuery] int pagina = 0)
    {
        Page = pagina;
        
        var produtos = await servicoProduto.ObterPagina(Page, PageSize);

        if (produtos != null)
            Produtos = produtos;
    }

    public async Task<IActionResult> OnPostAddPageAsync([FromServices] ServicoProduto servicoProduto)
    {
        var pagina = Page + 1;
        
        var produtos = await servicoProduto.ObterPagina(pagina, PageSize);
        
        if (produtos == null)
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