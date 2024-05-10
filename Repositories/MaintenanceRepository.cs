using cia_aerea_api.Contexts;
using cia_aerea_api.Models;
using cia_aerea_api.Models.Enums;
using cia_aerea_api.ViewModels.Maintenance;
using Microsoft.EntityFrameworkCore;

namespace cia_aerea_api.Repositories;

public class MaintenanceRepository
{
    private readonly CiaAereaContext _context;

    public MaintenanceRepository(CiaAereaContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ListMaintenanceViewModel>> GetAllByAirplaneIdAsync(int airplaneId, DateTime? startMaintenanceDate, DateTime? endMaintenanceDate, MaintenanceType? type)
    {
        var maintenanceStartDateFilter = (Maintenance maintenance) =>
            (!startMaintenanceDate.HasValue || maintenance.MaintenanceDateTime >= startMaintenanceDate);
        var maintenanceEndDateFilter = (Maintenance maintenance) =>
            (!endMaintenanceDate.HasValue || maintenance.MaintenanceDateTime <= endMaintenanceDate);
        var typeMaintenanceFilter = (Maintenance maintenance) => (type == null || maintenance.TypeOfMaintenance == type);
        
        var maintenances =  _context.Maintenances
            .AsNoTracking()
            .Where(m => m.AirplaneId == airplaneId).AsEnumerable()
            .Where(maintenanceStartDateFilter)
            .Where(maintenanceEndDateFilter)
            .Where(typeMaintenanceFilter)
            .Select<Maintenance, ListMaintenanceViewModel>(m => new ListMaintenanceViewModel(
                    m.Id,
                    m.MaintenanceDateTime,
                    m.Comments,
                    m.TypeOfMaintenance,
                    m.AirplaneId
                )
    )
            .ToList();
        return maintenances;
    }

    public async Task<ListMaintenanceViewModel> GetMaintenanceByIdAsync(int id)
    {
        var maintenance = await _context.Maintenances.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        if (maintenance != null)
        {
            return new ListMaintenanceViewModel(
                maintenance.Id,
                maintenance.MaintenanceDateTime,
                maintenance.Comments,
                maintenance.TypeOfMaintenance,
                maintenance.AirplaneId
            );
        }
        return null!;
    }

    public async Task<ListMaintenanceViewModel> AddMaintenanceAsync(AddManintenanceViewModel entity)
    {
        var maintenance = new Maintenance(
            entity.MaintenanceDateTime,
            entity.TypeOfMaintenance,
            entity.AirplaneId,
            entity.Comments!
        );

        _context.Maintenances.Add(maintenance);
        await _context.SaveChangesAsync();

        var newMaintenance = await GetMaintenanceByIdAsync(maintenance.Id);
        return newMaintenance;
    }

    public async Task<ListMaintenanceViewModel> UpdateMaintenanceAsync(UpdateMaintenanceViewModel entity)
    {
        var maintenanceToUpdate = await _context.Maintenances.FindAsync(entity.Id);

        if (maintenanceToUpdate != null)
        {
            _context.Entry(maintenanceToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            var updatedMaintenance = await GetMaintenanceByIdAsync(entity.Id);
            return updatedMaintenance;
        }

        return null!;
    }

    public async Task<bool> DeleteMaintenanceAsync(int id)
    {
        var entityToRemove = await _context.Maintenances.FindAsync(id);
        if (entityToRemove != null)
        {
            _context.Maintenances.Remove(entityToRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
    
    
}