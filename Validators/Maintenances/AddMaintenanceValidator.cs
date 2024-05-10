using cia_aerea_api.Contexts;
using cia_aerea_api.Models;
using cia_aerea_api.ViewModels.Maintenance;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace cia_aerea_api.Validators.Maintenances;

public class AddMaintenanceValidator : AbstractValidator<AddManintenanceViewModel>
{
    private readonly CiaAereaContext _context;

    public AddMaintenanceValidator(CiaAereaContext context)
    {
        _context = context;

        RuleFor(m => m.MaintenanceDateTime)
            .NotEmpty().WithMessage("A data e hora da manutenção devem ser inormadas");
        RuleFor(m => m.TypeOfMaintenance)
            .NotNull().WithMessage("O tipo da manutenção deve ser informado.");

        RuleFor(m => m).Custom((maintenance, validationContext) =>
        {
            var airplaane = _context.Airplanes
                .Include(a => a.Flights).FirstOrDefault(a => a.Id == maintenance.AirplaneId);

            if (airplaane == null)
            {
                validationContext.AddFailure("Id de aeronave inválido!");
            }
            else
            {
                if (airplaane.Flights.Any(f =>
                        f.DepartureDateTime <= maintenance.MaintenanceDateTime && f.ArrivalDateTime >= maintenance.MaintenanceDateTime))
                {
                    validationContext.AddFailure("Não é possível realizar manutenção de areronaves em vôo.");
                }
            }

        });
    }
}

    