namespace cia_aerea_api.Models
public class Pilot
{
    public Pilot(string name, string registration) 
    {
        Name = name;
        Registration = registration;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string Registration { get; set; }

    public ICollection<Flight> Flights { get; set; } = null!;
}
