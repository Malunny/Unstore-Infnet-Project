using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;

namespace UnstoreProjectADS.Services;

public class ServicoProduto
{
    private readonly AppDbContext _contexto;
    private readonly int _tamanhoMinimoPagina = 5;
    private readonly int _tamanhoMaximoPagina = 25;
    public ServicoProduto(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ICollection<Produto>?> ObterPagina(int numeroPagina, int tamanhoPagina)
    {
        ValidarParametrosPagina(tamanhoPagina, numeroPagina);

        var registrosPulados = (numeroPagina - 1) * tamanhoPagina;

        var paginaProdutos = await _contexto.Produtos.Skip(registrosPulados).Take(tamanhoPagina).ToListAsync();
        
        return paginaProdutos;
    }

    public async Task<bool> PaginaPossuiRegistros(int numeroPagina, int tamanhoPagina)
    {
        ValidarParametrosPagina(tamanhoPagina, numeroPagina);
        
        var registrosPulados = (numeroPagina - 1) * tamanhoPagina;

        var possuiRegistros = await _contexto.Produtos.Skip(registrosPulados).Take(tamanhoPagina).AnyAsync();
        
        return possuiRegistros;
    }

    public async Task<bool> SalvarProduto(Produto produto)
    {
        if (produto.Id < 1)
            return false;
        
        _contexto.Produtos.Entry(produto);
        
        await _contexto.SaveChangesAsync();

        return true;
    }

    public async Task<Produto?> ObterPorId(int id)
    {
        if (id <= 0)
            return null;
        
        return await _contexto.Produtos.FirstOrDefaultAsync(p => p.Id == id);
    }
    
    #region Métodos Auxiliares
    private bool ValidarParametrosPagina(int tamanhoPagina, int numeroPagina)
    {
        if (tamanhoPagina < _tamanhoMinimoPagina)
            return false;

        if (tamanhoPagina > _tamanhoMaximoPagina)
            return false;

        if (numeroPagina <= 0)
            return false;
        
        return true;
    }
    #endregion
}