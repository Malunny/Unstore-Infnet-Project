namespace UnstoreProjectADS.Models;

public class Conversa
{
    public int Id { get; set; }

    public Cliente Cliente { get; set; }
    public int ClienteId { get; set; }
    
    public Funcionario Funcionario { get; set; }
    public int FuncionarioId { get; set; }
    
    public ICollection<Mensagem> Mensagens { get; set; } = new List<Mensagem>();
}