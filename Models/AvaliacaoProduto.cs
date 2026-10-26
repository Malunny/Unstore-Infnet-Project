namespace UnstoreProjectADS.Models;

public class AvaliacaoProduto
{
    public int Id { get; set; }
    public short Estrelas { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;

    public int? CompraId { get; set; }
    public Compra? Compra { get; set; }
}