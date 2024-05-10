using cia_aerea_api.Contexts;
using FluentValidation;

namespace cia_aerea_api.Validators.Flights;

public class DeleteFlightValidator : AbstractValidator<int>
{
    private readonly CiaAereaContext _context;

    public DeleteFlightValidator(CiaAereaContext context)
    {
        _context = context;

        RuleFor(id => _context.Flights.Find(id))
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("O ID do vôo é inválido!")
            .Must(flight => flight.ArrivalDateTime >= DateTime.Now)
            .WithMessage("Não é possível deletar um vôo finalizado.");
    }
}