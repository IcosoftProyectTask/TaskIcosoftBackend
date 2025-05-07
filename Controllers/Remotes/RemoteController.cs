using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Common;
using TaskIcosoftBackend.Dtos.RemoteDto;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Service;

namespace TaskIcosoftBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Remote")]
    public class RemoteController : ControllerBase
    {
        private readonly RemoteService _remoteService;
        private readonly ILogger<RemoteController> _logger;

        public RemoteController(RemoteService remoteService, ILogger<RemoteController> logger)
        {
            _remoteService = remoteService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRemote([FromBody] CreateRemoteDto remoteDto)
        {
            _logger.LogInformation("Creando un nuevo registro remoto.");
            try
            {
                var remote = remoteDto.ToModel();
                var result = await _remoteService.CreateRemote(remote);

                // Asegurarse de no devolver la contraseña encriptada en la respuesta
                var responseDto = result.ToDto();
                responseDto.Password = "********"; // Ocultar la contraseña en la respuesta

                _logger.LogInformation("Registro remoto creado exitosamente con ID {RemoteId}.", result.IdRemote);
                return Ok(ApiResponse<RemoteDto>.Ok(responseDto, "Registro remoto creado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el registro remoto.");
                return StatusCode(500, ApiResponse<string>.Error("Error al crear el registro remoto."));
            }
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreateRemotes([FromBody] List<CreateRemoteDto> remoteDtos)
        {
            _logger.LogInformation("Creando múltiples registros remotos ({Count}).", remoteDtos.Count);
            try
            {
                if (remoteDtos == null || !remoteDtos.Any())
                {
                    _logger.LogWarning("Se recibió una solicitud de importación masiva sin datos.");
                    return BadRequest(ApiResponse<string>.Error("No se proporcionaron datos para la importación."));
                }

                var results = new List<RemoteDto>();
                var errors = new List<string>();
                var successCount = 0;

                foreach (var remoteDto in remoteDtos)
                {
                    try
                    {
                        var remote = remoteDto.ToModel();
                        var result = await _remoteService.CreateRemote(remote);

                        // Ocultar la contraseña en la respuesta
                        var responseDto = result.ToDto();

                        results.Add(responseDto);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al crear registro remoto para el cliente {Customer}.", remoteDto.Customer);
                        errors.Add($"Error al crear registro para {remoteDto.Customer}: {ex.Message}");
                    }
                }

                if (errors.Any())
                {
                    _logger.LogWarning("Importación masiva completada con {SuccessCount} éxitos y {ErrorCount} errores.", successCount, errors.Count);
                    return Ok(ApiResponse<object>.Ok(
                        new { SuccessCount = successCount, ErrorCount = errors.Count, Errors = errors, Results = results },
                        $"Importación completada con {successCount} éxitos y {errors.Count} errores."
                    ));
                }

                _logger.LogInformation("Importación masiva completada correctamente. Se crearon {SuccessCount} registros.", successCount);
                return Ok(ApiResponse<object>.Ok(
                    new { SuccessCount = successCount, Results = results },
                    $"Se importaron {successCount} registros correctamente."
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la importación masiva de registros remotos.");
                return StatusCode(500, ApiResponse<string>.Error("Error al procesar la importación masiva de registros remotos."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRemoteById(int id)
        {
            _logger.LogInformation("Obteniendo registro remoto con ID {RemoteId}.", id);
            try
            {
                var remote = await _remoteService.GetRemoteById(id);
                if (remote == null)
                {
                    _logger.LogWarning("No se encontró el registro remoto con ID {RemoteId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el registro remoto."));
                }

                // Asegurarse de no devolver la contraseña encriptada en la respuesta
                var responseDto = remote.ToDto();

                return Ok(ApiResponse<RemoteDto>.Ok(responseDto, "Registro remoto encontrado."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro remoto con ID {RemoteId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener el registro remoto."));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRemotes()
        {
            _logger.LogInformation("Obteniendo lista de registros remotos.");
            try
            {
                var remotes = await _remoteService.GetRemotes();

                // Ocultar contraseñas en la lista de respuesta
                var responseDtos = remotes.Select(r =>
                {
                    var dto = r.ToDto();
                    return dto;
                });

                return Ok(ApiResponse<IEnumerable<RemoteDto>>.Ok(responseDtos, "Lista de registros remotos obtenida."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de registros remotos.");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la lista de registros remotos."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRemote(int id, [FromBody] UpdateRemoteDto remoteDto)
        {
            _logger.LogInformation("Actualizando registro remoto con ID {RemoteId}.", id);
            try
            {
                var existingRemote = await _remoteService.GetRemoteById(id);
                if (existingRemote == null)
                {
                    _logger.LogWarning("No se encontró el registro remoto con ID {RemoteId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el registro remoto."));
                }

                existingRemote.Customer = remoteDto.Customer;
                existingRemote.Terminal = remoteDto.Terminal;
                existingRemote.Software = remoteDto.Software;
                existingRemote.IpAddress = remoteDto.IpAddress;
                existingRemote.User = remoteDto.User;
                existingRemote.Password = remoteDto.Password; // La encriptación se maneja en el repositorio
                existingRemote.UpdatedAt = DateTime.UtcNow;

                var updatedRemote = await _remoteService.UpdateRemote(existingRemote);

                // Asegurarse de no devolver la contraseña encriptada en la respuesta
                var responseDto = updatedRemote.ToDto();

                _logger.LogInformation("Registro remoto con ID {RemoteId} actualizado exitosamente.", id);
                return Ok(ApiResponse<RemoteDto>.Ok(responseDto, "Registro remoto actualizado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro remoto con ID {RemoteId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar el registro remoto."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRemote(int id)
        {
            _logger.LogInformation("Eliminando registro remoto con ID {RemoteId}.", id);
            try
            {
                var result = await _remoteService.DeleteRemote(id);
                if (result)
                {
                    _logger.LogInformation("Registro remoto con ID {RemoteId} eliminado exitosamente.", id);
                    return Ok(ApiResponse<string>.Ok(null, "Registro remoto eliminado exitosamente."));
                }
                else
                {
                    _logger.LogWarning("No se encontró el registro remoto con ID {RemoteId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el registro remoto."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro remoto con ID {RemoteId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al eliminar el registro remoto."));
            }
        }
    }
}