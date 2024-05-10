using cia_aerea_api.Contexts;
using cia_aerea_api.ViewModels.Cancellation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace cia_aerea_api.Validators.Cancellation;

public class FlightCancellationValidator : AbstractValidator<CancellationViewModel>
{
    private readonly CiaAereaContext _context;

    public FlightCancellationValidator(CiaAereaContext context)
    {
        _context = context;

        RuleFor(c => c).Custom((cancellation, validationContext) =>
        {
            var flight =  _context.Flights
                .Include(f => f.FlightCancelations)
                .FirstOrDefault(f => f.Id == cancellation.FlightId);
            if (flight == null)
            {
                validationContext.AddFailure("O Id do vôo é inválido.");
            }
            else
            {
                if (flight.FlightCancelations != null)
                {
                    validationContext.AddFailure("Não é possível cancelar um vôo já cancelado.");
                }

                if (flight.DepartureDateTime <= DateTime.Now && flight.ArrivalDateTime >= DateTime.Now || flight.ArrivalDateTime <= DateTime.Now)
                {
                    validationContext.AddFailure("Não é possível cancelar um vôo em andamento ou já finalizado.");
                }
            }
        });
    }
}