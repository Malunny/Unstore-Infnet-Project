namespace UnstoreProjectADS.Core.Services.Compartilhado;

public class RespostaServico
{
    public List<string> MensagensErro { get; set; } = new();
    public List<string> Mensagens { get; set; } = new();
    public bool Ok { get; set; }

    public void AdicionarErro(string mensagem)
    {
        MensagensErro.Add(mensagem);
        Ok = false;
    }

    public void AdicionarVariosErros(IEnumerable<string> mensagens)
    {
        foreach (var mensagem in mensagens)
        {
            AdicionarErro(mensagem);
        }
    }

    public void AdicionarMensagem(string mensagem)
    {
        MensagensErro.Add(mensagem);
    }
}