namespace UnstoreProjectADS.Models;

public class PedidoDeReposicao
{
    public int Id { get; set; }
    public ICollection<Produto> ProdutosApontados { get; set; }
    public Funcionario CriadorDoPedido { get; set; }
    public int CriadorDoPedidoId { get; set; }
}