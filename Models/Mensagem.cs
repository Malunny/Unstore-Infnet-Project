namespace UnstoreProjectADS.Models;

public class Mensagem
{
    public int Id { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public DateTime EnviadaEm { get; set; }

    public int RemetenteId { get; set; }
    public Usuario Remetente { get; set; } = null!;

    public int ConversaId { get; set; }
    public Conversa Conversa { get; set; } = null!;
}