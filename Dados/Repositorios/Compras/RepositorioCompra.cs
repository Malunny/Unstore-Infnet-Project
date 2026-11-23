using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Dados.Contratos.Repositorios;

namespace UnstoreProjectADS.Dados.Repositorios.Compras;

public class RepositorioCompra : IRepositorio<Compra>
{
    private readonly ContextoDados _contextoBancoDados;

    public RepositorioCompra(ContextoDados contextoBancoDados)
    {
        _contextoBancoDados = contextoBancoDados;
    }
    
    public async Task<ICollection<Compra>> ObterTodosAsync()
    {
        return await _contextoBancoDados.Compras.ToListAsync();
    }

    public async Task<Compra?> ObterPorIdAsync(int id)
    {
        return await _contextoBancoDados.Compras.FirstOrDefaultAsync(compra => compra.Id == id);
    }

    public async Task<bool> SalvarAsync(Compra entidade)
    {
        if (entidade.Id == 0)
        {
            await _contextoBancoDados.Compras.AddAsync(entidade);
            await _contextoBancoDados.SaveChangesAsync();
            return true;
        }
        
        _contextoBancoDados.Attach(entidade).State = EntityState.Modified;
        await _contextoBancoDados.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeletarPorIdAsync(int id)
    {
        var compra = await _contextoBancoDados.Compras.FirstOrDefaultAsync(usuario => usuario.Id == id);

        if (compra == null)
            return false;
        
        _contextoBancoDados.Compras.Remove(compra);
        await _contextoBancoDados.SaveChangesAsync();

        return true;
    }

    public async Task<ICollection<Compra>> ObterPorClienteIdAsync(int clienteId)
    {
        return _contextoBancoDados.Compras.Where(compra => compra.ClienteId == clienteId).ToList();
    }
}