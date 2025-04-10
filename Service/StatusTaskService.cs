using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.StatusTasks;
using TaskIcosoftBackend.Dtos.StatusTasksDto;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Repository;

namespace TaskIcosoftBackend.Service
{
    public class StatusTaskService
    {
        private readonly StatusTasksRepository _statusTasksRepository;
        private readonly ILogger<StatusTaskService> _logger;

        public StatusTaskService(StatusTasksRepository statusTasksRepository, ILogger<StatusTaskService> logger)
        {
            _statusTasksRepository = statusTasksRepository;
            _logger = logger;
        }

        public async Task<StatusTaskDto> CreateStatusTask(CreateStatusTaskDto createStatusTaskDto)
        {
            try
            {
                var statusTask = createStatusTaskDto.statusTask();
                statusTask = await _statusTasksRepository.CreateStatusTask(statusTask);
                return statusTask.statusTaskDto();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear el estado de la tarea.");
                throw new ApplicationException("Error al crear el estado de la tarea.", e);
            }
        }

        public async Task<StatusTaskDto> GetStatusTaskById(int id)
        {
            try
            {
                var statusTask = await _statusTasksRepository.GetStatusTaskById(id);

                if (statusTask == null)
                {
                    _logger.LogWarning("No se encontró el estado de la tarea con ID {StatusTaskId}.", id);
                    return null;
                }

                return statusTask.statusTaskDto();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener el estado de la tarea con ID {StatusTaskId}.", id);
                throw new ApplicationException($"Error al obtener el estado de la tarea con ID {id}.", e);
            }
        }


        public async Task<IEnumerable<StatusTaskDto>> GetAllStatusTasks()
        {
            try
            {
                var statusTasks = await _statusTasksRepository.GetStatusTasks();
                return statusTasks.Select(st => st.statusTaskDto());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener los estados de las tareas.");
                throw new ApplicationException("Error al obtener los estados de las tareas.", e);
            }
        }

        public async Task<StatusTaskDto> UpdateStatusTask(int id, CreateStatusTaskDto createStatusTaskDto)
        {
            try
            {
                var statusTask = await _statusTasksRepository.GetStatusTaskById(id);

                if (statusTask == null)
                {
                    _logger.LogWarning("No se encontró el estado de la tarea con ID {StatusTaskId}.", id);
                    return null;
                }

                statusTask.Name = createStatusTaskDto.Name;
                statusTask.UpdatedAt = DateTime.UtcNow;

                statusTask = await _statusTasksRepository.UpdateStatusTask(statusTask);
                return statusTask.statusTaskDto();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al actualizar el estado de la tarea con ID {StatusTaskId}.", id);
                throw new ApplicationException($"Error al actualizar el estado de la tarea con ID {id}.", e);
            }
        }

        public async Task<bool> DeleteStatusTask(int id)
        {
            try
            {
                var statusTask = await _statusTasksRepository.GetStatusTaskById(id);

                if (statusTask == null)
                {
                    _logger.LogWarning("No se encontró el estado de la tarea con ID {StatusTaskId}.", id);
                    return false;
                }

                statusTask.Status = false;
                statusTask.UpdatedAt = DateTime.UtcNow;

                statusTask = await _statusTasksRepository.UpdateStatusTask(statusTask);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al eliminar el estado de la tarea con ID {StatusTaskId}.", id);
                throw new ApplicationException($"Error al eliminar el estado de la tarea con ID {id}.", e);
            }
        }   

        

        
    }
}