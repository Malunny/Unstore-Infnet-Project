using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnstoreProjectADS.Pages;

[Authorize]
public class PaginaSairDeSessao : PageModel
{
    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync();

        return RedirectToPage("/Index");
    }
}