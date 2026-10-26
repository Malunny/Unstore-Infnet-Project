namespace UnstoreProjectADS.Models;

public class Endereco
{
    public int Id { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Municipio { get; set; } = string.Empty;
    public string Rua { get; set; } = string.Empty;
    public int Numero { get; set; }
    public string Complemento { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public ICollection<Encomenda> Encomendas { get; set; } = new List<Encomenda>();
}