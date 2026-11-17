using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.Repositorios.Contratos;

namespace UnstoreProjectADS.Repositorios.Usuarios.Funcionarios;

public class RepositorioFuncionario : IRepositorio<Funcionario>
{
    private readonly AppDbContext _contextoBancoDados;
    public RepositorioFuncionario(AppDbContext contextoBancoDados)
    {
        _contextoBancoDados = contextoBancoDados;
    }
    public async Task<Funcionario?> ObterPorIdAsync(int id)
    {
        var funcionario = await _contextoBancoDados.Funcionarios.FirstOrDefaultAsync(funcionario => funcionario.Id == id);
        
        return funcionario;
    }

    public async Task<ICollection<Funcionario>> ObterTodosAsync()
    {
        return await _contextoBancoDados.Funcionarios.ToListAsync();
    }

    public async Task<bool> SalvarAsync(Funcionario entidade)
    {
        if (entidade.Id == 0)
        {
            await _contextoBancoDados.Funcionarios.AddAsync(entidade);
            await _contextoBancoDados.SaveChangesAsync();
            return true;
        }
        
        _contextoBancoDados.Attach(entidade).State = EntityState.Modified;
        await _contextoBancoDados.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeletarPorIdAsync(int id)
    {
        var funcionario = await _contextoBancoDados.Funcionarios.FirstOrDefaultAsync(funcionario => funcionario.Id == id);

        if (funcionario == null)
            return false;
        
        _contextoBancoDados.Funcionarios.Remove(funcionario);
        await _contextoBancoDados.SaveChangesAsync();

        return true;
    }
}