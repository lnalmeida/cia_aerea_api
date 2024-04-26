namespace cia_aerea_api.ViewModels.Generics;

public class ResponseViewModel<T> 
{
    public ResponseViewModel(int status, string message, T body, IEnumerable<string> errors = null!)
    {
        Status = status;
        Message = message;
        Body = body;
        Errors = errors ?? new List<string>();
    }

    public int Status { get; set; }
    public string Message { get; set; }
    public T Body { get; set; }
    public IEnumerable<string> Errors { get; set; }
}