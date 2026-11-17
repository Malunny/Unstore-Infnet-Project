using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnstoreProjectADS.Core.Entidades;

namespace UnstoreProjectADS.Pages;

[Authorize(Policy = "AdministradorTotal")]
public class PaginaAdministracaoRegistradorUsuario : PageModel
{
    public Funcionario Funcionario { get; set; }
    public Dictionary<string, int> NomeCargoId { get; set; }

    public async Task OnGetAsync()
    {
        
    }
}