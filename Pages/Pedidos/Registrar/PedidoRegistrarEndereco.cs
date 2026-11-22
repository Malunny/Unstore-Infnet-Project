using System.ComponentModel.DataAnnotations;

namespace UnstoreProjectADS.Pages.Pedidos.Registrar;

public class PedidoDeRegistroEndereco
{
    [Required(ErrorMessage = "O estado não pode estar vazio.")]
    [MinLength(1, ErrorMessage = "O estado não pode estar vazio.")]
    public string Estado { get; set; } = string.Empty;
    [Required(ErrorMessage = "O município não pode estar vazio.")]
    [MinLength(1, ErrorMessage = "O município não pode estar vazio.")]
    public string Municipio { get; set; } = string.Empty;
    [Required(ErrorMessage = "A rua não pode estar vazia.")]
    [MinLength(1, ErrorMessage = "A rua não pode estar vazia.")]
    public string Rua { get; set; } = string.Empty;
    [Required(ErrorMessage = "O número não pode estar vazio.")]
    public int Numero { get; set; } = 0;
    public string Complemento { get; set; } = string.Empty;
    [Required(ErrorMessage = "O CEP não pode estar vazio.")]
    [MinLength(1, ErrorMessage = "O CEP não pode estar vazia.")]
    public string CEP { get; set; } = string.Empty;
}