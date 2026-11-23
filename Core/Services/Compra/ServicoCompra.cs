using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Services.Compartilhado;
using UnstoreProjectADS.Core.Services.Compra.Respostas;
using UnstoreProjectADS.Dados;

namespace UnstoreProjectADS.Core.Services.Compra;

public class ServicoCompra
{
    private readonly ContextoDados _contexto;
    public ServicoCompra(ContextoDados contexto)
    {
        _contexto = contexto;
    }

    public async Task<RespostaServicoComDados<RespostaCompraDetalhada>> PegarDadosDetalhadosCompra(int compraId)
    {
        var resposta = new RespostaServicoComDados<RespostaCompraDetalhada>();

        var compra = await _contexto.Compras
            .AsNoTracking()
            .Include(compra => compra.Encomenda)
            .Include(compra => compra.ProdutosCompra)
                .ThenInclude(produtoCOmpra => produtoCOmpra.Produto)
            .FirstOrDefaultAsync(compra => compra.Id == compraId);

        if (compra == null)
        {
            resposta.AdicionarErro($"Compra de id {compraId} não foi encontrada.");
            return resposta;
        }

        resposta.Dados = new RespostaCompraDetalhada(compra);
        
        return resposta;;
    }

    public async Task<ICollection<RespostaCompraDetalhada>> PegarVariosDadosDetalhadasCompra(IEnumerable<int> compraIds)
    {
        var respostas = new List<RespostaCompraDetalhada>();
        foreach (var compraId in compraIds)
        {
            var dados = await PegarDadosDetalhadosCompra(compraId);
            if (!dados.Ok || dados.Dados == null)
                continue;
            respostas.Add(dados.Dados);
        }
        return respostas;
    }
}