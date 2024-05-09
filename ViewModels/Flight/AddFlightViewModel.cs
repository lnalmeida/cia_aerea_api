namespace cia_aerea_api.ViewModels.Flight;

public class AddFlightViewModel
{
    public AddFlightViewModel(string origin, string destiny, DateTime departureDateTime, DateTime arrivalDateTime, int airplaneId, int pilotId)
    {
        Origin = origin;
        Destiny = destiny;
        DepartureDateTime = departureDateTime;
        ArrivalDateTime = arrivalDateTime;
        AirplaneId = airplaneId;
        PilotId = pilotId;
    }
    
    public string Origin { get; set; }
    public string Destiny { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public DateTime ArrivalDateTime { get; set; }
    public int AirplaneId { get; set; }
    public int PilotId { get; set; }

}