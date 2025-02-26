using System;
using TaskIcosoftBackend.Logic.validation.Results;

namespace gymconnect_backend.Logic.Validation.Rules
{
    public static class NumberValidationRules
    {
        public static ValidationResult ValidatePositive(int value, string fieldName)
        {
            if (value <= 0)
            {
                return ValidationResult.Failure($"El campo {fieldName} debe ser un número positivo.");
            }
            return ValidationResult.Success();
        }

   
        public static ValidationResult ValidatePositive(decimal value, string fieldName)
        {
            if (value <= 0)
            {
                return ValidationResult.Failure($"El campo {fieldName} debe ser un número positivo.");
            }
            return ValidationResult.Success();
        }

    
        public static ValidationResult ValidateNonNegative(int value, string fieldName)
        {
            if (value < 0)
            {
                return ValidationResult.Failure($"El campo {fieldName} no puede ser negativo.");
            }
            return ValidationResult.Success();
        }

        
        public static ValidationResult ValidateNonNegative(decimal value, string fieldName)
        {
            if (value < 0)
            {
                return ValidationResult.Failure($"El campo {fieldName} no puede ser negativo.");
            }
            return ValidationResult.Success();
        }

                public static ValidationResult ValidateRange(int value, int min, int max, string fieldName)
        {
            if (value < min || value > max)
            {
                return ValidationResult.Failure(
                    $"El campo {fieldName} debe estar entre {min} y {max}.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateRange(decimal value, decimal min, decimal max, string fieldName)
        {
            if (value < min || value > max)
            {
                return ValidationResult.Failure(
                    $"El campo {fieldName} debe estar entre {min} y {max}.");
            }
            return ValidationResult.Success();
        }


        public static ValidationResult ValidateMaxValue(int value, int max, string fieldName)
        {
            if (value > max)
            {
                return ValidationResult.Failure($"El campo {fieldName} no puede ser mayor a {max}.");
            }
            return ValidationResult.Success();
        }

     
        public static ValidationResult ValidateMinValue(int value, int min, string fieldName)
        {
            if (value < min)
            {
                return ValidationResult.Failure($"El campo {fieldName} no puede ser menor a {min}.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateInRange(decimal value, string fieldName, decimal min, decimal max)
        {
            if (value < min || value > max)
            {
                return ValidationResult.Failure($"El campo {fieldName} debe estar entre {min} y {max}.");
            }

            return ValidationResult.Success();
        }
        
        
    }
}
