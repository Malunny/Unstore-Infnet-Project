using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Core.Services;

namespace UnstoreProjectADS.Pages;

[Authorize(Policy = "AdministradoresProduto")]
public class PaginaAdministracaoProdutos : PageModel
{
    [BindProperty(SupportsGet = true)] public bool Creating { get; set; } = false;

    [BindProperty(SupportsGet = true, Name = "produtoId")] public int ProdutoId { get; set; } = 0;
    [BindProperty] public Produto Produto { get; set; }
    public async Task OnGetAsync([FromServices] ServicoProduto servicoProduto)
    {
        if (ProdutoId == 0) 
        {
            Creating = true;
            Produto = new Produto();
            return;
        }

        var produto = await servicoProduto.ObterPorId(ProdutoId);

        if (produto == null)
        {
            Creating = true;
            Produto = new Produto();
            return;
        }
        
        Produto = produto;
    }

    public async Task<IActionResult> OnPostAsync([FromServices] ServicoProduto servicoProduto)
    {
        if (Produto.Id == 0)
            Produto.Id = ProdutoId;
        
        var produtoFoiSalvo = await servicoProduto.SalvarProduto(Produto);

        if (!produtoFoiSalvo)
            TempData["ProdutoNaoSalvo"] = "O produto não conseguiu ser salvo.";
        
        if (produtoFoiSalvo)
            TempData["ProdutoSalvo"] = "O produto foi salvo com sucesso.";
        
        return Page();
    }
}