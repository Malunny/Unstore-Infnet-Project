namespace UnstoreProjectADS.Models;

public class Carrinho
{
    public int Id { get; set; }
    public ICollection<Produto> Produtos { get; set; }
    public Cliente Cliente { get; set; }
}