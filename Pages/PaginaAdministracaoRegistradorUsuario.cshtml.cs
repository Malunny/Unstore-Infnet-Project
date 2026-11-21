using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Core.Services;
using UnstoreProjectADS.Core.Services.Compartilhado;
using UnstoreProjectADS.Dados.Repositorios.Usuarios.Funcionarios.Cargos;
using UnstoreProjectADS.Pages.Pedidos.Registrar;

namespace UnstoreProjectADS.Pages;

[Authorize(Policy = "AdministradorTotal")]
public class PaginaAdministracaoRegistradorUsuario : PageModel
{
    [BindProperty] public PedidoRegistroFuncionario Formulario { get; set; } = new();
    public RespostaServico RespostaServico { get; set; } = new RespostaServico();
    public List<Cargo> Cargos { get; set; } = new();
    
    private readonly RepositorioCargos _repositorioCargos;
    private readonly ServicoConta _servicoConta;

    public PaginaAdministracaoRegistradorUsuario(RepositorioCargos repositorioCargos,
        ServicoConta servicoConta)
    {
        _repositorioCargos = repositorioCargos;
        _servicoConta = servicoConta;
    }

    public async Task OnGetAsync()
    {
        await AlimentarCargos();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await AlimentarCargos();
            
        if (!ModelState.IsValid)
            return Page();
        
        RespostaServico = await _servicoConta.RegistrarFuncionarioAsync(Formulario);

        if (!RespostaServico.Ok)
            return Page();

        TempData["Salvo"] = "Funcionário salvo!";
        
        LimparDados();
        
        return Page();
    }

    private async Task AlimentarCargos()
    {
        var cargosColetados = _repositorioCargos.ObterTodosAsync().Result;
        Cargos = cargosColetados.ToList();
    }

    private void LimparDados()
    {
        Formulario = new();
    }
}