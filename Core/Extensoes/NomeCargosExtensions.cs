using UnstoreProjectADS.Core.Enums;

namespace UnstoreProjectADS.Core.Extensoes;

public static class NomeCargosExtensions
{
    public static string PegarNome(this Cargos cargo)
    {
        switch (cargo)
        {
            case Cargos.AdministradorSistema:
                return "AdministradorSistema";
                break;
            case Cargos.AdministradorEstoque:
                return "AdministradorEstoque";
                break;
            case Cargos.Repositor:
                return "Repositor";
                break;
            case Cargos.Financeiro:
                return "Financeiro";
                break;
            case Cargos.SAC:
                return "SAC";
            case Cargos.Vendas:
                return "Vendas";
                break;
            case Cargos.Logistica:
                return "Logistica";
                break;
            case Cargos.Cliente:
                return "Cliente";
                break;
            default:
                throw new NotImplementedException("Enum de Cargos não implementado.");
        }
    }
}