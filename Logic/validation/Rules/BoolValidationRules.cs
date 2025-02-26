using TaskIcosoftBackend.Logic.validation.Results;
using TaskIcosoftBackend.Logic.Validation;
namespace TaskIcosoftBackend.Logic.Validation.Rules
{
    public static class BoolValidationRules
    {
        public static ValidationResult ValidateRequired(bool value, string fieldName)
        {
            if (!value)
            {
                return ValidationResult.Failure($"El campo {fieldName} es obligatorio y debe ser verdadero.");
            }

            return ValidationResult.Success();
        }
    }
}
