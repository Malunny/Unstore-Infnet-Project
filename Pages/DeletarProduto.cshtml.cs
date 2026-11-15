using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Services;

namespace UnstoreProjectADS.Pages;

public class DeletarProduto : PageModel
{
    [BindProperty(Name = "produtoId")] public int ProdutoId { get; set; }
    public async Task<IActionResult> OnPostAsync([FromServices] ServicoProduto servicoProduto)
    {
        var foiDeletado = await servicoProduto.DeletarProduto(ProdutoId);
        
        if (!foiDeletado)
            return NotFound();

        var paginaAnterior = Request.Headers.Referer.ToString();
        
        if (string.IsNullOrEmpty(paginaAnterior))
            return RedirectToPage("/Index");
        
        return Redirect(paginaAnterior);
    }
}