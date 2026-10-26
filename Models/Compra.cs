namespace UnstoreProjectADS.Models;

public class Compra
{
    public int Id { get; set; }
    public DateTime RealizadaEm { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public Encomenda? Encomenda { get; set; }
    public ICollection<ProdutoCompra> ProdutosCompra { get; set; } = new List<ProdutoCompra>();
    public ICollection<AvaliacaoProduto> AvaliacoesReferenciadas { get; set; } = new List<AvaliacaoProduto>();
}