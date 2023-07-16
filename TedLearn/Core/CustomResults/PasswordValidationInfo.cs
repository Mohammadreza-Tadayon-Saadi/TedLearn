namespace Core.CustomResults;

public class PasswordValidationInfo
{
    public string Pattern { get; set; }
    public string ErrorMessage { get; set; }

    public PasswordValidationInfo(string pattern, string errorMessage)
    {
        Pattern = pattern;
        ErrorMessage = errorMessage;
    }
}
