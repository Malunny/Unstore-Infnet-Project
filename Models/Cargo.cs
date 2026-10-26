namespace UnstoreProjectADS.Models;

public class Cargo
{
    public int Id { get; set; }
    public string Nome { get; set; }

    public ICollection<Funcionario> Funcionarios { get; set; }
}