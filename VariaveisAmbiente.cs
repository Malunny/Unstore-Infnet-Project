namespace UnstoreProjectADS;

public static class VariaveisAmbiente
{
    public static VariaveisAdministrador Administrador = new VariaveisAdministrador();

    public class VariaveisAdministrador
    {
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}