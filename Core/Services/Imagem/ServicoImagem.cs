using Microsoft.EntityFrameworkCore;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Core.Services.Compartilhado;
using UnstoreProjectADS.Dados;

namespace UnstoreProjectADS.Core.Services.Imagem;

public class ServicoImagem
{
    private readonly ContextoDados _contexto;
    private readonly IWebHostEnvironment _environment;
    public ServicoImagem(ContextoDados context, IWebHostEnvironment environment)
    {
        _contexto = context;
        _environment = environment;
    }

    public async Task<RespostaServicoComDados<Entidades.Imagem>> SalvarImagem(IFormFile arquivoImagem)
    {
        var resposta =  new RespostaServicoComDados<Entidades.Imagem>();
        
        var pastaImagensProdutos = Path.Combine(_environment.WebRootPath, "imagens", "produtos");
        if (!Directory.Exists(pastaImagensProdutos))
            Directory.CreateDirectory(pastaImagensProdutos);

        var nomeArquivo = Guid.NewGuid() + Path.GetExtension(arquivoImagem.FileName);
        var caminhoCompleto = Path.Combine(pastaImagensProdutos, nomeArquivo);

        await using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
        {
            await arquivoImagem.CopyToAsync(stream);
        }

        var imagem = new Entidades.Imagem();
        imagem.Url = $"imagens/produtos/{nomeArquivo}";
        
        await _contexto.Imagens.AddAsync(imagem);
        await _contexto.SaveChangesAsync();
        
        resposta.Dados = imagem;
        
        return resposta;
    }

    public async Task<Entidades.Imagem?> ColetarImagemDeProduto(Produto produto)
    {
        if (produto.ImagemId == null || produto.ImagemId == 0)
        {
            return null;
        }

        var imagem = await _contexto.Imagens.FirstOrDefaultAsync(i => i.Id == produto.ImagemId);
        
        return imagem;
    }
}