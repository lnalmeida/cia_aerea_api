using cia_aerea_api.Models;

namespace cia_aerea_api.Models;

public class Flight
{
    public Flight( string origin, string destiny, DateTime departureDateTime, DateTime arrivalDateTime, int airplaneId, int pilotId) 
    {
        Origin = origin;
        Destiny = destiny;
        DepartureDateTime = departureDateTime;
        ArrivalDateTime = arrivalDateTime;
        AirplaneId = airplaneId;
        PilotId = pilotId;
    }

    public int Id { get; set; }
    public string Origin { get; set; }
    public string Destiny { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public DateTime ArrivalDateTime { get; set; }
    public int AirplaneId { get; set; }
    public int PilotId { get; set; }

    public Airplane Airplane { get; set; } = null!;
    public Pilot Pilot { get; set; } = null!;
    public FlightCancellation? FlightCancelations { get; set; } 
}
