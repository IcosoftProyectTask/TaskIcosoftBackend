using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskIcosoftBackend.Interface;
using TaskIcosoftBackend.Logic.validation.Results;

namespace TaskIcosoftBackend.Logic.validation.Base
{
    public abstract class BaseEntityValidator<TEntity, TContext> : IEntityValidator<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
    {
        protected readonly TContext Context;

        protected BaseEntityValidator(TContext context)
        {
            Context = context;
        }

        public abstract Task<ValidationResult> ValidateForCreateAsync(TEntity entity);
        public abstract Task<ValidationResult> ValidateForUpdateAsync(TEntity entity);

        protected ValidationResult CombineValidationResults(params ValidationResult[] results)
        {
            var errors = results.SelectMany(r => r.Errors).ToList();
            return errors.Any()
                ? ValidationResult.Failure(errors)
                : ValidationResult.Success();
        }

        Task<System.ComponentModel.DataAnnotations.ValidationResult> IEntityValidator<TEntity, TContext>.ValidateForCreateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        Task<System.ComponentModel.DataAnnotations.ValidationResult> IEntityValidator<TEntity, TContext>.ValidateForUpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
