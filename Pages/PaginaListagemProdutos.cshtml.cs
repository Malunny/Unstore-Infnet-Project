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
    [BindProperty(SupportsGet = true, Name = "tamanhoPagina")] public int TamanhoPagina { get; set; } = 10;
    [BindProperty(SupportsGet = true, Name = "pesquisa")] public string? TextoPesquisa { get; set; }
    public bool PossuiPaginaAnterior => NumeroPagina > 1;
    public bool PossuiProximaPagina { get; set; }
    
    public int QuantidadePaginas { get; set; }
    public async Task OnGetAsync([FromServices] ServicoProduto servicoProduto)
    {
        if (NumeroPagina < 1) 
            NumeroPagina = 1;

        if (TextoPesquisa != null)
        {
            Produtos = await servicoProduto.ObterPagina(NumeroPagina, TamanhoPagina, TextoPesquisa) ??
                       new List<Produto>();
            QuantidadePaginas = await servicoProduto.ObterQuantidadePaginas(TamanhoPagina, TextoPesquisa);
        }
        else
        {
            Produtos =  await servicoProduto.ObterPagina(NumeroPagina, TamanhoPagina) ?? new List<Produto>();
            QuantidadePaginas = await servicoProduto.ObterQuantidadePaginas(TamanhoPagina);
        }
        
        
        PossuiProximaPagina = NumeroPagina < QuantidadePaginas;
    }
}