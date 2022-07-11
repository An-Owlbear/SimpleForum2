using Microsoft.AspNetCore.Mvc.Rendering;

namespace SimpleForum.Extensions;

public static class HtmlHelperExtensions
{
    public static string ProfileImgUrl(this IHtmlHelper htmlHelper, string filename) =>
        $"/images/profile_images/{filename}.jpeg";
}