using cia_aerea_api.Validators.Model;
using FluentValidation;

namespace cia_aerea_api.Validators.Services;

public class ValidationService
{
    public async Task<ValidationResultModel> ValidateModel<T>(T model, AbstractValidator<T> validator) where T : class
    {
        var validationResult = await validator.ValidateAsync(model);
        var errors = new List<string>();
        validationResult.Errors.ForEach(e => errors.Add(e.ErrorMessage));
        var result = new ValidationResultModel
        {
            IsValid = validationResult.IsValid,
            Errors = errors
        };

        return result;
    }
}