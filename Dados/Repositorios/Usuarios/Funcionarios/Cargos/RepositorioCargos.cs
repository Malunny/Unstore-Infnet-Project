using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Dados.Contratos.Repositorios;

namespace UnstoreProjectADS.Dados.Repositorios.Usuarios.Funcionarios.Cargos;

public class RepositorioCargos : IRepositorio<Cargo>
{
    private readonly ContextoDados _contextoBancoDados;
    public RepositorioCargos(ContextoDados contextoBancoDados)
    {
        _contextoBancoDados = contextoBancoDados;
    }
    
    public async Task<Cargo?> ObterPorIdAsync(int id)
    {
        return await _contextoBancoDados.Cargos.FirstOrDefaultAsync(cargo => cargo.Id == id);
    }

    public async Task<ICollection<Cargo>> ObterTodosAsync()
    {
        return await _contextoBancoDados.Cargos.ToListAsync();
    }

    public async Task<bool> SalvarAsync(Cargo entidade)
    {
        if (entidade.Id == 0)
        {
            await _contextoBancoDados.Cargos.AddAsync(entidade);
            await _contextoBancoDados.SaveChangesAsync();
            return true;
        }
        
        _contextoBancoDados.Attach(entidade).State = EntityState.Modified;
        await _contextoBancoDados.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeletarPorIdAsync(int id)
    {
        var cargo = await _contextoBancoDados.Cargos.FirstOrDefaultAsync(cargo => cargo.Id == id);

        if (cargo == null)
            return false;

        _contextoBancoDados.Remove(cargo);
        await _contextoBancoDados.SaveChangesAsync();
        return true;
    }
}