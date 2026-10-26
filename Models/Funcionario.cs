namespace UnstoreProjectADS.Models;

public class Funcionario : Usuario
{
    public ICollection<Conversa> ConversasParticipadas { get; set; } = new List<Conversa>();
    public Cargo Cargo { get; set; }
    public int CargoId { get; set; }
}