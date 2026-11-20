using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Dados.Contratos.Repositorios;

namespace UnstoreProjectADS.Dados.Repositorios.Usuarios;

public class RepositorioUsuario : IRepositorio<Usuario>
{
    private readonly ContextoDados _contextoBancoDados;
    public RepositorioUsuario(ContextoDados contextoBancoDados)
    {
        _contextoBancoDados = contextoBancoDados;
    }

    #region Coleta

    public async Task<ICollection<Usuario>> ObterTodosAsync()
    {
        return await _contextoBancoDados.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> ObterPorIdAsync(int id)
    {
        return await _contextoBancoDados.Usuarios.FirstOrDefaultAsync(usuario => usuario.Id == id);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await _contextoBancoDados.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == email);
    }

    #endregion
    

    public async Task<bool> SalvarAsync(Usuario entidade)
    {
        if (entidade.Id == 0)
        {
            await _contextoBancoDados.Usuarios.AddAsync(entidade);
            await _contextoBancoDados.SaveChangesAsync();
            return true;
        }
        
        _contextoBancoDados.Attach(entidade).State = EntityState.Modified;
        await _contextoBancoDados.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeletarPorIdAsync(int id)
    {
        var usuario = await _contextoBancoDados.Usuarios.FirstOrDefaultAsync(usuario => usuario.Id == id);

        if (usuario == null)
            return false;
        
        _contextoBancoDados.Usuarios.Remove(usuario);
        await _contextoBancoDados.SaveChangesAsync();

        return true;
    }
}