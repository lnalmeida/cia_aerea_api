using cia_aerea_api.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace cia_aerea_api.Validators.Pilots;

public class DeletePilotValidator : AbstractValidator<int>
{
    private readonly CiaAereaContext _context;

    public DeletePilotValidator(CiaAereaContext context)
    {
        _context = context;

        RuleFor(id => _context.Pilots
                .Include(p => p.Flights)
                .FirstOrDefault(p => p.Id == id))
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("The pilot Id is invalid.")
            .Must(pilot => pilot!.Flights.Count == 0)
            .WithMessage("It is not possible to delete a pilot that already has flights registered.");
    }
}