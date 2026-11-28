using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Core.Services;
using UnstoreProjectADS.Dados.Repositorios.Produtos;

namespace UnstoreProjectADS.Pages;

[Authorize(Policy = "AdministradoresProduto")]
public class DeletarProduto(RepositorioProdutos repositorioProdutos) : PageModel
{
    [BindProperty(Name = "produtoId")] public int ProdutoId { get; set; }
    public async Task<IActionResult> OnPostAsync([FromServices] ServicoProduto servicoProduto)
    {
        var foiDeletado = await repositorioProdutos.DeletarPorIdAsync(ProdutoId);
        
        if (!foiDeletado)
            return NotFound();

        var paginaAnterior = Request.Headers.Referer.ToString();
        
        if (string.IsNullOrEmpty(paginaAnterior))
            return RedirectToPage("/Index");
        
        return Redirect(paginaAnterior);
    }
}