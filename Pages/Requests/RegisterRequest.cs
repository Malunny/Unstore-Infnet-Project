namespace UnstoreProjectADS.Pages.Requests;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Nome { get; set; }
    public string Senha { get; set; }

    public string Estado { get; set; } = string.Empty;
    public string Municipio { get; set; } = string.Empty;
    public string Rua { get; set; } = string.Empty;
    public int Numero { get; set; }
    public string Complemento { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
}