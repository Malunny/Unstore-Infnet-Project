namespace UnstoreProjectADS.Pages.Requests;

public class RegisterAddressRequest
{
    public string Estado { get; set; } = string.Empty;
    public string Municipio { get; set; } = string.Empty;
    public string Rua { get; set; } = string.Empty;
    public int Numero { get; set; }
    public string Complemento { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
}