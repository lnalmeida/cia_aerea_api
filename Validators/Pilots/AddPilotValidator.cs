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
            .NotEmpty().WithMessage("The Name field cannot be null.")
            .MaximumLength(100).WithMessage("The field Name must contain a maximum of 50 characters");
        
        RuleFor(p => p.Registration)
            .NotEmpty().WithMessage("The Registration field cannot be null.")
            .MaximumLength(10).WithMessage("The field Registration must contain a maximum of 10 characters")
            .Must(registration => _context.Pilots.Count(a => a.Registration == registration) == 0)
            .WithMessage("There is already a pilot registered with this registration.");
    }
}