using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Common;
using TaskIcosoftBackend.Dtos.PriorityDtos;
using TaskIcosoftBackend.Dtos.SupportTasks;
using TaskIcosoftBackend.Hubs;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Service;

namespace TaskIcosoftBackend.Controllers.SupportTasks
{
    [Authorize]
    [ApiController]
    [Route("api/SupportTask")]
    public class SupportTaskController : ControllerBase
    {
        private readonly SupportTaskService _supportTaskService;
        private readonly ILogger<SupportTaskController> _logger;
        private readonly IHubContext<TaskHub> _hubContext;

        public SupportTaskController(SupportTaskService supportTaskService, ILogger<SupportTaskController> logger,IHubContext<TaskHub> hubContext)
        {
            _supportTaskService = supportTaskService;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupportTask([FromBody] CreateSupportTask supportTaskDto)
        {
            _logger.LogInformation("Creando una nueva tarea de soporte.");
            try
            {
                var supportTask = supportTaskDto.ToModel();
                var result = await _supportTaskService.CreateSupportTask(supportTask);

                _logger.LogInformation("Tarea de soporte creada con ID {SupportTaskId}.", result.IdSupportTask);
                return Ok(ApiResponse<SupportTaskDto>.Ok(result.ToDto(), "Tarea de soporte creada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la tarea de soporte.");
                return StatusCode(500, ApiResponse<string>.Error("Error al crear la tarea de soporte."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupportTaskById(int id)
        {
            _logger.LogInformation("Obteniendo tarea de soporte con ID {SupportTaskId}.", id);
            try
            {
                var supportTask = await _supportTaskService.GetSupportTaskById(id);
                if (supportTask == null)
                {
                    _logger.LogWarning("No se encontró la tarea de soporte con ID {SupportTaskId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la tarea de soporte."));
                }

                return Ok(ApiResponse<SupportTaskDto>.Ok(supportTask.ToDto(), "Tarea de soporte encontrada."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarea de soporte con ID {SupportTaskId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la tarea de soporte."));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSupportTasks()
        {
            _logger.LogInformation("Obteniendo lista de tareas de soporte.");
            try
            {
                var supportTasks = await _supportTaskService.GetSupportTasks();
                return Ok(ApiResponse<IEnumerable<SupportTaskDto>>.Ok(supportTasks, "Lista de tareas de soporte obtenida."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de tareas de soporte.");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la lista de tareas de soporte."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupportTask(int id, [FromBody] UpdateSupportTask updateSupportTaskDto)
        {
            _logger.LogInformation("Actualizando tarea de soporte con ID {SupportTaskId}.", id);
            try
            {
                // Obtener la tarea de soporte existente (de tipo SupportTasks)
                var existingTask = await _supportTaskService.GetSupportTaskById(id);
                if (existingTask == null)
                {
                    _logger.LogWarning("No se encontró la tarea de soporte con ID {SupportTaskId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la tarea de soporte."));
                }

                // Actualizar los campos de la tarea existente con los valores del DTO
                existingTask.Title = updateSupportTaskDto.Title;
                existingTask.Description = updateSupportTaskDto.Description;
                existingTask.Category = updateSupportTaskDto.Category;
                existingTask.IdUser = updateSupportTaskDto.IdUser;
                existingTask.IdCompany = updateSupportTaskDto.IdCompany;
                existingTask.IdCompanyEmployee = updateSupportTaskDto.IdCompanyEmployee;
                existingTask.IdPriority = updateSupportTaskDto.IdPriority;
                existingTask.IdStatus = updateSupportTaskDto.IdStatus;
                existingTask.Solution = updateSupportTaskDto.Solution;
                existingTask.StartTask = updateSupportTaskDto.StartTask;
                existingTask.EndTask = updateSupportTaskDto.EndTask;
                existingTask.UpdatedAt = DateTime.UtcNow;

                // Llamar al servicio para actualizar la tarea (de tipo SupportTasks)
                var updatedTask = await _supportTaskService.UpdateSupportTask(existingTask);
                _logger.LogInformation("Tarea de soporte con ID {SupportTaskId} actualizada exitosamente.", id);

                // Convertir el resultado a DTO antes de devolverlo
                var updatedTaskDto = updatedTask.ToDto();
                return Ok(ApiResponse<SupportTaskDto>.Ok(updatedTaskDto, "Tarea de soporte actualizada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la tarea de soporte con ID {SupportTaskId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar la tarea de soporte."));
            }
        }

        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateStatusSupportTask updateStatusDto)
        {
            _logger.LogInformation("Actualizando el estado de la tarea de soporte con ID {SupportTaskId}.", id);
            _logger.LogInformation("El estado de la tarea de soporte es {StatusTaskId}.", updateStatusDto.IdStatus);
            try
            {
                var result = await _supportTaskService.UpdateStatus(id, updateStatusDto);
                if (result)
                {
                    await _hubContext.Clients.All.SendAsync("TaskUpdated", id);
                    _logger.LogInformation("Estado de la tarea de soporte con ID {SupportTaskId} actualizado exitosamente.", id);
                    return Ok(ApiResponse<string>.Ok(null, "Estado de la tarea de soporte actualizado exitosamente."));
                }

                _logger.LogWarning("No se encontró la tarea de soporte con ID {SupportTaskId}.", id);
                return NotFound(ApiResponse<string>.Error("No se encontró la tarea de soporte."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el estado de la tarea de soporte con ID {SupportTaskId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar el estado de la tarea de soporte."));
            }
        }

        [HttpPut("update-user-asigment/{id}")]

        public async Task<IActionResult> UpdateUserAsigment(int id, [FromBody] UpdateUserAsigmentDto updateUserAsigmentDto)
        {
            _logger.LogInformation("Actualizando el usuario asignado a la tarea de soporte con ID {SupportTaskId}.", id);
            _logger.LogInformation("El usuario asignado a la tarea de soporte es {UserId}.", updateUserAsigmentDto.IdUser);
            try
            {
                var result = await _supportTaskService.UpdateUserAsigment(id, updateUserAsigmentDto);
                if (result)
                {
                     await _hubContext.Clients.All.SendAsync("TaskReassigned", id);
                    _logger.LogInformation("Usuario asignado a la tarea de soporte con ID {SupportTaskId} actualizado exitosamente.", id);
                    return Ok(ApiResponse<string>.Ok(null, "Usuario asignado a la tarea de soporte actualizado exitosamente."));
                }

                _logger.LogWarning("No se encontró la tarea de soporte con ID {SupportTaskId}.", id);
                return NotFound(ApiResponse<string>.Error("No se encontró la tarea de soporte."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario asignado a la tarea de soporte con ID {SupportTaskId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar el usuario asignado a la tarea de soporte."));
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupportTask(int id)
        {
            _logger.LogInformation("Eliminando tarea de soporte con ID {SupportTaskId}.", id);
            try
            {
                var result = await _supportTaskService.DeleteSupportTask(id);
                if (result)
                {
                    _logger.LogInformation("Tarea de soporte con ID {SupportTaskId} eliminada exitosamente.", id);
                    return Ok(ApiResponse<string>.Ok(null, "Tarea de soporte eliminada exitosamente."));
                }

                _logger.LogWarning("No se encontró la tarea de soporte con ID {SupportTaskId}.", id);
                return NotFound(ApiResponse<string>.Error("No se encontró la tarea de soporte."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la tarea de soporte con ID {SupportTaskId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al eliminar la tarea de soporte."));
            }
        }
    }
}
