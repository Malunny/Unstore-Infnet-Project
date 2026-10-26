namespace UnstoreProjectADS.Models;

public class Reclamacao
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public int MotivoReclamacaoId { get; set; }
    public MotivoReclamacao MotivoReclamacao { get; set; } = null!;
}