using Core.CustomResults;

namespace Core.Securities;

public static class RegularExpression
{
    public static PasswordValidationInfo GetPasswordValidationPatternAndMessage()
        => new PasswordValidationInfo(@"(?=.*\d)(?=.*[a-z])|(?=.*[A-Z])", "رمز عبور وارد شده حداقل باید شامل حرف انگلیسی و عدد باشد.");

    public static string PhoneRegularExpression()
        => @"^09[0|1|2|3|4|9][0-9]{8}$";

    public static string EmailRegularExpression()
        => @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

    public static string UrlRegularExpression()
        => @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})";
    
    public static string PersianDateRegularExpression(bool withTime = false)
    {
        if (withTime)
            return @"^(\d{4})/([0-9]|0[1-9]|1[0-2])/([0-9]|0[1-9]|[12][0-9]|3[01]) ([01][0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$";

        return @"^(\d{4})/([0-9]|0[1-9]|1[0-2])/([0-9]|0[1-9]|[12][0-9]|3[01])$";
    }
}
