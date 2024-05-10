using cia_aerea_api.Contexts;
using cia_aerea_api.ViewModels.Pilot;
using FluentValidation;

namespace cia_aerea_api.Validators.Pilots;

public class AddPilotValidator : AbstractValidator<AddPilotViewModel>
{
    private readonly CiaAereaContext _context;
    
    public AddPilotValidator(CiaAereaContext context)
    {
        _context = context;
        
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("O campo NOME deve ser informado.")
            .MaximumLength(100).WithMessage("O campo NOME deve ter o máximo de 50 caracteres.");
        
        RuleFor(p => p.Registration)
            .NotEmpty().WithMessage("O campo MATRÍCULA deve ser informado.")
            .MaximumLength(10).WithMessage("O campo MATRÍCULA deve ter o máximo de 10 caracteres.")
            .Must(registration => _context.Pilots.Count(a => a.Registration == registration) == 0)
            .WithMessage("Já existe um piloto registrado com esse nº de matrícula.");
    }
}