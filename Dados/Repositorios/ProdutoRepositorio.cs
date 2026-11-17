using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Dados.Contratos;

namespace UnstoreProjectADS.Dados.Repositorios;

public class ProdutoRepositorio : IRepositorio<Produto>
{
    private readonly ContextoDados _contextoDados;

    public ProdutoRepositorio(ContextoDados contextoDados)
    {
        _contextoDados = contextoDados;
    }
    
    public async Task Deletar(Produto entidade)
    {
        _contextoDados.Produtos.Remove(entidade);
        await _contextoDados.SaveChangesAsync();
    }

    public async Task Inserir(Produto entidade)
    {
        _contextoDados.Produtos.Add(entidade);
        await _contextoDados.SaveChangesAsync();
    }

    public async Task InserirVarias(IEnumerable<Produto> entidades)
    {
        _contextoDados.Produtos.AddRange(entidades);
        await _contextoDados.SaveChangesAsync();
    }

    public async Task<IEnumerable<Produto>> ObterTodos()
    {
        return await _contextoDados.Produtos.ToListAsync();
    }

    public async Task Salvar(Produto entidade)
    {
        _contextoDados.Attach(entidade).State = EntityState.Modified;
        await _contextoDados.SaveChangesAsync();
    }
}