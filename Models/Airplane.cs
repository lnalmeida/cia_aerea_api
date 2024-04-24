namespace cia_aerea_api.Models

public class Airplane
{
    public Airplane(string manufacturer, string model, string prefix) 
    {
        manufacturer = manufacturer;
        Model = model;
        Prefix = prefix;
           
    }
    
    public int Id { get; set; }
    public string manufacturer { get; set; } 
    public string Model { get; set; } 
    public string Prefix { get; set; } 

    public ICollection<Maintenance> Maintenances { get; set; } = null!;
    public ICollection<Flight> Flights { get; set; } = null!;
}