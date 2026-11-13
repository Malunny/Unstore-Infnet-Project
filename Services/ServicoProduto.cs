using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;

namespace UnstoreProjectADS.Services;

public class ServicoProduto
{
    private readonly AppDbContext _contexto;
    private readonly int _tamanhoMinimoPagina = 5;
    public ServicoProduto(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ICollection<Produto>?> ObterPagina(int numeroPagina, int tamanhoPagina)
    {
        if (tamanhoPagina < _tamanhoMinimoPagina)
            return null;

        var registrosPulados = numeroPagina * tamanhoPagina;

        var paginaProdutos = await _contexto.Produtos.Skip(registrosPulados).Take(tamanhoPagina).ToListAsync();
        
        return paginaProdutos;
    }

    public async Task<Produto?> ObterPorId(int id)
    {
        if (id <= 0)
            return null;
        
        return await _contexto.Produtos.FirstOrDefaultAsync(p => p.Id == id);
    }
}