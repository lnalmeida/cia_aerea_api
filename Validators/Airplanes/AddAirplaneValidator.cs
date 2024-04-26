using cia_aerea_api.Contexts;
using cia_aerea_api.Repositories;
using cia_aerea_api.ViewModels.Airplane;
using FluentValidation;

namespace cia_aerea_api.Validators.Airplanes;

public class AddAirplaneValidator : AbstractValidator<AddAirplaneViewModel>
{
    private readonly CiaAereaContext _context;
    
    public AddAirplaneValidator(CiaAereaContext context)
    {
        _context = context;
        
        RuleFor(a => a.Manufacturer)
            .NotEmpty().WithMessage("The Manufacturer field cannot be null.")
            .MaximumLength(50).WithMessage("The field Manufacturer must contain a maximum of 50 characters");
        
        RuleFor(a => a.Model)
            .NotEmpty().WithMessage("The Model field cannot be null.")
            .MaximumLength(50).WithMessage("The field Model must contain a maximum of 50 characters");
        
        RuleFor(a => a.Prefix)
            .NotEmpty().WithMessage("The Prefix field cannot be null.")
            .MaximumLength(50).WithMessage("The field Prefix must contain a maximum of 10 characters")
            .Must(prefix => _context.Airplanes.Count(a => a.Prefix == prefix) == 0).WithMessage("There is already a plane registered with this prefix.");
    }
}