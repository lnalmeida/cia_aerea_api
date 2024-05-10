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
            .NotNull().WithMessage("O Id da aeronave é inválido.")
            .Must(airplane => airplane!.Flights.Count == 0)
            .WithMessage("Não é possível excluir uma aeronave que já tenha vôos registrados.")
            .Must(airplane => airplane!.Maintenances.Count == 0)
            .WithMessage("Não é possível excluma aeronave que já tenha manutenções registradas.");
    }
}