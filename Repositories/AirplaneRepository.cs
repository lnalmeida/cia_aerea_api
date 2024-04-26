using System.Linq.Expressions;
using cia_aerea_api.Contexts;
using cia_aerea_api.Models;
using cia_aerea_api.Validators.Services;
using cia_aerea_api.Validators.Airplanes;
using cia_aerea_api.ViewModels.Airplane;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cia_aerea_api.Repositories;

public class AirplaneRepository 
{
    private readonly CiaAereaContext _context;


    public AirplaneRepository(CiaAereaContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ListAirplaneViewModel>> GetAllAsync()
    {
        var airplanes = await _context.Airplanes.AsNoTracking().ToListAsync();
        var airplanesViewModelList = airplanes.Select<Airplane, ListAirplaneViewModel>(a =>
        { 
            return new ListAirplaneViewModel(
                a.Id,
                a.Manufacturer,
                a.Model,
                a.Prefix
            );
        }). ToList();

        return airplanesViewModelList;
    }

    public async Task<DetailAirplaneViewModel> GetByIdAsync(int id)
    {
        var airplane = await _context.Airplanes.FindAsync(id);
        if (airplane is null)
        {
            return null!;
        }

        var detailAriplaneViewModel = new DetailAirplaneViewModel(
            airplane.Id,
            airplane.Manufacturer,
            airplane.Model,
            airplane.Prefix
        );

        return detailAriplaneViewModel;
    }

    public async Task<IEnumerable<ListAirplaneViewModel>> FindAsync(Expression<Func<Models.Airplane, bool>> predicate)
    {
        var airplanes =  await _context.Airplanes.AsNoTracking().Where(predicate).ToListAsync();
        var airplanesViewModelList = airplanes.Select(a =>
        {
            return new ListAirplaneViewModel(
                a.Id,
                a.Manufacturer,
                a.Model,
                a.Prefix
            );
        }).ToList();

        return airplanesViewModelList;
    }

    public async Task<DetailAirplaneViewModel> AddAsync(AddAirplaneViewModel entity)
    {
        var airplane = new Models.Airplane(entity.Manufacturer, entity.Model, entity.Prefix);
        
        _context.Airplanes.Add(airplane);
        await _context.SaveChangesAsync();
        return new DetailAirplaneViewModel(
            airplane.Id, 
            airplane.Manufacturer, 
            airplane.Model, 
            airplane.Prefix
        );
    }

    public async Task<DetailAirplaneViewModel> UpdateAsync(UpdateAirplaneViewModel entity)
    {
        var entityToUpdate = await _context.Airplanes.FindAsync(entity.Id);
        if (entityToUpdate != null)
        {
            _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return new DetailAirplaneViewModel(
                    entity.Id,
                    entity.Manufacturer,
                    entity.Model,
                    entity.Prefix
                );
        }
        return null!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entityToRemove = await _context.Airplanes.FindAsync(id);
        if (entityToRemove != null)
        {
            _context.Airplanes.Remove(entityToRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}