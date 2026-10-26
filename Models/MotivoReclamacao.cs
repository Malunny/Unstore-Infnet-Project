namespace UnstoreProjectADS.Models;

public class MotivoReclamacao
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;

    public ICollection<Reclamacao> Reclamacoes { get; set; } = new List<Reclamacao>();
}