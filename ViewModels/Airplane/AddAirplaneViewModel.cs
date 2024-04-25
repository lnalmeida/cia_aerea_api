namespace cia_aerea_api.ViewModels.Airplane;

public class AddAirplaneViewModel
{
    public AddAirplaneViewModel(string manufacturer, string model, string prefix) 
    {
        Manufacturer = manufacturer;
        Model = model;
        Prefix = prefix;
    }

    public string Manufacturer { get; set; } 
    public string Model { get; set; } 
    public string Prefix { get; set; } 
}