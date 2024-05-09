using cia_aerea_api.Contexts;
using cia_aerea_api.Models;
using cia_aerea_api.ViewModels.Airplane;
using cia_aerea_api.ViewModels.Flight;
using cia_aerea_api.ViewModels.Pilot;
using Microsoft.EntityFrameworkCore;

namespace cia_aerea_api.Repositories;

public class FlightRepository 
{
    private readonly CiaAereaContext _context;


    public FlightRepository(CiaAereaContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DetailFlightViewModel>> GetAllAsync(string? origin, string? destiny, DateTime? departure, DateTime? arrival)
    {
        var filterOrigin = (Flight flight) => string.IsNullOrWhiteSpace(origin) || (flight.Origin.IndexOf(origin, StringComparison.OrdinalIgnoreCase) >= 0) || flight.Origin == origin;
        var filterDestiny = (Flight flight) => string.IsNullOrWhiteSpace(destiny) ||  (flight.Destiny.IndexOf(destiny, StringComparison.OrdinalIgnoreCase) >= 0) || flight.Destiny == destiny;
        var filterDeparture = (Flight flight) => !departure.HasValue || flight.DepartureDateTime >= departure;
        var filterArrival = (Flight flight) => !arrival.HasValue || flight.ArrivalDateTime <= arrival;
        
        var flights =  _context.Flights.AsNoTracking()
            .Include(f => f.Airplane)
            .Include(f => f.Pilot)
            .Where(filterOrigin)
            .Where(filterDestiny)
            .Where(filterDeparture)
            .Where(filterArrival)
            .Select<Flight, DetailFlightViewModel>(f => new DetailFlightViewModel(
                f.Id,
                f.Origin,
                f.Destiny,
                f.DepartureDateTime,
                f.ArrivalDateTime,
                f.AirplaneId,
                f.PilotId
            )).ToList();

        return flights;
    }

    public async Task<DetailFlightViewModel> GetByIdAsync(int id)
    {
        var flight = await _context.Flights
            .Include(f => f.Pilot)
            .Include(f => f.Airplane)
            .FirstOrDefaultAsync(f => f.Id == id);
        if (flight == null)
        {
            return null!;
        }

        var flightAirplane = new DetailAirplaneViewModel(
            flight.Airplane.Id,
            flight.Airplane.Manufacturer,
            flight.Airplane.Model,
            flight.Airplane.Prefix
        );

        var flightPilot = new DetailPilotViewModel(
            flight.Pilot.Id,
            flight.Pilot.Name,
            flight.Pilot.Registration
        );

        var detailFlightViewModel = new DetailFlightViewModel(
            flight.Id,
            flight.Origin,
            flight.Destiny,
            flight.DepartureDateTime,
            flight.ArrivalDateTime,
            flight.AirplaneId,
            flight.PilotId
        );

        return detailFlightViewModel;
    }

    public async Task<DetailFlightViewModel> AddAsync(AddFlightViewModel entity)
    {
        var flight = new Models.Flight(
            entity.Origin, 
            entity.Destiny,
            entity.DepartureDateTime,
            entity.ArrivalDateTime,
            entity.AirplaneId,
            entity.PilotId
            );
        
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();
        
        return new DetailFlightViewModel(
            flight.Id,
            flight.Origin,
            flight.Destiny,
            flight.DepartureDateTime, 
            flight.ArrivalDateTime,
            flight.AirplaneId,
            flight.PilotId
        );
    }
    
    public async Task<DetailFlightViewModel> UpdateAsync(UpdateFlightViewModel entity)
    {
        var entityToUpdate = await _context.Flights.FindAsync(entity.Id);
        if (entityToUpdate != null)
        {
            _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return new DetailFlightViewModel(
                entity.Id,
                entity.Origin, 
                entity.Destiny,
                entity.DepartureDateTime,
                entity.ArrivalDateTime,
                entity.AirplaneId,
                entity.PilotId
            );
        }
        return null!;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var entityToRemove = await _context.Flights.FindAsync(id);
        if (entityToRemove != null)
        {
            _context.Flights.Remove(entityToRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
    
}