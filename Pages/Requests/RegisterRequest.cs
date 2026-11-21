namespace UnstoreProjectADS.Pages.Requests;

public class PedidoDeRegistro
{
    public string Email { get; set; }
    public string Nome { get; set; }
    public string Senha { get; set; }

    public List<PedidoDeRegistroEndereco> Enderecos { get; set; } = [ new PedidoDeRegistroEndereco() ];

    public class PedidoDeRegistroEndereco
    {
        public string Estado { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
        public int Numero { get; set; } = 0;
        public string Complemento { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
    }
}