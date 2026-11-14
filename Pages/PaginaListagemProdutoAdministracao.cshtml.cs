using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.Services;

namespace UnstoreProjectADS.Pages;

[Authorize(Policy = "AdministradoresProduto")]
public class PaginaListagemProdutoAdministracao : PageModel
{
    public ICollection<Produto> Produtos { get; set; }

    [BindProperty(SupportsGet = true, Name = "pagina")]
    public int NumeroPagina { get; set; }
    
    public bool PossuiPaginaAnterior => NumeroPagina > 1;
    public bool PossuiProximaPagina { get; set; }
    public int TamanhoPagina { get; set; } = 6;
    public async Task OnGetAsync([FromServices] ServicoProduto servicoProduto)
    {
        if (NumeroPagina < 1) 
            NumeroPagina = 1;
        
        var produtos = await servicoProduto.ObterPagina(NumeroPagina, TamanhoPagina);
        
        Produtos = produtos ?? new List<Produto>();
        
        PossuiProximaPagina = await servicoProduto.PaginaPossuiRegistros(NumeroPagina + 1, TamanhoPagina);
    }
}