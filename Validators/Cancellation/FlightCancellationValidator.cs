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
                validationContext.AddFailure("Invalid flight Id.");
            }
            else
            {
                if (flight.FlightCancelations != null)
                {
                    validationContext.AddFailure("You can't cancel a flinght already cacelled.");
                }

                if (flight.DepartureDateTime <= DateTime.Now && flight.ArrivalDateTime >= DateTime.Now || flight.ArrivalDateTime <= DateTime.Now)
                {
                    validationContext.AddFailure("It is not possible to cancel a flight that is in progress or has already completed.");
                }
            }
        });
    }
}