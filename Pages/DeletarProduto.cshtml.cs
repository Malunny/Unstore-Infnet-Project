using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Core.Services;

namespace UnstoreProjectADS.Pages;

[Authorize(Policy = "AdministradoresProduto")]
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