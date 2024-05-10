using System.Text;
using cia_aerea_api.Contexts;
using cia_aerea_api.Models;
using cia_aerea_api.ViewModels.Airplane;
using cia_aerea_api.ViewModels.Cancellation;
using cia_aerea_api.ViewModels.Flight;
using cia_aerea_api.ViewModels.Pilot;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace cia_aerea_api.Repositories;

public class FlightRepository 
{
    private readonly CiaAereaContext _context;
    private readonly IConverter _converter;


    public FlightRepository(CiaAereaContext context, IConverter converter)
    {
        _context = context;
        _converter = converter;
    }

    public async Task<IEnumerable<ListFlightViewModel>> GetAllAsync(string? origin, string? destiny, DateTime? departure, DateTime? arrival)
    {
        var filterOrigin =  (Flight flight) => string.IsNullOrWhiteSpace(origin) || (flight.Origin.IndexOf(origin, StringComparison.OrdinalIgnoreCase) >= 0) || flight.Origin == origin;
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
            .Select<Flight, ListFlightViewModel>(f => new ListFlightViewModel(
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
            .Include(f => f.FlightCancelations)
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

        var detailFlightViewModel = new DetailFlightViewModel();

        if (flight.FlightCancelations != null)
        {
            var flightCancellation = new CancellationDetailViewModel(
                flight.FlightCancelations.Id,
                flight.FlightCancelations.CancelationReason,
                flight.FlightCancelations.NotificationDateTime,
                flight.FlightCancelations.FlightId
            );
            detailFlightViewModel.Id = flight.Id;
            detailFlightViewModel.Origin = flight.Origin;
            detailFlightViewModel.Destiny = flight.Destiny;
            detailFlightViewModel.DepartureDateTime = flight.DepartureDateTime;
            detailFlightViewModel.ArrivalDateTime = flight.ArrivalDateTime;
            detailFlightViewModel.AirplaneId = flight.AirplaneId;
            detailFlightViewModel.PilotId = flight.PilotId;
            detailFlightViewModel.Airplane = flightAirplane;
            detailFlightViewModel.Pilot = flightPilot;
            detailFlightViewModel.Cancellation = flightCancellation;
        }
        else
        {
            detailFlightViewModel.Id = flight.Id;
            detailFlightViewModel.Origin = flight.Origin;
            detailFlightViewModel.Destiny = flight.Destiny;
            detailFlightViewModel.DepartureDateTime = flight.DepartureDateTime;
            detailFlightViewModel.ArrivalDateTime = flight.ArrivalDateTime;
            detailFlightViewModel.AirplaneId = flight.AirplaneId;
            detailFlightViewModel.PilotId = flight.PilotId;
            detailFlightViewModel.Airplane = flightAirplane;
            detailFlightViewModel.Pilot = flightPilot;
        }

        return detailFlightViewModel;
    }

    public async Task<DetailFlightViewModel> AddAsync(AddFlightViewModel entity)
    {
        var flight = new Flight(
            entity.Origin, 
            entity.Destiny,
            entity.DepartureDateTime,
            entity.ArrivalDateTime,
            entity.AirplaneId,
            entity.PilotId
            );
        
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();
        
        var newFlight = await GetByIdAsync(flight.Id);
        return  newFlight;
    }
    
    public async Task<DetailFlightViewModel> UpdateAsync(UpdateFlightViewModel entity)
    {
        var entityToUpdate = await _context.Flights.FindAsync(entity.Id);
        if (entityToUpdate != null)
        {
            _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            var updatedFlight = await GetByIdAsync(entity.Id);
            return  updatedFlight;
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

    public async Task<DetailFlightViewModel?> FlightCancellation(CancellationViewModel cancellationData)
    {
        var flightCancellation = new FlightCancellation(cancellationData.CancelationReason,
            cancellationData.NotificationDateTime, cancellationData.FlightId);

        var cancelledFlight =  _context.FlightCancelations.Add(flightCancellation);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(cancellationData.FlightId);
    }

    private async Task<Flight> GetFlightRecord(int id)
    {
                var flight =  await _context.Flights
                    .Include((f => f.Airplane))
                    .Include(f => f.Pilot)
                    .Include(f => f.FlightCancelations)
                    .FirstOrDefaultAsync(f => f.Id == id);

                return flight;
    }

    public async Task<byte[]> GenerateFlightRecordPDF(int id)
    {
        var flight = await  GetFlightRecord(id);
        if (flight != null)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(
                $"<h1 style='text-align: center'>Ficha do Voo {flight?.Id.ToString().PadLeft(10, '0')}</h1>")
                .Append($"<hr>")
                .Append($"<p><b>ORIGEM:</b> { flight?.Origin } (saída em { flight?.DepartureDateTime:dd/MM/yyyy} às { flight?.DepartureDateTime:hh:mm})</p>")
                .Append($"<p><b>DESTINO:</b> { flight?.Destiny} (chegada em { flight?.ArrivalDateTime:dd/MM/yyyy} às { flight?.ArrivalDateTime:hh:mm})</p>")
                .Append($"<hr>")
                .Append($"<p><b>AERONAVE:</b> { flight?.Airplane!.Prefix } ({ flight?.Airplane.Manufacturer } { flight?.Airplane.Model })</p>")
                .Append($"<hr>")
                .Append($"<p><b>PILOTO:</b> { flight?.Pilot!.Name } ({ flight?.Pilot.Registration})</p>")
                .Append($"<hr>");
            
            if (flight.FlightCancelations != null)
            {
                strBuilder.Append($"<p style='color: red'><b>VOO CANCELADO:</b> {flight?.FlightCancelations.CancelationReason}</p>");
            }

            var docPDF = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = strBuilder.ToString(),
                        WebSettings = { DefaultEncoding = "utf-8"}
                    }
                }
            };

            return _converter.Convert(docPDF);
        }

        return null;
    }
}