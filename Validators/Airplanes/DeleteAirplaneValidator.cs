using cia_aerea_api.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace cia_aerea_api.Validators.Airplanes;

public class DeleteAirplaneValidator : AbstractValidator<int>
{
    private readonly CiaAereaContext _context;

    public DeleteAirplaneValidator(CiaAereaContext context)
    {
        _context = context;

        RuleFor(id => _context.Airplanes
                .Include(a => a.Flights)
                .Include(a => a.Maintenances)
                .FirstOrDefault(a => a.Id == id))
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("The airplane Id is invalid.")
            .Must(airplane => airplane!.Flights.Count == 0)
            .WithMessage("It is not possible to delete an aircraft that already has flights registered.")
            .Must(airplane => airplane!.Maintenances.Count == 0)
            .WithMessage("It is not possible to delete an aircraft that already has maintenance registered.");
    }
}