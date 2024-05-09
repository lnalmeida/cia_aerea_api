namespace cia_aerea_api.Models;

public class FlightCancellation
{
    public FlightCancellation(string cancelationReason, DateTime notificationDateTime, int flightId) 
    {
        CancelationReason = cancelationReason;
        NotificationDateTime = notificationDateTime;
        FlightId = flightId;                
    }

    public int Id { get; set; }
    public string CancelationReason { get; set; }
    public DateTime NotificationDateTime { get; set; }
    public int FlightId { get; set; }
    public Flight Flight { get; set; } = null!;
}
