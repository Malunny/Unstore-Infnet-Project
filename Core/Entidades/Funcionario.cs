namespace UnstoreProjectADS.Core.Entidades;

public class Funcionario : Usuario
{
    public Cargo Cargo { get; set; }
    public int CargoId { get; set; }
    public ICollection<PedidoDeReposicao> PedidosDeReposicao { get; set; }
}