using System.Linq.Expressions;
using cia_aerea_api.Contexts;
using cia_aerea_api.Models;
using cia_aerea_api.ViewModels.Airplane;
using cia_aerea_api.ViewModels.Pilot;
using Microsoft.EntityFrameworkCore;

namespace cia_aerea_api.Repositories;

public class PilotRepository
{
    private readonly CiaAereaContext _context;

    public PilotRepository(CiaAereaContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ListPilotsViewModel>> GetAllAsync()
    {
        var pilots = await _context.Pilots.AsNoTracking().ToListAsync();
        var pilotsViewModelList = pilots.Select<Pilot, ListPilotsViewModel>(p =>
        { 
            return new ListPilotsViewModel(
                p.Id,
                p.Name,
                p.Registration
            );
        }). ToList();

        return pilotsViewModelList;
    }
    
    public async Task<DetailPilotViewModel> GetByIdAsync(int id)
    {
        var pilot = await _context.Pilots.FindAsync(id);
        if (pilot is null)
        {
            return null!;
        }

        var detailPilotViewModel = new DetailPilotViewModel(
            pilot.Id,
            pilot.Name,
            pilot.Registration
        );

        return detailPilotViewModel;
    }
    
    public async Task<IEnumerable<ListPilotsViewModel>> FindAsync(Expression<Func<Models.Pilot, bool>> predicate)
    {
        var pilots =  await _context.Pilots.AsNoTracking().Where(predicate).ToListAsync();
        var pilotsViewModelList = pilots.Select(p =>
        {
            return new ListPilotsViewModel(
                p.Id,
                p.Name,
                p.Registration
            );
        }).ToList();

        return pilotsViewModelList;
    }
    
    public async Task<DetailPilotViewModel> AddAsync(AddPilotViewModel entity)
    {
        var pilot = new Models.Pilot(entity.Name, entity.Registration);
        
        _context.Pilots.Add(pilot);
        await _context.SaveChangesAsync();
        return new DetailPilotViewModel(
            pilot.Id,
            pilot.Name,
            pilot.Registration
        );
    }
    
    public async Task<DetailPilotViewModel> UpdateAsync(UpdatePilotViewModel entity)
    {
        var entityToUpdate = await _context.Pilots.FindAsync(entity.Id);
        if (entityToUpdate != null)
        {
            _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return new DetailPilotViewModel(
                entity.Id,
                entity.Name,
                entity.Registration
            );
        }
        return null!;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var entityToRemove = await _context.Pilots.FindAsync(id);
        if (entityToRemove != null)
        {
            _context.Pilots.Remove(entityToRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}