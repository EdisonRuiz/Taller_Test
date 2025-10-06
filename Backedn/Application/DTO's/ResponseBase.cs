namespace Application.DTO_s;

public class ResponseBase
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Result { get; set; }
}
