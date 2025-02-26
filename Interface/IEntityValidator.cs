using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskIcosoftBackend.Interface
{
   public interface IEntityValidator<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    Task<ValidationResult> ValidateForCreateAsync(TEntity entity);
    Task<ValidationResult> ValidateForUpdateAsync(TEntity entity);
}
}