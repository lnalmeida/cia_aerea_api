namespace cia_aerea_api.Validators.Model;

public class ValidationResultModel
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; }
}