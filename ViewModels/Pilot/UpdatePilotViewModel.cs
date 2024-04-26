namespace cia_aerea_api.ViewModels.Pilot;

public class UpdatePilotViewModel
{
    public UpdatePilotViewModel(int id,string name, string registration)
    {
        Id = id;
        Name = name;
        Registration = registration;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string Registration { get; set; }
}