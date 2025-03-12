using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskIcosoftBackend.Common;
using TaskIcosoftBackend.Dtos.StatusTasks;
using TaskIcosoftBackend.Dtos.StatusTasksDto;
using TaskIcosoftBackend.Service;

namespace TaskIcosoftBackend.Controllers.SupportTasks
{
    [Authorize]
    [ApiController]
    [Route("api/StatusTask")]
    public class StatusTaskController : ControllerBase
    {
        private readonly StatusTaskService _statusTaskService;
        private readonly ILogger<StatusTaskController> _logger;
        
        public StatusTaskController(StatusTaskService statusTaskService, ILogger<StatusTaskController> logger)
        {
            _statusTaskService = statusTaskService;
            _logger = logger;
        }


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