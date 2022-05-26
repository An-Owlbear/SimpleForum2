using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SimpleForum.Filters;

public class UnauthorizedAttribute : Attribute, IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (context.HttpContext.User.Identity!.IsAuthenticated)
            context.Result = new RedirectToPageResult("/Index");
    }

    public void OnResourceExecuted(ResourceExecutedContext context) { }
}