using DeviceManagement.Api.DTOs;
using FluentValidation;

namespace DeviceManagement.Api.Validators
{
    /// <summary>
    /// Validator for the DeviceDTO class.
    /// </summary>
    public class DeviceDTOValidator : AbstractValidator<DeviceDTO>
    {
        public DeviceDTOValidator()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Device name is required.")
                .MaximumLength(255).WithMessage("Device name must not exceed 255 characters.");

            RuleFor(d => d.Manufacturer)
                .NotEmpty().WithMessage("Manufacturer is required.")
                .MaximumLength(255).WithMessage("Manufacturer must not exceed 255 characters.");

            RuleFor(d => d.Type)
                .NotEmpty().WithMessage("Device type is required.")
                .MaximumLength(255).WithMessage("Device type must not exceed 255 characters.");

            RuleFor(d => d.OperatingSystem)
                .NotEmpty().WithMessage("Operating system is required.")
                .MaximumLength(255).WithMessage("Operating system must not exceed 255 characters.");

            RuleFor(d => d.OsVersion)
                .NotEmpty().WithMessage("OS version is required.")
                .MaximumLength(255).WithMessage("OS version must not exceed 255 characters.");

            RuleFor(d => d.Processor)
                .NotEmpty().WithMessage("Processor is required.")
                .MaximumLength(255).WithMessage("Processor must not exceed 255 characters.");

            RuleFor(d => d.RamAmountGb)
                .GreaterThan(0).WithMessage("RAM amount must be greater than 0.");
        }
    }
}
