using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.Services;

namespace UnstoreProjectADS.Pages;

public class PaginaListagemProdutos : PageModel
{
    public ICollection<Produto> Produtos { get; set; }

    [BindProperty(SupportsGet = true, Name = "pagina")] public int NumeroPagina { get; set; }
    
    public bool PossuiPaginaAnterior => NumeroPagina > 1;
    public bool PossuiProximaPagina { get; set; }
    public int TamanhoPagina { get; set; } = 20;
    public int QuantidadePaginas { get; set; }
    public async Task OnGetAsync([FromServices] ServicoProduto servicoProduto)
    {
        if (NumeroPagina < 1) 
            NumeroPagina = 1;
        
        Produtos =  await servicoProduto.ObterPagina(NumeroPagina, TamanhoPagina) ?? new List<Produto>();
        QuantidadePaginas = await servicoProduto.ObterQuantidadePaginas(TamanhoPagina);
        
        PossuiProximaPagina = await servicoProduto.PaginaPossuiRegistros(NumeroPagina + 1, TamanhoPagina);
    }
}