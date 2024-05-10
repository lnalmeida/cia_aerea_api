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
            .NotNull().WithMessage("O Id do piloto é inválido.")
            .Must(pilot => pilot!.Flights.Count == 0)
            .WithMessage("Não é possível deletar um piloto que já tenha vôos registrados.");
    }
}