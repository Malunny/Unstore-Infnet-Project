using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;

namespace UnstoreProjectADS.Pages;

[Authorize(Roles = $"Vendas, AdministradorEstoque, AdministradorSistema, Financeiro")]
public class ProductManagerPage : PageModel
{
    [BindProperty(SupportsGet = true)] public bool Creating { get; set; } = false;

    [BindProperty] public Produto Produto { get; set; }
    public int Id { get; set; }
    public async Task OnGetAsync(int id, [FromServices] AppDbContext appDbContext)
    {
        Id = id;
        if (id == 0) 
        {
            Creating = true;
            Produto = new Produto();
            return;
        }
        
        var produto = await appDbContext.Produtos.FirstOrDefaultAsync(p => p.Id == id);

        if (produto == null)
        {
            Creating = true;
            Produto = new Produto();
            return;
        }
        
        Produto = produto;
    }

    public async Task<IActionResult> OnPostAsync([FromServices] AppDbContext appDbContext)
    {
        if (Produto.Id == 0)
        {
            await appDbContext.Produtos.AddAsync(Produto);
            ;
        }

        await appDbContext.SaveChangesAsync();
        
        return RedirectToPage("/Products");
    }
}