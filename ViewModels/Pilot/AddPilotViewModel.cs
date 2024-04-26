namespace cia_aerea_api.ViewModels.Pilot;

public class AddPilotViewModel
{
    public AddPilotViewModel(string name, string registration) 
    {
        Name = name;
        Registration = registration;
    }
    
    public string Name { get; set; }
    public string Registration { get; set; }
}