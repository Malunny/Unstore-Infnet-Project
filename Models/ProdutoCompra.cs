namespace UnstoreProjectADS.Models;

public class ProdutoCompra
{
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;

    public int CompraId { get; set; }
    public Compra Compra { get; set; } = null!;

    public decimal ValorNoMomento { get; set; }
    public int QuantidadeCompra { get; set; }
}