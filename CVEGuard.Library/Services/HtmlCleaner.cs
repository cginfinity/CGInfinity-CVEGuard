using System;
using System.Text.RegularExpressions;

public static class HtmlCleaner
{
    public static string RemoveBeforeHtmlTag(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        int index = input.IndexOf("<html", StringComparison.OrdinalIgnoreCase);
        return index >= 0 ? input.Substring(index) : input;
    }

    public static string RemoveAfterHtmlTag(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        int index = input.LastIndexOf("</html>", StringComparison.OrdinalIgnoreCase);
        return index >= 0 ? input.Substring(0, index + 7) : input;
    }
}
