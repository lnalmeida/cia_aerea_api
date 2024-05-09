namespace cia_aerea_api.ViewModels.Cancellation;

public class CancellationViewModel
{
    public CancellationViewModel(string cancelationReason, DateTime notificationDateTime, int flightId)
    {
        CancelationReason = cancelationReason;
        NotificationDateTime = notificationDateTime;
        FlightId = flightId;
    }

    public string CancelationReason { get; set; }
    public DateTime NotificationDateTime { get; set; }
    public int FlightId { get; set; }
}