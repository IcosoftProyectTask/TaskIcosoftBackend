using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskIcosoftBackend.Logic.validation.Results;

namespace gymconnect_backend.Logic
{
    public static class TextValidationRules
    {
        public static ValidationResult ValidateRequired(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ValidationResult.Failure($"El campo {fieldName} es requerido.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateLength(string value, string fieldName, int minLength, int maxLength)
        {
            if (value != null && (value.Length < minLength || value.Length > maxLength))
            {
                return ValidationResult.Failure(
                    $"El campo {fieldName} debe tener entre {minLength} y {maxLength} caracteres.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateNoNumbers(string value, string fieldName)
        {
            if (value != null && value.Any(char.IsDigit))
            {
                return ValidationResult.Failure($"El campo {fieldName} no puede contener números.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateAlphabetic(string value, string fieldName, bool allowSpaces = true)
        {
            var pattern = allowSpaces ? @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-]+$" : @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\-]+$";
            if (value != null && !Regex.IsMatch(value, pattern))
            {
                return ValidationResult.Failure(
                    $"El campo {fieldName} solo puede contener letras{(allowSpaces ? ", espacios" : "")} y guiones.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateUrlFormat(string value, string fieldName)
        {
            if (value != null && !Uri.IsWellFormedUriString(value, UriKind.Absolute))
            {
                return ValidationResult.Failure($"El campo {fieldName} debe ser una URL válida.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateNonEmptyString(string value, string fieldName)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Failure($"El campo {fieldName} no puede estar vacío.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateNoSpecialCharacters(string value, string fieldName)
        {
            var pattern = @"^[a-zA-Z0-9\s]+$";
            if (value != null && !Regex.IsMatch(value, pattern))
            {
                return ValidationResult.Failure($"El campo {fieldName} no puede contener caracteres especiales.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateEmailFormat(string value, string fieldName)
        {
            var pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            if (value != null && !Regex.IsMatch(value, pattern))
            {
                return ValidationResult.Failure($"El campo {fieldName} debe ser una dirección de correo válida.");
            }
            return ValidationResult.Success();
        }

        public static async Task<ValidationResult> ValidateUnique<TEntity, TContext>(
            string value,
            string fieldName,
            Expression<Func<TEntity, bool>> uniqueCondition,
            TContext context) 
            where TEntity : class
            where TContext : DbContext
        {
            var exists = await context.Set<TEntity>().AnyAsync(uniqueCondition);
            if (exists)
            {
                return ValidationResult.Failure($"Ya existe un registro con este {fieldName}.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateNoLeadingOrTrailingSpaces(string value, string fieldName)
        {
            if (value != null && value != value.Trim())
            {
                return ValidationResult.Failure($"El campo {fieldName} no debe contener espacios al inicio o al final.");
            }
            return ValidationResult.Success();
        }

        public static ValidationResult ValidateNoConsecutiveSpaces(string value, string fieldName)
        {
            if (value != null && value.Contains("  "))
            {
                return ValidationResult.Failure($"El campo {fieldName} no debe contener múltiples espacios consecutivos.");
            }
            return ValidationResult.Success();
        }

        
    }
}
