using FluentValidation;
using HotelBackend.DTOs;

namespace HotelBackend.Validators;

public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
{
    public RegisterDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'email è obbligatoria.")
            .EmailAddress().WithMessage("Formato email non valido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La password è obbligatoria.")
            .MinimumLength(6).WithMessage("La password deve contenere almeno 6 caratteri.")
            .Matches("[A-Z]").WithMessage("La password deve contenere almeno una lettera maiuscola.")
            .Matches("[a-z]").WithMessage("La password deve contenere almeno una lettera minuscola.")
            .Matches("[0-9]").WithMessage("La password deve contenere almeno un numero.");
    }
}