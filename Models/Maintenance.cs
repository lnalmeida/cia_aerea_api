namespace cia_aerea_api.Models

public class Maintenance
{
    public Maintenance(DateTime maintenanceDateTime, MaintenanceType typeOfMaintenance, int airplaneId, string comments = null) 
    {
        MaintenanceDateTime = maintenanceDateTime;
        Comments = comments;
        TypeOfMaintenance = typeOfMaintenance;
        AirplaneId = airplaneId;           
    }

    public int Id { get; set; }
    public DateTime MaintenanceDateTime { get; set; }
    public string? Comments { get; set; }
    public MaintenanceType TypeOfMaintenance { get; set; }
    public int AirplaneId { get; set; }

    public Airplane Airplane { get; set; } = null!
}
