namespace Core.CustomResults;

public class MessageCardInfo
{
    public MessageCardInfo(string title, string message, MessageCardStatus status)
    {
        Title = title;
        Message = message;
        Status = status;
    }

    public string Title { get; set; }
    public string Message { get; set; }
    public MessageCardStatus Status { get; set; }
}

public enum MessageCardStatus
{
    Ok = 0,
    Warning = 1,
    Error = 2,
}