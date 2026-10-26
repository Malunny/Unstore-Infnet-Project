namespace UnstoreProjectADS.Models;

public class Conversa
{
    public int Id { get; set; }

    public ICollection<Usuario> Participantes { get; set; } = new List<Usuario>();
    public ICollection<Mensagem> Mensagens { get; set; } = new List<Mensagem>();
}