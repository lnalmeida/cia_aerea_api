namespace cia_aerea_api.ViewModels.Cancellation;

public class CancellationDetailViewModel
{
    public CancellationDetailViewModel(int id, string cancelationReason, DateTime notificationDateTime, int flightId)
    {
        Id = id;
        CancelationReason = cancelationReason;
        NotificationDateTime = notificationDateTime;
        FlightId = flightId;
    }

    public int Id { get; set; }
    public string CancelationReason { get; set; }
    public DateTime NotificationDateTime { get; set; }
    public int FlightId { get; set; }
}