using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Core.Services.Imagem;
using UnstoreProjectADS.Dados;
using UnstoreProjectADS.Dados.Repositorios.Produtos;

namespace UnstoreProjectADS.Core.Services;

public class ServicoProduto
{
    private readonly ContextoDados _contexto;
    private readonly ServicoImagem _servicoImagem;
    private readonly RepositorioProdutos _repositorioProdutos;
    
    private readonly int _tamanhoMinimoPagina = 5;
    private readonly int _tamanhoMaximoPagina = 25;
    public ServicoProduto(ContextoDados contexto, ServicoImagem servicoImagem, RepositorioProdutos repositorioProdutos)
    {
        _contexto = contexto;
        _servicoImagem = servicoImagem;
        _repositorioProdutos = repositorioProdutos;
    }
    
    public async Task<bool> PaginaPossuiRegistros(int numeroPagina, int tamanhoPagina)
    {
        ValidarParametrosPagina(tamanhoPagina, numeroPagina);
        
        var registrosPulados = (numeroPagina - 1) * tamanhoPagina;

        var possuiRegistros = await _contexto.Produtos.Skip(registrosPulados).Take(tamanhoPagina).AnyAsync();
        
        return possuiRegistros;
    }
    
    public async Task<bool> SalvarProdutoComImagem(Produto produto, IFormFile arquivoImagem)
    {
        var respostaImagem = await _servicoImagem.SalvarImagem(arquivoImagem);

        if (!respostaImagem.Ok)
            return false;
        
        produto.Imagem = respostaImagem.Dados;
        produto.ImagemId = respostaImagem.Dados?.Id;
        
        if (produto.Id < 1)
        {
            _contexto.Produtos.Add(produto);
        
            await _contexto.SaveChangesAsync();

            return true;
        }
        
        _contexto.Attach(produto).State = EntityState.Modified;
        
        await _contexto.SaveChangesAsync();

        return true;
    }

    public async Task<int> ObterQuantidadePaginas(int tamanhoPagina)
    {
        var quantidadeRegistros = await _contexto.Produtos.CountAsync();

        var quantidadePaginas = (double)quantidadeRegistros / tamanhoPagina; 
        
        return (int)double.Ceiling(quantidadePaginas);
    }
    
    public async Task<int> ObterQuantidadePaginas(int tamanhoPagina, string textoPesquisa)
    {
        var quantidadeRegistros = await _contexto.Produtos
            .Where(produto => EF.Functions.Like(produto.Nome, $"%{textoPesquisa}%"))
                .CountAsync();

        var quantidadePaginas = (double)quantidadeRegistros / tamanhoPagina; 
        
        return (int)double.Ceiling(quantidadePaginas);
    }

    public async Task<Produto?> ObterPorId(int id)
    {
        if (id <= 0)
            return null;

        return await _repositorioProdutos.ObterPorIdAsync(id);
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