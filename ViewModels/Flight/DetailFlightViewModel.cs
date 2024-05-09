using System.Text.Json.Serialization;
using cia_aerea_api.ViewModels.Airplane;
using cia_aerea_api.ViewModels.Pilot;

namespace cia_aerea_api.ViewModels.Flight;

public class DetailFlightViewModel
{
    public DetailFlightViewModel(int id, string origin, string destiny, DateTime departureDateTime, DateTime arrivalDateTime, int airplaneId, int pilotId, DetailAirplaneViewModel? airplane, DetailPilotViewModel? pilot)
    {
        Id = id;
        Origin = origin;
        Destiny = destiny;
        DepartureDateTime = departureDateTime;
        ArrivalDateTime = arrivalDateTime;
        AirplaneId = airplaneId;
        PilotId = pilotId;
        Airplane = airplane;
        Pilot = pilot;
    }
  
    public DetailFlightViewModel(int id, string origin, string destiny, DateTime departureDateTime, DateTime arrivalDateTime)
    {
        Id = id;
        Origin = origin;
        Destiny = destiny;
        DepartureDateTime = departureDateTime;
        ArrivalDateTime = arrivalDateTime;
    }

    public int Id { get; set; }
    public string Origin { get; set; }
    public string Destiny { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public DateTime ArrivalDateTime { get; set; }
    public int AirplaneId { get; set; }
    public int PilotId { get; set; }
    public DetailAirplaneViewModel? Airplane { get; set; }
    public DetailPilotViewModel? Pilot { get; set; }
}