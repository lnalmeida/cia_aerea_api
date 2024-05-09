using cia_aerea_api.Contexts;
using FluentValidation;

namespace cia_aerea_api.Validators.Flights;

public class DeleteFlightValidator : AbstractValidator<int>
{
    private readonly CiaAereaContext _context;

    public DeleteFlightValidator(CiaAereaContext context)
    {
        _context = context;

        RuleFor(id => _context.Flights.Find(id))
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Flight id is invalid!")
            .Must(flight => flight.ArrivalDateTime >= DateTime.Now)
            .WithMessage("Not is possible delete an ended flight.");
    }
}