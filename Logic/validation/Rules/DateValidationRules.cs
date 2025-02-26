using System;
using gymconnect_backend.Logic.Validation;
using TaskIcosoftBackend.Logic.validation.Results;

namespace gymconnect_backend.Logic.Validation.Rules
{
    public static class DateValidationRules
    {
        public static ValidationResult ValidateInRange(DateTime date, string fieldName, DateTime min, DateTime max)
        {
            if (date < min || date > max)
            {
                return ValidationResult.Failure($"El campo {fieldName} debe estar entre {min:dd/MM/yyyy} y {max:dd/MM/yyyy}.");
            }

            return ValidationResult.Success();
        }

        public static ValidationResult ValidateRequired(DateTime? date, string fieldName)
        {
            if (!date.HasValue)
            {
                return ValidationResult.Failure($"El campo {fieldName} es obligatorio.");
            }

            return ValidationResult.Success();
        }

        public static ValidationResult ValidateNotFutureDate(DateTime date, string fieldName)
        {
            if (date > DateTime.Now)
            {
                return ValidationResult.Failure($"El campo {fieldName} no puede ser una fecha futura.");
            }

            return ValidationResult.Success();
        }
    }
}
