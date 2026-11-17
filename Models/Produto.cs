using System.ComponentModel.DataAnnotations;

namespace UnstoreProjectADS.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int QuantidadeEstoque { get; set; }
    public string EstadoEstoque { get; set; }
    public ICollection<Imagem> Imagens { get; set; }

    public int? DescontoProdutoId { get; set; }
    public DescontoProduto? Desconto { get; set; }

    public ICollection<AvaliacaoProduto> Avaliacoes { get; set; } = new List<AvaliacaoProduto>();
    public ICollection<ProdutoCompra> ProdutosCompra { get; set; } = new List<ProdutoCompra>();
    public ICollection<PedidoDeReposicao> PedidosDeReposicao { get; set; } = new List<PedidoDeReposicao>();
}