namespace UnstoreProjectADS.Models;

public class Cliente
{
    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
    public ICollection<AvaliacaoProduto> Avaliacoes { get; set; } = new List<AvaliacaoProduto>();
    public ICollection<Reclamacao> Reclamacoes { get; set; } = new List<Reclamacao>();
    public ICollection<Compra> Compras { get; set; } = new List<Compra>();
}