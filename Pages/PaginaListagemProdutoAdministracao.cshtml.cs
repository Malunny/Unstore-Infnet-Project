using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Core.Services;
using UnstoreProjectADS.Dados.Repositorios.Produtos;

namespace UnstoreProjectADS.Pages;

[Authorize(Policy = "AdministradoresProduto")]
public class PaginaListagemProdutoAdministracao(
    RepositorioProdutos repositorioProdutos,
    ServicoProduto servicoProduto) : PageModel
{
    public ICollection<Produto> Produtos { get; set; }

    [BindProperty(SupportsGet = true, Name = "pagina")] public int NumeroPagina { get; set; }
    [BindProperty(SupportsGet = true, Name = "textoPesquisa")] public string? TextoPequisa { get; set; }
    [BindProperty(SupportsGet = true, Name = "tamanhoPagina")] public int TamanhoPagina { get; set; } = 20;
    public int QuantidadePaginas { get; set; }
    
    public bool PossuiPaginaAnterior => NumeroPagina > 1;
    public bool PossuiProximaPagina { get; set; }
    
    public async Task OnGetAsync()
    {
        if (NumeroPagina < 1) 
            NumeroPagina = 1;

        ICollection<Produto>? produtos;

        if (TextoPequisa != null)
        {
            produtos = await repositorioProdutos.ObterPagina(NumeroPagina, TamanhoPagina, TextoPequisa);
            QuantidadePaginas = await servicoProduto.ObterQuantidadePaginas(TamanhoPagina, TextoPequisa);    
        }
        else
        {
            produtos = await repositorioProdutos.ObterPagina(NumeroPagina, TamanhoPagina);
            QuantidadePaginas = await servicoProduto.ObterQuantidadePaginas(TamanhoPagina);    
        }
        
        produtos ??= new List<Produto>();
        
        Produtos = produtos;
        
        PossuiProximaPagina = await servicoProduto.PaginaPossuiRegistros(NumeroPagina + 1, TamanhoPagina);
    }
}