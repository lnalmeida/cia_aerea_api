using cia_aerea_api.Contexts;
using cia_aerea_api.ViewModels.Airplane;
using FluentValidation;

namespace cia_aerea_api.Validators.Airplanes;

public class UpdateAirplaneValidator : AbstractValidator<UpdateAirplaneViewModel>
{
    private readonly CiaAereaContext _context;
    
    public UpdateAirplaneValidator(CiaAereaContext context)
    {
        _context = context;
        
        RuleFor(a => a.Manufacturer)
            .NotEmpty().WithMessage("O campo FABRICANTE deve ser informado.")
            .MaximumLength(50).WithMessage("The field Manufacturer must contain a maximum of 50 characters");
        
        RuleFor(a => a.Model)
            .NotEmpty().WithMessage("O campo MODELO deve ser informado.")
            .MaximumLength(50).WithMessage("The field Model must contain a maximum of 50 characters");
        
        RuleFor(a => a.Prefix)
            .NotEmpty().WithMessage("O campo PREFIXO deve ser informado.")
            .MaximumLength(10).WithMessage("The field Prefix must contain a maximum of 10 characters");

        RuleFor(a => a)
            .Must(airplane => _context.Airplanes.Count(a => a.Prefix == airplane.Prefix && a.Id != airplane.Id) == 0)
            .WithMessage("Já existe uma aeronave registrada com esse prefixo.");
    }
}