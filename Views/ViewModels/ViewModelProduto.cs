using UnstoreProjectADS.Core.Entidades;

namespace UnstoreProjectADS.Views.ViewModels;

public class ViewModelProduto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public int QuantidadeEmEstoque { get; set; }
    public string? ImagemUrl { get; set; }
    public bool ContemImagem { get; set; }

    public ViewModelProduto(Produto produto)
    {
        Id = produto.Id;
        Nome = produto.Nome;
        Descricao = produto.Descricao;
        Valor = produto.Valor;
        QuantidadeEmEstoque = produto.QuantidadeEstoque;
        ImagemUrl = NormalizarUrlImagem(produto.Imagem?.Url);
        ContemImagem = produto.Imagem != null;
    }

    private static string? NormalizarUrlImagem(string? caminho)
    {
        if (string.IsNullOrWhiteSpace(caminho))
            return null;

        var url = caminho.Replace('\\', '/');
        const string pastaWebRoot = "wwwroot/";
        var indiceWebRoot = url.LastIndexOf($"/{pastaWebRoot}", StringComparison.OrdinalIgnoreCase);

        if (indiceWebRoot >= 0)
            url = url[(indiceWebRoot + pastaWebRoot.Length + 1)..];
        else if (url.StartsWith(pastaWebRoot, StringComparison.OrdinalIgnoreCase))
            url = url[pastaWebRoot.Length..];

        return $"~/{url.TrimStart('/')}";
    }
}