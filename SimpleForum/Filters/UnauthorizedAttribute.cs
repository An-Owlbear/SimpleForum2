using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SimpleForum.Filters;

/// <summary>
/// Redirects unauthenticated users to index page, only allowing unauthenticated users to access the page
/// </summary>
public class UnauthorizedAttribute : Attribute, IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (context.HttpContext.User.Identity!.IsAuthenticated)
            context.Result = new RedirectToPageResult("/Index");
    }

    public void OnResourceExecuted(ResourceExecutedContext context) { }
}