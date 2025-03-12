using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskIcosoftBackend.Data;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Repository
{
    public class PriorityRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<PriorityRepository> _logger;

        public PriorityRepository(DataContext context, ILogger<PriorityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Priority> CreatePriority(Priority priority)
        {
            try
            {
                await _context.Priorities.AddAsync(priority);
                await _context.SaveChangesAsync();
                return priority;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear la prioridad.");
                throw new ApplicationException("Error al crear la prioridad.", e);
            }
        }

        public async Task<Priority> GetPriorityById(int id)
        {
            try
            {
                var priority = await _context.Priorities.FindAsync(id);

                if (priority == null)
                {
                    _logger.LogWarning("No se encontró la prioridad con ID {PriorityId}.", id);
                    return null;
                }

                return priority;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener la prioridad con ID {PriorityId}.", id);
                throw new ApplicationException($"Error al obtener la prioridad con ID {id}.", e);
            }
        }

        public async Task<List<Priority>> GetPriorities()
        {
            try
            {
                return await _context.Priorities.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener las prioridades.");
                throw new ApplicationException("Error al obtener las prioridades.", e);
            }
        }
        public async Task<Priority> UpdatePriority(Priority priority)
        {
            try
            {
                _context.Priorities.Update(priority);
                await _context.SaveChangesAsync();
                return priority;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al actualizar la tarea de soporte con ID {priority.IdPriority}.");
                throw new ApplicationException($"Error al actualizar la tarea de soporte con ID {priority.IdPriority}.", e);
            }
        }



        public async Task<bool> DeletePriority(int id)
        {
            try
            {
                var priority = await _context.Priorities.FindAsync(id);
                if (priority == null)
                {
                    _logger.LogWarning($"No se encontró la prioridad con ID {id}.");
                    return false;
                }

                _context.Priorities.Remove(priority);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al eliminar la prioridad con ID {id}.");
                throw new ApplicationException($"Error al eliminar la prioridad con ID {id}.", e);
            }
        }
        

    }
}