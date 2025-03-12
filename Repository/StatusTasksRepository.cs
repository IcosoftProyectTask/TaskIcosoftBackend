using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskIcosoftBackend.Data;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Repository
{
    public class StatusTasksRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<StatusTasksRepository> _logger;


        public StatusTasksRepository(DataContext context, ILogger<StatusTasksRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<StatusTask> CreateStatusTask(StatusTask statusTask)
        {
            try
            {
                await _context.StatusTasks.AddAsync(statusTask);
                await _context.SaveChangesAsync();
                return statusTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear el estado de la tarea.");
                throw new ApplicationException("Error al crear el estado de la tarea.", e);
            }
        }

        public async Task<StatusTask> GetStatusTaskById(int id)
        {
            try
            {
                var statusTask = await _context.StatusTasks
                    .FirstOrDefaultAsync(st => st.IdStatus == id);

                if (statusTask == null)
                {
                    _logger.LogWarning("No se encontró el estado de la tarea con ID {StatusTaskId}.", id);
                    return null;
                }

                return statusTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener el estado de la tarea con ID {StatusTaskId}.", id);
                throw new ApplicationException($"Error al obtener el estado de la tarea con ID {id}.", e);
            }
        }

        public async Task<List<StatusTask>> GetStatusTasks()
        {
            try
            {
                return await _context.StatusTasks.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener los estados de las tareas.");
                throw new ApplicationException("Error al obtener los estados de las tareas.", e);
            }
        }

        public async Task<StatusTask> UpdateStatusTask(StatusTask statusTask)
        {
            try
            {
                _context.StatusTasks.Update(statusTask);
                await _context.SaveChangesAsync();
                return statusTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al actualizar el estado de la tarea con ID {StatusTaskId}.", statusTask.IdStatus);
                throw new ApplicationException($"Error al actualizar el estado de la tarea con ID {statusTask.IdStatus}.", e);
            }
        }

       
        public async Task<bool> DeleteStatusTask(int id)
        {
            try
            {
                var statusTask = await _context.StatusTasks.FindAsync(id);
                if (statusTask == null)
                {
                    _logger.LogWarning($"No se encontró el estado de la tarea con ID {id}.");
                    return false;
                }

                statusTask.Status = false;
                _context.StatusTasks.Update(statusTask);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al eliminar el estado de la tarea con ID {id}.");
                throw new ApplicationException($"Error al eliminar el estado de la tarea con ID {id}.", e);
            }

        }

        }
    }