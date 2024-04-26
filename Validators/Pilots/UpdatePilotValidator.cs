using cia_aerea_api.Contexts;
using cia_aerea_api.ViewModels.Airplane;
using cia_aerea_api.ViewModels.Pilot;
using FluentValidation;

namespace cia_aerea_api.Validators.Pilots;

public class UpdatePilotValidator : AbstractValidator<UpdatePilotViewModel>
{
    private readonly CiaAereaContext _context;
    
    public UpdatePilotValidator(CiaAereaContext context)
    {
        _context = context;
        
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("The Name field cannot be null.")
            .MaximumLength(100).WithMessage("The field Name must contain a maximum of 100 characters");
        
        RuleFor(p => p.Registration)
            .NotEmpty().WithMessage("The Registration field cannot be null.")
            .MaximumLength(10).WithMessage("The field Registration must contain a maximum of 10 characters");
        
        RuleFor(p => p)
            .Must(pilot => _context.Pilots.Count(p => p.Registration == pilot.Registration && p.Id != pilot.Id) == 0)
            .WithMessage("There is already a pilot registered with this registration.");
    }
}