using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Dados.Contratos.Repositorios;

namespace UnstoreProjectADS.Dados.Repositorios.Produtos;

public class RepositorioProdutos : IRepositorio<Produto>
{
    private readonly ContextoDados _contextoBancoDados;

    public RepositorioProdutos(ContextoDados contextoBancoDados)
    {
        _contextoBancoDados = contextoBancoDados;
    }
    
    public async Task<ICollection<Produto>> ObterTodosAsync()
    {
        return await _contextoBancoDados.Produtos.ToListAsync();
    }

    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        var produto = await _contextoBancoDados.Produtos.FirstOrDefaultAsync(p => p.Id == id);
        
        if (produto == null)
            return null;
        
        return await AlimentarImagem(produto);
    }
    
    public async Task<ICollection<Produto>?> ObterPagina(int numeroPagina, int tamanhoPagina, string textoPesquisa)
    {
        var registrosPulados = (numeroPagina - 1) * tamanhoPagina;

        var paginaProdutos = await _contextoBancoDados.Produtos
            .Where(produto => EF.Functions.Like(produto.Nome, $"%{textoPesquisa}%"))
            .Skip(registrosPulados).Take(tamanhoPagina)
            .ToListAsync();

        var paginaProdutosImagensAlimentads = new List<Produto>();
        
        foreach (var produto in paginaProdutos)
        {
            paginaProdutosImagensAlimentads.Add(await AlimentarImagem(produto));
        }
        
        return paginaProdutosImagensAlimentads;
    }
    
    public async Task<ICollection<Produto>?> ObterPagina(int numeroPagina, int tamanhoPagina)
    {
        var registrosPulados = (numeroPagina - 1) * tamanhoPagina;

        var paginaProdutos = await _contextoBancoDados.Produtos.Skip(registrosPulados).Take(tamanhoPagina).ToListAsync();
        
        var paginaProdutosImagensAlimentads = new List<Produto>();
        
        foreach (var produto in paginaProdutos)
        {
            paginaProdutosImagensAlimentads.Add(await AlimentarImagem(produto));
        }
        
        return paginaProdutosImagensAlimentads;
    }

    public async Task<bool> SalvarAsync(Produto entidade)
    {
        if (entidade.Id == 0)
        {
            await _contextoBancoDados.Produtos.AddAsync(entidade);
            await _contextoBancoDados.SaveChangesAsync();
            return true;
        }
        
        _contextoBancoDados.Attach(entidade).State = EntityState.Modified;
        await _contextoBancoDados.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeletarPorIdAsync(int id)
    {
        var produto = await _contextoBancoDados.Produtos.FirstOrDefaultAsync(usuario => usuario.Id == id);

        if (produto == null)
            return false;
        
        _contextoBancoDados.Produtos.Remove(produto);
        await _contextoBancoDados.SaveChangesAsync();

        return true;
    }

    #region Métodos de alimentação

    private async Task<Produto> AlimentarImagem(Produto entidade)
    {
        var imagem = await _contextoBancoDados.Imagens.FirstOrDefaultAsync(i => i.Id == entidade.ImagemId);

        if (imagem == null)
            return entidade;
        
        entidade.Imagem = imagem;
        return entidade;
    }

    #endregion
}