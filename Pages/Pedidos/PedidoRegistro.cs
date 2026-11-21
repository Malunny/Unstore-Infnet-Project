using System.ComponentModel.DataAnnotations;

namespace UnstoreProjectADS.Pages.Pedidos;

public class PedidoDeRegistro
{
    [EmailAddress(ErrorMessage = "O formato deve estar de acordo com o de e-mail.")]
    [Required(ErrorMessage = "O email deve estar preenchido.")]
    public string? Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "O nome deve estar preenchido.")]
    [MinLength(2, ErrorMessage = "O nome precisa ter ao menos 2 letras.")]
    public string? Nome { get; set; } = string.Empty;

    [MinLength(8, ErrorMessage = "A senha deve possuir, no mínimo, 8 caracteres.")]
    [Required(ErrorMessage = "A senha deve estar preenchida.")]
    public string? Senha { get; set; } = string.Empty;

    public List<PedidoDeRegistroEndereco> Enderecos { get; set; } = [ new PedidoDeRegistroEndereco() ];

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
}