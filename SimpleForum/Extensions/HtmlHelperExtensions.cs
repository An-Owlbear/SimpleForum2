using Microsoft.AspNetCore.Mvc.Rendering;

namespace SimpleForum.Extensions;

public static class HtmlHelperExtensions
{
    /// <summary>
    /// Returns the path to a profile picture for the given filename
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="filename">The filename of the profile picture, not including the extension</param>
    /// <returns>The path to the profile picture, not including the domain name</returns>
    public static string ProfileImgUrl(this IHtmlHelper htmlHelper, string filename) =>
        $"/images/profile_images/{filename}.jpeg";
}