using System.Text.RegularExpressions;

namespace Core.Securities;

public static class RegularExpression
{
    public static bool MatchPassword(string? password)
    {
        if (String.IsNullOrEmpty(password)) return false;

        var pattern = @"(?=.*\d)(?=.*[a-z])|(?=.*[A-Z])";
        var reg = new Regex(pattern);
        return reg.IsMatch(password.Trim());
    }

    public static bool isEmail(this string email)
    {
        if (String.IsNullOrEmpty(email)) return true;

        var pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        var reg = new Regex(pattern);
        return reg.IsMatch(email);
    }

    public static bool isUrl(this string url)
    {
        if (String.IsNullOrEmpty(url)) return true;

        var pattern = @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})";
        var reg = new Regex(pattern);
        return reg.IsMatch(url);
    }
}
