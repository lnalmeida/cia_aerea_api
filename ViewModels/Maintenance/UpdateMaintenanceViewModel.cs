using cia_aerea_api.Models.Enums;

namespace cia_aerea_api.ViewModels.Maintenance;

public class UpdateMaintenanceViewModel
{
    public UpdateMaintenanceViewModel(int id, DateTime maintenanceDateTime, string? comments, MaintenanceType typeOfMaintenance, int airplaneId)
    {
        Id = id;
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
}