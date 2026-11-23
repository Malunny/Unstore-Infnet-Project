using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnstoreProjectADS.Pages;

[Authorize(Roles = "Cliente")]
public class PaginaCompraRealizada : PageModel
{
    public void OnGet()
    {
        
    }
}