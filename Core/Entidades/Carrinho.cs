namespace UnstoreProjectADS.Core.Entidades;

public class Carrinho
{
    public int Id { get; set; }
    public ICollection<ProdutoCarrinho> ProdutosCarrinho { get; set; }
    public Cliente Cliente { get; set; }

    public ICollection<Produto> PegarProdutos()
    {
        var produtos = new List<Produto>();
        foreach (var produtoCarrinho in ProdutosCarrinho)
        {
            produtos.Add(produtoCarrinho.Produto);
        }
        return produtos;
    }
}