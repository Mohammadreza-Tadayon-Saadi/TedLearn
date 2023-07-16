using System.Text.RegularExpressions;
using Core.CustomResults;

namespace Core.Securities;

public static class RegularExpression
{
    public static PasswordValidationInfo GetPasswordValidationPatternAndMessage()
        => new PasswordValidationInfo(@"(?=.*\d)(?=.*[a-z])|(?=.*[A-Z])", "رمز عبور وارد شده حداقل باید شامل حرف انگلیسی و عدد باشد");

    public static string PhoneRegularExpression()
        => @"^09[0|1|2|3|4|9][0-9]{8}$";

    public static string EmailRegularExpression()
        => @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

    public static string UrlRegularExpression()
        => @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})";
}
