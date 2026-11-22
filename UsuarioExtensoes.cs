using System.Security.Claims;
using UnstoreProjectADS.Core.Enums;
using UnstoreProjectADS.Core.Extensoes;
using UnstoreProjectADS.Dados;

namespace UnstoreProjectADS;

public static class UsuarioExtensoes
{
    public static string? ColetarEmail(this ClaimsPrincipal usuario) =>
        usuario.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
    
    public static string? ColetarNome(this ClaimsPrincipal usuario) =>
        usuario.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    
    public static bool VerificarUsuarioCliente(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.Cliente.PegarNome());
    
    public static bool VerificarCargoFuncionario(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.Vendas.PegarNome()) ||
        usuario.IsInRole(Cargos.AdministradorSistema.PegarNome()) ||
        usuario.IsInRole(Cargos.AdministradorEstoque.PegarNome()) ||
        usuario.IsInRole(Cargos.Repositor.PegarNome()) ||
        usuario.IsInRole(Cargos.Financeiro.PegarNome()) ||
        usuario.IsInRole(Cargos.SAC.PegarNome()) ||
        usuario.IsInRole(Cargos.Repositor.PegarNome());

    public static bool VerificarCargoVendas(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.Vendas.PegarNome());

    public static bool VerificarCargoAdministradorSistema(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.AdministradorSistema.PegarNome());

    public static bool VerificarCargoAdministradorEstoque(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.AdministradorEstoque.PegarNome());

    public static bool VerificarCargoRepositor(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.Repositor.PegarNome());

    public static bool VerificarCargoFinanceiro(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.Financeiro.PegarNome());

    public static bool VerificarCargoSAC(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(Cargos.SAC.PegarNome());
}