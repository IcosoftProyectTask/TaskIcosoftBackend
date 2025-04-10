using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskIcosoftBackend.Common;
using TaskIcosoftBackend.Dtos.StatusTasks;
using TaskIcosoftBackend.Dtos.StatusTasksDto;
using TaskIcosoftBackend.Dtos.SupportTasks;
using TaskIcosoftBackend.Hubs;
using TaskIcosoftBackend.Service;

namespace TaskIcosoftBackend.Controllers.SupportTasks
{
    [Authorize]
    [ApiController]
    [Route("api/StatusTask")]
    public class StatusTaskController : ControllerBase
    {
        private readonly SupportTaskService _supportTaskService;
        private readonly IHubContext<TaskHub> _hubContext;
        private readonly StatusTaskService _statusTaskService;
        private readonly ILogger<StatusTaskController> _logger;
        
        public StatusTaskController(StatusTaskService statusTaskService, ILogger<StatusTaskController> logger,SupportTaskService supportTaskService,IHubContext<TaskHub> hubContext)
        {
             _supportTaskService = supportTaskService;
            _statusTaskService = statusTaskService;
            _hubContext = hubContext;
            _logger = logger;
        }
/*
         [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusDto updateDto)
        {
            _logger.LogInformation("Actualizando estado de la tarea con ID {TaskId} al estado {StatusId}.", id, updateDto.IdStatus);
            try
            {
                // Obtener la tarea actual
                var task = await _supportTaskService.GetSupportTaskById(id);
                if (task == null)
                {
                    _logger.LogWarning("No se encontró la tarea con ID {TaskId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la tarea."));
                }

                // Verificar si el estado está cambiando a "Completado" (asumiendo que el ID es 3)
                bool isCompletingTask = updateDto.IdStatus == 3;

                // Si cambia a completado y no se especificó una fecha de fin, establecer la fecha actual
                if (isCompletingTask && updateDto.EndTask == null)
                {
                    updateDto.EndTask = DateTime.UtcNow;
                }
                // Si cambia a un estado diferente a completado y no se especificó una fecha de fin, establecer como null
                else if (!isCompletingTask && updateDto.EndTask == null)
                {
                    // Aquí dejamos EndTask como null para que se actualice a null en la BD
                }
                
                // Actualizar el estado y posiblemente la fecha de finalización
                var updatedTask = await _supportTaskService.UpdateTaskStatus(id, updateDto);

                // Obtener datos del estado para notificar a los clientes
                var statusTask = await _statusTaskService.GetStatusTaskById(updateDto.IdStatus);

                // Notificar a los clientes a través de SignalR
                await _hubContext.Clients.All.SendAsync("TaskStatusChanged", id, statusTask);

                return Ok(ApiResponse<SupportTaskDto>.Ok(updatedTask, "Estado de la tarea actualizado."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el estado de la tarea.");
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar el estado de la tarea."));
            }
        }
*/


        [HttpPost]
        public async Task<IActionResult> CreateStatusTask([FromBody] CreateStatusTaskDto createStatusTask)
        {
            _logger.LogInformation("Creando un nuevo estado de tarea.");
            try
            {
                var statusTask = await _statusTaskService.CreateStatusTask(createStatusTask);
                _logger.LogInformation("Estado de tarea creado con ID {StatusTaskId}.", statusTask.IdStatus);
                return Ok(ApiResponse<StatusTaskDto>.Ok(statusTask, "Estado de tarea creado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el estado de la tarea.");
                return StatusCode(500, ApiResponse<string>.Error("Error al crear el estado de la tarea."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStatusTaskById(int id)
        {
            _logger.LogInformation("Obteniendo estado de tarea con ID {StatusTaskId}.", id);
            try
            {
                var statusTask = await _statusTaskService.GetStatusTaskById(id);
                if (statusTask == null)
                {
                    _logger.LogWarning("No se encontró el estado de la tarea con ID {StatusTaskId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el estado de la tarea."));
                }

                return Ok(ApiResponse<StatusTaskDto>.Ok(statusTask, "Estado de tarea encontrado."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el estado de la tarea.");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener el estado de la tarea."));
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetStatusTasks()
        {
            _logger.LogInformation("Obteniendo lista de estados de tarea.");
            try
            {
                var statusTasks = await _statusTaskService.GetAllStatusTasks();
                return Ok(ApiResponse<IEnumerable<StatusTaskDto>>.Ok(statusTasks, "Estados de tarea encontrados."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de estados de tarea.");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la lista de estados de tarea."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatusTask(int id)
        {
            _logger.LogInformation("Eliminando estado de tarea con ID {StatusTaskId}.", id);
            try
            {
                var isDeleted = await _statusTaskService.DeleteStatusTask(id);
                if (!isDeleted)
                {
                    _logger.LogWarning("No se encontró el estado de la tarea con ID {StatusTaskId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el estado de la tarea."));
                }

                return Ok(ApiResponse<string>.Ok("Estado de tarea eliminado."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el estado de la tarea.");
                return StatusCode(500, ApiResponse<string>.Error("Error al eliminar el estado de la tarea."));
            }
        }

    }
}