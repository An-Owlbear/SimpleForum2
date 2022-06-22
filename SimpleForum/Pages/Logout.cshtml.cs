using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleForum.Pages;

[Authorize]
public class Logout : PageModel
{
    public async Task<IActionResult> OnPost()
    {
        await HttpContext.SignOutAsync();
        return RedirectToPage("/Index");
    }
}