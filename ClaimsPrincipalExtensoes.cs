using System.Security.Claims;

namespace UnstoreProjectADS;

public static class ClaimsPrincipalExtensoes
{
    public static bool PossuiCargoFuncionario(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.Vendas.PegarNome()) ||
        usuario.IsInRole(Cargos.AdministradorSistema.PegarNome()) ||
        usuario.IsInRole(Cargos.AdministradorEstoque.PegarNome()) ||
        usuario.IsInRole(Cargos.Repositor.PegarNome()) ||
        usuario.IsInRole(Cargos.Financeiro.PegarNome()) ||
        usuario.IsInRole(Cargos.SAC.PegarNome()) ||
        usuario.IsInRole(Cargos.Repositor.PegarNome());

    public static bool PossuiCargoVendas(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.Vendas.PegarNome());

    public static bool PossuiCargoAdministradorSistema(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.AdministradorSistema.PegarNome());

    public static bool PossuiCargoAdministradorEstoque(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.AdministradorEstoque.PegarNome());

    public static bool PossuiCargoRepositor(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.Repositor.PegarNome());

    public static bool PossuiCargoFinanceiro(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.Financeiro.PegarNome());

    public static bool PossuiCargoSAC(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.SAC.PegarNome());
}