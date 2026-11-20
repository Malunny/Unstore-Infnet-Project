using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnstoreProjectADS.Pages;

[Authorize]
public class PaginaSairDeSessao : PageModel
{
    public IActionResult OnGet()
    {
        HttpContext.SignOutAsync();

        return RedirectToPage("/Index");
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync();

        return RedirectToPage("/Index");
    }
}