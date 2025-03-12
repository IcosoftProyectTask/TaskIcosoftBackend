using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskIcosoftBackend.Common;
using TaskIcosoftBackend.Dtos.PriorityDtos;
using TaskIcosoftBackend.Service;
using TaskIcosoftBackend.Mappers;

namespace TaskIcosoftBackend.Controllers.SupportTasks
{
    [Authorize]
    [ApiController]
    [Route("api/Priority")]
    public class PriorityController : ControllerBase
    {
        private readonly PriorityService _priorityService;
        private readonly ILogger<PriorityController> _logger;

        public PriorityController(PriorityService priorityService, ILogger<PriorityController> logger)
        {
            _priorityService = priorityService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePriority([FromBody] CreatePriorityDto createPriority)
        {
            _logger.LogInformation("Creando una nueva prioridad.");
            try
            {
                var priority = await _priorityService.CreatePriority(createPriority);
                _logger.LogInformation("Prioridad creada con ID {PriorityId}.", priority.IdPriority);
                return Ok(ApiResponse<PriorityDto>.Ok(priority, "Prioridad creada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la prioridad.");
                return StatusCode(500, ApiResponse<string>.Error("Error al crear la prioridad."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPriorityById(int id)
        {
            _logger.LogInformation("Obteniendo prioridad con ID {PriorityId}.", id);
            try
            {
                var priority = await _priorityService.GetPriorityById(id);
                if (priority == null)
                {
                    _logger.LogWarning("No se encontró la prioridad con ID {PriorityId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la prioridad."));
                }

                return Ok(ApiResponse<PriorityDto>.Ok(priority.ToDto(), "Prioridad encontrada."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la prioridad.");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la prioridad."));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPriorities()
        {
            _logger.LogInformation("Obteniendo lista de prioridades.");
            try
            {
                var priorities = await _priorityService.GetPriorities();
                return Ok(ApiResponse<List<PriorityDto>>.Ok(priorities, "Lista de prioridades obtenida exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de prioridades.");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la lista de prioridades."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePriority(int id, [FromBody] PriorityDto priorityDto)
        {
            _logger.LogInformation("Actualizando prioridad con ID {PriorityId}.", id);
            try
            {
                // Obtener la prioridad existente (de tipo Priority)
                var existingPriority = await _priorityService.GetPriorityById(id);
                if (existingPriority == null)
                {
                    _logger.LogWarning("No se encontró la prioridad con ID {PriorityId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la prioridad."));
                }

                // Actualizar los campos de la prioridad existente con los valores del DTO
                existingPriority.Name = priorityDto.Name;
                existingPriority.Status = priorityDto.Status;
                existingPriority.UpdatedAt = DateTime.UtcNow;

                // Llamar al servicio para actualizar la prioridad (de tipo Priority)
                var updatedPriority = await _priorityService.UpdatePriority(existingPriority);
                _logger.LogInformation("Prioridad con ID {PriorityId} actualizada exitosamente.", id);

                // Convertir el resultado a DTO antes de devolverlo
                var updatedPriorityDto = updatedPriority.ToDto();
                return Ok(ApiResponse<PriorityDto>.Ok(updatedPriorityDto, "Prioridad actualizada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la prioridad con ID {PriorityId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar la prioridad."));
            }
        }




    }
}