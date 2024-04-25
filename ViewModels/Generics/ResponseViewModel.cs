namespace cia_aerea_api.ViewModels.Generics;

public class ResponseViewModel<T> 
{
    public ResponseViewModel(int status, string message, T body)
    {
        Status = status;
        Message = message;
        Body = body;
    }

    public int Status { get; set; }
    public string Message { get; set; }
    public T Body { get; set; }
}