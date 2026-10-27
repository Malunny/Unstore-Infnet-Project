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
    public async Task OnGetAsync([FromServices] AppDbContext appDbContext, [FromQuery] int page = 0)
    {
        Page = page;
        Produtos = await appDbContext.Produtos.Skip(page).Take(PageSize).ToListAsync();
        Console.WriteLine(Produtos.Count);
    }
}