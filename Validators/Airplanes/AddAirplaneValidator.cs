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
            .NotEmpty().WithMessage("O campo FABRICANTE deve ser informado.")
            .MaximumLength(50).WithMessage("O campo FABRICANTE deve conter o máximo de 50 caracteres");
        
        RuleFor(a => a.Model)
            .NotEmpty().WithMessage("O campo MODELO deve ser informado.")
            .MaximumLength(50).WithMessage("O campo MODELO deve conter o máximo de 50 caracteres");
        
        RuleFor(a => a.Prefix)
            .NotEmpty().WithMessage("O campo PREFIXO deve ser informado..")
            .MaximumLength(10).WithMessage("O campo PREFIXO deve conter o máximo de 50 caracteres")
            .Must(prefix => _context.Airplanes.Count(a => a.Prefix == prefix) == 0)
            .WithMessage("Já existe uma aeronave registrada com esse prefixo.");
    }
}