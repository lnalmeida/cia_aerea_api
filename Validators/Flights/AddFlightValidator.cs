using System.ComponentModel;
using cia_aerea_api.Contexts;
using cia_aerea_api.ViewModels.Flight;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace cia_aerea_api.Validators.Flights;

public class AddFlightValidator : AbstractValidator<AddFlightViewModel>
{
    private readonly CiaAereaContext _context;

    public AddFlightValidator(CiaAereaContext context)
    {
        _context = context;

        RuleFor(f => f.Origin)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("The Origin field cannot be null.")
            .Length(3).WithMessage("The field Origin must contain 3 characters");
            
        RuleFor(f => f.Destiny)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("The Destiny field cannot be null.")
            .Length(3).WithMessage("The field Destiny must contain 3 characters");

        RuleFor(f => f)
            .Cascade(CascadeMode.Stop)
            .Must(flight => flight.DepartureDateTime >= DateTime.Now.AddHours(1)).WithMessage("Departure time must be at least 1 hour from now.")
            .Must(flight => flight.ArrivalDateTime > flight.DepartureDateTime).WithMessage("Arrival time must be at greater than Departure time.");

        RuleFor(v => v).Custom((flight, validationContext) =>
        {
            var pilot = _context.Pilots
                .Include(p => p.Flights)
                .FirstOrDefault(p => p.Id == flight.PilotId);
            if (pilot == null)
            {
                validationContext.AddFailure("Invalid pilot.");
            }
            else
            {
                var pilotInFlight = pilot.Flights.Any(f =>
                    (f.DepartureDateTime <= flight.DepartureDateTime && f.ArrivalDateTime >= flight.ArrivalDateTime) ||
                    (f.DepartureDateTime >= flight.DepartureDateTime && f.ArrivalDateTime <= flight.ArrivalDateTime) ||
                    (f.ArrivalDateTime >= flight.DepartureDateTime && f.ArrivalDateTime <= flight.ArrivalDateTime));

                if (pilotInFlight)
                {
                    validationContext.AddFailure("This pilot will be flying at the selected time.");
                }
            }

            var airplane = _context.Airplanes
                .Include(a => a.Flights)
                .Include(a => a.Maintenances)
                .FirstOrDefault(a => a.Id == flight.AirplaneId);

            if (airplane == null)
            {
                validationContext.AddFailure("Invalid airplane.");
            }
            else
            {
                var airplaneInFlight = airplane.Flights.Any(f =>  
                    (f.DepartureDateTime <= flight.DepartureDateTime && f.ArrivalDateTime >= flight.ArrivalDateTime) ||
                    (f.DepartureDateTime >= flight.DepartureDateTime && f.ArrivalDateTime <= flight.ArrivalDateTime) ||
                    (f.ArrivalDateTime >= flight.DepartureDateTime && f.ArrivalDateTime <= flight.ArrivalDateTime));

                if (airplaneInFlight)
                {
                    validationContext.AddFailure("This airplane will be flying at the selected time.");
                }

                var airplaneInMantenance = airplane.Maintenances.Any(m =>
                    m.MaintenanceDateTime >= flight.DepartureDateTime &&
                    m.MaintenanceDateTime <= flight.ArrivalDateTime);

                if (airplaneInMantenance)
                {
                    validationContext.AddFailure("This airplane will be in maintenance at the selected time.");
                }
            }
        });
    }
}