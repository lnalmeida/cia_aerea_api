using System.ComponentModel.DataAnnotations;

namespace cia_aerea_api.Models;

public class Airplane
{
    public Airplane(string manufacturer, string model, string prefix) 
    {
        Manufacturer = manufacturer;
        Model = model;
        Prefix = prefix;
           
    }
    
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string Manufacturer { get; set; } 
    [MaxLength(50)]
    public string Model { get; set; }
    [MaxLength(10)]
    public string Prefix { get; set; } 

    public ICollection<Maintenance> Maintenances { get; set; } = null!;
    public ICollection<Flight> Flights { get; set; } = null!;
}