using UnstoreProjectADS.Models;

namespace UnstoreProjectADS.ViewModels;

public class ViewModelProduto
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public int QuantidadeEmEstoque { get; set; }

    public ViewModelProduto(Produto produto)
    {
        Nome = produto.Nome;
        Descricao = produto.Descricao;
        Valor = produto.Valor;
        QuantidadeEmEstoque = produto.QuantidadeEstoque;
    }
}