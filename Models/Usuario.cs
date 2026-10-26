namespace UnstoreProjectADS.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;

    public ICollection<Conversa> Conversas { get; set; } = new List<Conversa>();
    public ICollection<Mensagem> MensagensEnviadas { get; set; } = new List<Mensagem>();
}