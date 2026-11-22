using System.ComponentModel.DataAnnotations;
using UnstoreProjectADS.Core.Entidades;

namespace UnstoreProjectADS.Pages.Pedidos.Registrar;

public class PedidoDeRegistroCliente : PedidoRegistro
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

    public List<PedidoDeRegistroEndereco> Enderecos { get; set; } = new List<PedidoDeRegistroEndereco>
    {
        new PedidoDeRegistroEndereco()
    };
}