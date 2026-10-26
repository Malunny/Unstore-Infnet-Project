namespace UnstoreProjectADS.Models;

public class Encomenda
{
    public int Id { get; set; }
    public DateTime PrevisaoEntrega { get; set; }
    public string EstadoEntrega { get; set; } = string.Empty;

    public int CompraId { get; set; }
    public Compra Compra { get; set; } = null!;

    public int FuncionarioId { get; set; }
    public Funcionario Funcionario { get; set; } = null!;

    public int EnderecoId { get; set; }
    public Endereco Endereco { get; set; } = null!;
}