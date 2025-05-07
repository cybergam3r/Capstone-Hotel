using FluentValidation;
using HotelBackend.DTOs;

namespace HotelBackend.Validators;

public class LoginDTOValidator : AbstractValidator<LoginDTO>
{
    public LoginDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'email è obbligatoria.")
            .EmailAddress().WithMessage("Formato email non valido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La password è obbligatoria.");
    }
}