namespace UnstoreProjectADS.Extensoes.Monetario;

public static class ExtensoesNumeroParaDinheiro
{
    public static string TransformarEmMonetario(this decimal numero) => $"R$: {numero.ToString("F2")}"; 
}