namespace UnstoreProjectADS.Core.Services.Compra.Respostas;

public class RespostaCompraDetalhada
{
    public DateTime RealizadaEm { get; set; }
    public string ProdutosComprados { get; set; }
    public string EstadoEncomenda { get; set; }
    public decimal ValorTotal { get; set; }

    public RespostaCompraDetalhada()
    {
        
    }

    public RespostaCompraDetalhada(Entidades.Compra compra)
    {
        ProdutosComprados = string.Join(", ", compra.ProdutosCompra.Select(produtoCompra => produtoCompra.Produto.Nome));
        RealizadaEm = compra.RealizadaEm;
        EstadoEncomenda = compra?.Encomenda?.EstadoEntrega ?? "Desconhecido";
        ValorTotal = compra.CalcularValorTotal();
    }
}