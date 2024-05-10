using cia_aerea_api.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata;

namespace cia_aerea_api.Validators.Maintenances;

public class DeleteMaintenanceValidator : AbstractValidator<int>
{
    private readonly CiaAereaContext _context;

    public DeleteMaintenanceValidator(CiaAereaContext context)
    {
        _context = context;

        RuleFor(id => _context.Maintenances.Find(id))
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Id de manutenção inválido!")
            .Must(maintenance => maintenance.MaintenanceDateTime > DateTime.Now)
            .WithMessage("Não é possível excluir uma manutenção já realizada.");
    }
}