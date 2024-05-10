using cia_aerea_api.Contexts;
using cia_aerea_api.ViewModels.Flight;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace cia_aerea_api.Validators.Flights;

public class UpdateFlightValidator : AbstractValidator<UpdateFlightViewModel>
{
    private readonly CiaAereaContext _context;

    public UpdateFlightValidator(CiaAereaContext context)
    {
        _context = context;

        RuleFor(f => f.Origin)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("O campo ORIGEM deve ser informado.")
            .Length(3).WithMessage("O campo ORIGEM deve conter 3 caracteres.");
            
        RuleFor(f => f.Destiny)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("\"O campo DESTINO deve ser informado.")
            .Length(3).WithMessage("O campo DESTINO deve conter 3 caracteres.");

        RuleFor(f => f)
            .Cascade(CascadeMode.Stop)
            .Must(flight => flight.ArrivalDateTime > flight.DepartureDateTime).WithMessage("A data/hora de chegada deve ser maior que a data/hora de partida.");

        RuleFor(v => v).Custom((flight, validationContext) =>
        {
            var pilot = _context.Pilots
                .Include(p => p.Flights)
                .FirstOrDefault(p => p.Id == flight.PilotId);
            if (pilot == null)
            {
                validationContext.AddFailure("Piloto inválido.");
            }
            else
            {
                var pilotInFlight = pilot.Flights.Any(f => f.Id != flight.Id &&
                    (f.DepartureDateTime <= flight.DepartureDateTime && f.ArrivalDateTime >= flight.ArrivalDateTime) ||
                    (f.DepartureDateTime >= flight.DepartureDateTime && f.ArrivalDateTime <= flight.ArrivalDateTime) ||
                    (f.ArrivalDateTime >= flight.DepartureDateTime && f.ArrivalDateTime <= flight.ArrivalDateTime));

                if (pilotInFlight)
                {
                    validationContext.AddFailure("O piloto estará em um outro vôo no horário selecionado.");
                }
            }

            var airplane = _context.Airplanes
                .Include(a => a.Flights)
                .Include(a => a.Maintenances)
                .FirstOrDefault(a => a.Id == flight.AirplaneId);

            if (airplane == null)
            {
                validationContext.AddFailure("Aeronave inválida.");
            }
            else
            {
                var airplaneInFlight = airplane.Flights.Any(f =>  f.Id != flight.Id &&
                    (f.DepartureDateTime <= flight.DepartureDateTime && f.ArrivalDateTime >= flight.ArrivalDateTime) ||
                    (f.DepartureDateTime >= flight.DepartureDateTime && f.ArrivalDateTime <= flight.ArrivalDateTime) ||
                    (f.ArrivalDateTime >= flight.DepartureDateTime && f.ArrivalDateTime <= flight.ArrivalDateTime));

                if (airplaneInFlight)
                {
                    validationContext.AddFailure("A aeronave estará em um outro vôo no horário selecionado.");
                }

                var airplaneInMantenance = airplane.Maintenances.Any(m =>
                    m.MaintenanceDateTime >= flight.DepartureDateTime &&
                    m.MaintenanceDateTime <= flight.ArrivalDateTime);

                if (airplaneInMantenance)
                {
                    validationContext.AddFailure("A aeronave estará em manutenção no horário selecionado.");
                }
            }
        });
    }
}