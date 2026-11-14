using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.Services;

namespace UnstoreProjectADS.Pages;

public class PaginaListagemProdutos : PageModel
{
    public ICollection<Produto> Produtos { get; set; }
    public int NumeroPagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 15;
    public async Task OnGetAsync([FromServices] ServicoProduto servicoProduto, [FromQuery] int pagina = 0)
    {
        NumeroPagina = pagina;
        
        var produtos = await servicoProduto.ObterPagina(NumeroPagina, TamanhoPagina);

        if (produtos != null)
            Produtos = produtos;
    }

    public async Task<IActionResult> OnPostAddPageAsync([FromServices] ServicoProduto servicoProduto)
    {
        var pagina = NumeroPagina + 1;
        
        var produtos = await servicoProduto.ObterPagina(pagina, TamanhoPagina);
        
        if (produtos == null)
        {
            return Redirect("/Products?pagina=" + NumeroPagina);
        }
        
        Produtos = produtos;
        NumeroPagina += 1;
        
        return Redirect("/Products?pagina=" + NumeroPagina);
    }
    
    public async Task<IActionResult> OnPostReturnPageAsync([FromServices] AppDbContext appDbContext)
    {
        var pagina = NumeroPagina - 1;
        
        if (pagina < 0)
            return Redirect("/Products?pagina=" + NumeroPagina);
        
        var produtos = await appDbContext.Produtos.Skip(pagina * TamanhoPagina).Take(TamanhoPagina).ToListAsync();
        
        if (produtos.Count == 0)
            return Redirect("/Products?pagina=" + NumeroPagina);
        
        Produtos = produtos;
        NumeroPagina -= 1;
        
        return Redirect("/Products?pagina=" + NumeroPagina);
    }
}