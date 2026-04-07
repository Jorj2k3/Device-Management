using DeviceManagement.Api.DTOs;
using FluentValidation;

namespace DeviceManagement.Api.Validators
{
    /// <summary>
    /// Validator for the UserDTO class.
    /// </summary>
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("User name is required.")
                .MaximumLength(255).WithMessage("User name must not exceed 255 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(u => u.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(255).WithMessage("Location must not exceed 255 characters.");
        }
    }
}
