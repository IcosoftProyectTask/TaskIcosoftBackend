using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.PriorityDtos;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Repository;

namespace TaskIcosoftBackend.Service
{
    public class PriorityService
    {
        private readonly PriorityRepository _priorityRepository;
        private readonly ILogger<PriorityService> _logger;


        public PriorityService(PriorityRepository priorityRepository, ILogger<PriorityService> logger)
        {
            _priorityRepository = priorityRepository;
            _logger = logger;
        }


        public async Task<PriorityDto> CreatePriority(CreatePriorityDto createPriority)
        {
            try
            {
                var priority = createPriority.ToModel();
                var priorityCreated = await _priorityRepository.CreatePriority(priority);
                return priorityCreated.ToDto();
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
                var priority = await _priorityRepository.GetPriorityById(id);
                if (priority == null)
                {
                    _logger.LogWarning("No se encontró la prioridad con ID {PriorityId}.", id);
                    return null;
                }

                return priority; // Devuelve Priority, no PriorityDto
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener la prioridad con ID {PriorityId}.", id);
                throw new ApplicationException($"Error al obtener la prioridad con ID {id}.", e);
            }
        }

        public async Task<List<PriorityDto>> GetPriorities()
        {
            try
            {
                var priorities = await _priorityRepository.GetPriorities();
                return priorities.Select(p => p.ToDto()).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener las prioridades.");
                throw new ApplicationException("Error al obtener las prioridades.", e);
            }
        }

        public async Task<Priority> UpdatePriority(Priority priority)
        {
            _logger.LogInformation("Actualizando la prioridad con ID {SupportTaskId}.", priority.IdPriority);
            try
            {
                var result = await _priorityRepository.UpdatePriority(priority);
                _logger.LogInformation("Prioridad {IdPriority} actualizada exitosamente.", priority.IdPriority);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la priorifad con ID {IdPriority}.", priority.IdPriority);
                throw;
            }
        }

        public async Task<bool> DeletePriority(int id)
        {
            try
            {
                var result = await _priorityRepository.DeletePriority(id);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al eliminar la prioridad con ID {PriorityId}.", id);
                throw new ApplicationException($"Error al eliminar la prioridad con ID {id}.", e);
            }
        }


    }
}