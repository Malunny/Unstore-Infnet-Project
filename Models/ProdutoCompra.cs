namespace UnstoreProjectADS.Models;

public class ProdutoCompra
{
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; }

    public int CompraId { get; set; }
    public Compra Compra { get; set; }

    public decimal ValorNoMomento { get; set; }
    public int QuantidadeCompra { get; set; }
}