namespace cia_aerea_api.ViewModels.Airplane;

public class UpdateAirplaneViewModel
{
    public UpdateAirplaneViewModel(int id, string manufacturer, string model, string prefix)
    { 
        Id = id;
        Manufacturer = manufacturer;
        Model = model;
        Prefix = prefix;
    }

    public int Id { get; set; }
    public string Manufacturer { get; set; } 
    public string Model { get; set; } 
    public string Prefix { get; set; } 
}