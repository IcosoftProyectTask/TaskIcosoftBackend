using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Logic.validation.Results
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public List<string> Errors { get; private set; } = new List<string>();

        public static ValidationResult Success()
        {
            return new ValidationResult { IsValid = true };
        }

        public static ValidationResult Failure(string error)
        {
            var result = new ValidationResult { IsValid = false };
            result.Errors.Add(error);
            return result;
        }

        public static ValidationResult Failure(IEnumerable<string> errors)
        {
            var result = new ValidationResult { IsValid = false };
            result.Errors.AddRange(errors);
            return result;
        }
    }
}