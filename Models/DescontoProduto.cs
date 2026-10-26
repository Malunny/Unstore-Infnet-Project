namespace UnstoreProjectADS.Models;

public class DescontoProduto
{
    public int Id { get; set; }
    public bool EPorcentagem { get; set; }
    public double Porcentagem { get; set; }
    public decimal ValorConcreto { get; set; }

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}