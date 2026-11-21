namespace UnstoreProjectADS.Core.Services.Compartilhado;

public class RespostaServicoComDados<T> : RespostaServico
{
    public T? Dados { get; set; }

    public RespostaServicoComDados()
    {
        
    }
    
    public RespostaServicoComDados(T? dados)
    {
        Dados = dados;
    }
}