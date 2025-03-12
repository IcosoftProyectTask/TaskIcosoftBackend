using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Repository;
using TaskIcosoftBackend.Dtos.SupportTasks;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Dtos.PriorityDtos;

namespace TaskIcosoftBackend.Service
{
    public class SupportTaskService
    {
        private readonly SupportTaskRepository _supportTaskRepository;
        private readonly ILogger<SupportTaskService> _logger;

        public SupportTaskService(SupportTaskRepository supportTaskRepository, ILogger<SupportTaskService> logger)
        {
            _supportTaskRepository = supportTaskRepository;
            _logger = logger;
        }

        public async Task<SupportTasks> CreateSupportTask(SupportTasks supportTask)
        {
            _logger.LogInformation("Creando una nueva tarea de soporte.");
            try
            {
                var result = await _supportTaskRepository.CreateSupportTask(supportTask);
                _logger.LogInformation("Tarea de soporte creada exitosamente con ID {SupportTaskId}.", result.IdSupportTask);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la tarea de soporte.");
                throw;
            }
        }

        public async Task<SupportTasks> GetSupportTaskById(int id)
        {
            _logger.LogInformation("Obteniendo tarea de soporte con ID {SupportTaskId}.", id);
            try
            {
                var supportTask = await _supportTaskRepository.GetSupportTaskById(id);
                if (supportTask == null)
                {
                    _logger.LogWarning("No se encontró la tarea de soporte con ID {SupportTaskId}.", id);
                    return null;
                }

                return supportTask; // Devuelve un objeto de tipo SupportTasks
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarea de soporte con ID {SupportTaskId}.", id);
                throw;
            }
        }

        public async Task<List<SupportTaskDto>> GetSupportTasks()
        {
            _logger.LogInformation("Obteniendo lista de tareas de soporte.");
            try
            {
                var supportTasks = await _supportTaskRepository.GetSupportTasks();
                var supportTaskDtos = supportTasks.Select(st => st.ToDto()).ToList();

                _logger.LogInformation("Se obtuvieron {Count} tareas de soporte exitosamente.", supportTaskDtos.Count);
                return supportTaskDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de tareas de soporte.");
                throw;
            }
        }


        public async Task<SupportTasks> UpdateSupportTask(SupportTasks supportTask)
        {
            _logger.LogInformation("Actualizando tarea de soporte con ID {SupportTaskId}.", supportTask.IdSupportTask);
            try
            {
                var result = await _supportTaskRepository.UpdateSupportTask(supportTask);
                _logger.LogInformation("Tarea de soporte con ID {SupportTaskId} actualizada exitosamente.", supportTask.IdSupportTask);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la tarea de soporte con ID {SupportTaskId}.", supportTask.IdSupportTask);
                throw;
            }
        }

        public async Task<bool> DeleteSupportTask(int id)
        {
            _logger.LogInformation("Eliminando tarea de soporte con ID {SupportTaskId}.", id);
            try
            {
                var result = await _supportTaskRepository.DeleteSupportTask(id);
                if (result)
                {
                    _logger.LogInformation("Tarea de soporte con ID {SupportTaskId} eliminada exitosamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar la tarea de soporte con ID {SupportTaskId}.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la tarea de soporte con ID {SupportTaskId}.", id);
                throw;
            }
        }

        public async Task<bool> UpdateStatus(int id, UpdateStatusSupportTask updateStatusDto)
        {
            _logger.LogInformation("Actualizando estado de tarea de soporte con ID {SupportTaskId}.", id);
            try
            {
                var result = await _supportTaskRepository.UpdateStatus(id, updateStatusDto);
                if (result)
                {
                    _logger.LogInformation("Estado de la tarea de soporte con ID {SupportTaskId} actualizado exitosamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo actualizar el estado de la tarea de soporte con ID {SupportTaskId}.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el estado de la tarea de soporte con ID {SupportTaskId}.", id);
                throw;
            }
        }

        public async Task<bool> UpdateUserAsigment(int id, UpdateUserAsigmentDto updateUserAsigmentDto)
        {
            _logger.LogInformation("Actualizando asignación de usuario de tarea de soporte con ID {SupportTaskId}.", id);
            try
            {
                var result = await _supportTaskRepository.UpdateUserAsigment (id, updateUserAsigmentDto);
                if (result)
                {
                    _logger.LogInformation("Asignación de usuario de la tarea de soporte con ID {SupportTaskId} actualizada exitosamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo actualizar la asignación de usuario de la tarea de soporte con ID {SupportTaskId}.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la asignación de usuario de la tarea de soporte con ID {SupportTaskId}.", id);
                throw;
            }
        }

    }
}