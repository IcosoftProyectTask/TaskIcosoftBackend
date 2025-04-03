using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskIcosoftBackend.Data;
using TaskIcosoftBackend.Dtos.PriorityDtos;
using TaskIcosoftBackend.Dtos.SupportTasks;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Repository
{
    public class SupportTaskRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<SupportTaskRepository> _logger;

        public SupportTaskRepository(DataContext context, ILogger<SupportTaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SupportTasks> CreateSupportTask(SupportTasks supportTask)
        {
            try
            {
                await _context.SupportTasks.AddAsync(supportTask);
                await _context.SaveChangesAsync();
                return supportTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear la tarea de soporte.");
                throw new ApplicationException("Error al crear la tarea de soporte.", e);
            }
        }

        public async Task<SupportTasks> GetSupportTaskById(int id)
        {
            try
            {
                var supportTask = await _context.SupportTasks
                    .Include(st => st.User) // Incluir la información del usuario
                    .Include(st => st.Company) // Incluir la información de la empresa
                         // Incluir la información del empleado de la empresa
                    .Include(st => st.Priority) // Incluir la información de la prioridad
                    .Include(st => st.StatusTask) // Incluir la información del estado de la tarea
                    .Where(st => st.Status)
                    .FirstOrDefaultAsync(st => st.IdSupportTask == id);

                if (supportTask == null)
                {
                    _logger.LogWarning("No se encontró la tarea de soporte con ID {SupportTaskId}.", id);
                    return null;
                }

                return supportTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener la tarea de soporte con ID {SupportTaskId}.", id);
                throw new ApplicationException($"Error al obtener la tarea de soporte con ID {id}.", e);
            }
        }

        public async Task<List<SupportTasks>> GetSupportTasks()
        {
            try
            {
                var supportTasks = await _context.SupportTasks
                    .Include(st => st.User) // Incluir la información del usuario
                    .Include(st => st.Company) // Incluir la información de la empresa
                    // Incluir la información del empleado de la empresa
                    .Include(st => st.Priority) // Incluir la información de la prioridad
                    .Include(st => st.StatusTask) // Incluir la información del estado de la tarea
                    .Where(st => st.Status) // Filtrar solo las tareas activas
                    .ToListAsync();

                if (!supportTasks.Any())
                {
                    _logger.LogWarning("No se encontraron tareas de soporte.");
                }

                return supportTasks;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener la lista de tareas de soporte.");
                throw new ApplicationException("Error al obtener la lista de tareas de soporte.", e);
            }
        }


        public async Task<SupportTasks> UpdateSupportTask(SupportTasks supportTask)
        {
            try
            {
                _context.SupportTasks.Update(supportTask);
                await _context.SaveChangesAsync();
                return supportTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al actualizar la tarea de soporte con ID {supportTask.IdSupportTask}.");
                throw new ApplicationException($"Error al actualizar la tarea de soporte con ID {supportTask.IdSupportTask}.", e);
            }
        }


        public async Task<bool> DeleteSupportTask(int id)
        {
            try
            {
                var supportTask = await _context.SupportTasks.FindAsync(id);
                if (supportTask == null)
                {
                    _logger.LogWarning($"No se encontró la tarea de soporte con ID {id}.");
                    return false;
                }

                supportTask.Status = false;
                _context.SupportTasks.Update(supportTask);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al eliminar la tarea de soporte con ID {id}.");
                throw new ApplicationException($"Error al eliminar la tarea de soporte con ID {id}.", e);
            }
        }

        public async Task<bool> UpdateStatus(int id, UpdateStatusSupportTask dto)
        {
            try
            {
                var supportTask = await _context.SupportTasks.FindAsync(id);
                if (supportTask == null)
                {
                    _logger.LogWarning($"No se encontró la tarea de soporte con ID {id}.");
                    return false;
                }

                supportTask.UpdateStatus(dto); // Usa el método del mapper
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al actualizar el estado de la tarea de soporte con ID {id}.");
                throw new ApplicationException($"Error al actualizar el estado de la tarea de soporte con ID {id}.", e);
            }
        }

        public async Task<bool> UpdateUserAsigment(int id, UpdateUserAsigmentDto dto)
        {
            try
            {
                var supportTask = await _context.SupportTasks.FindAsync(id);
                if (supportTask == null)
                {
                    _logger.LogWarning($"No se encontró la tarea de soporte con ID {id}.");
                    return false;
                }

                supportTask.UpdateUserAsigment(dto); // Usa el método del mapper
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al actualizar el usuario asignado a la tarea de soporte con ID {id}.");
                throw new ApplicationException($"Error al actualizar el usuario asignado a la tarea de soporte con ID {id}.", e);
            }
        }



    }
}