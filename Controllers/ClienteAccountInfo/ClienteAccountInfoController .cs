using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Common;
using TaskIcosoftBackend.Dtos.ClienteAccountInfoDto;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Service;

namespace TaskIcosoftBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/ClienteAccountInfo")]
    public class ClienteAccountInfoController : ControllerBase
    {
        private readonly ClienteAccountInfoService _clienteAccountInfoService;
        private readonly ILogger<ClienteAccountInfoController> _logger;

        public ClienteAccountInfoController(ClienteAccountInfoService clienteAccountInfoService, ILogger<ClienteAccountInfoController> logger)
        {
            _clienteAccountInfoService = clienteAccountInfoService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClienteAccountInfo([FromBody] CreateClienteAccountInfoDto clienteAccountInfoDto)
        {
            _logger.LogInformation("Creando una nueva información de cuenta de cliente.");
            try
            {
                var clienteAccountInfo = clienteAccountInfoDto.ToModel();
                var result = await _clienteAccountInfoService.CreateClienteAccountInfo(clienteAccountInfo);

                // Asegurarse de no devolver datos sensibles en la respuesta
                var responseDto = result.ToDto();

                _logger.LogInformation("Información de cuenta de cliente creada exitosamente con ID {ClienteAccountInfoId}.", result.IdClienteAccountInfo);
                return Ok(ApiResponse<ClienteAccountInfoDto>.Ok(responseDto, "Información de cuenta de cliente creada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la información de cuenta de cliente.");
                return StatusCode(500, ApiResponse<string>.Error("Error al crear la información de cuenta de cliente."));
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreateClienteAccountInfos([FromBody] List<CreateClienteAccountInfoDto> clienteAccountInfoDtos)
        {
            _logger.LogInformation("Iniciando importación masiva de {Count} registros.", clienteAccountInfoDtos?.Count ?? 0);

            try
            {
                if (clienteAccountInfoDtos == null || !clienteAccountInfoDtos.Any())
                {
                    _logger.LogWarning("Solicitud de importación masiva sin datos recibida.");
                    return BadRequest(ApiResponse<string>.Error("No se proporcionaron datos para la importación."));
                }

                _logger.LogDebug("Datos recibidos: {@Datos}", clienteAccountInfoDtos);

                var results = new List<ClienteAccountInfoDto>();
                var errors = new List<string>();
                var successCount = 0;

                foreach (var dto in clienteAccountInfoDtos)
                {
                    try
                    {
                        // Validación adicional
                        if (string.IsNullOrWhiteSpace(dto.Client))
                        {
                            throw new Exception("El nombre del cliente es requerido");
                        }

                        if (string.IsNullOrWhiteSpace(dto.Email))
                        {
                            throw new Exception("El email es requerido");
                        }

                        var model = dto.ToModel();
                        var result = await _clienteAccountInfoService.CreateClienteAccountInfo(model);

                        results.Add(result.ToDto());
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al crear registro para {Client}", dto.Client);
                        errors.Add($"Error en {dto.Client}: {ex.Message}");
                    }
                }

                var response = new
                {
                    SuccessCount = successCount,
                    ErrorCount = errors.Count,
                    Errors = errors,
                    Results = results
                };

                if (errors.Any())
                {
                    _logger.LogWarning("Importación completada con {SuccessCount} éxitos y {ErrorCount} errores", successCount, errors.Count);
                    return Ok(ApiResponse<object>.Ok(
                        response,
                        $"Importación completada con {successCount} éxitos y {errors.Count} errores."
                    ));
                }

                _logger.LogInformation("Importación masiva completada exitosamente: {SuccessCount} registros creados", successCount);
                return Ok(ApiResponse<object>.Ok(
                    response,
                    $"Se importaron {successCount} registros correctamente."
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en la importación masiva");
                return StatusCode(500, ApiResponse<string>.Error("Error interno al procesar la importación masiva"));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteAccountInfoById(int id)
        {
            _logger.LogInformation("Obteniendo información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
            try
            {
                var clienteAccountInfo = await _clienteAccountInfoService.GetClienteAccountInfoById(id);
                if (clienteAccountInfo == null)
                {
                    _logger.LogWarning("No se encontró la información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la información de cuenta de cliente."));
                }

                // Asegurarse de no devolver datos sensibles en la respuesta
                var responseDto = clienteAccountInfo.ToDto();

                return Ok(ApiResponse<ClienteAccountInfoDto>.Ok(responseDto, "Información de cuenta de cliente encontrada."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la información de cuenta de cliente."));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClienteAccountInfos()
        {
            _logger.LogInformation("Obteniendo lista de información de cuentas de clientes.");
            try
            {
                var clienteAccountInfos = await _clienteAccountInfoService.GetClienteAccountInfos();

                // Ocultar datos sensibles en la lista de respuesta
                var responseDtos = clienteAccountInfos.Select(c =>
                {
                    var dto = c.ToDto();
                    return dto;
                });

                return Ok(ApiResponse<IEnumerable<ClienteAccountInfoDto>>.Ok(responseDtos, "Lista de información de cuentas de clientes obtenida."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de información de cuentas de clientes.");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la lista de información de cuentas de clientes."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClienteAccountInfo(int id, [FromBody] UpdateClienteAccountInfoDto clienteAccountInfoDto)
        {
            _logger.LogInformation("Actualizando información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
            try
            {
                var existingClienteAccountInfo = await _clienteAccountInfoService.GetClienteAccountInfoById(id);
                if (existingClienteAccountInfo == null)
                {
                    _logger.LogWarning("No se encontró la información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la información de cuenta de cliente."));
                }

                existingClienteAccountInfo.Client = clienteAccountInfoDto.Client;
                existingClienteAccountInfo.Email = clienteAccountInfoDto.Email;
                existingClienteAccountInfo.Password = clienteAccountInfoDto.Password; // La encriptación se maneja en el repositorio
                existingClienteAccountInfo.AppPassword = clienteAccountInfoDto.AppPassword; // La encriptación se maneja en el repositorio
                existingClienteAccountInfo.Vin = clienteAccountInfoDto.Vin; // La encriptación se maneja en el repositorio
                existingClienteAccountInfo.Date1 = clienteAccountInfoDto.Date1;
                existingClienteAccountInfo.UpdatedAt = DateTime.UtcNow;

                var updatedClienteAccountInfo = await _clienteAccountInfoService.UpdateClienteAccountInfo(existingClienteAccountInfo);

                // Asegurarse de no devolver datos sensibles en la respuesta
                var responseDto = updatedClienteAccountInfo.ToDto();

                _logger.LogInformation("Información de cuenta de cliente con ID {ClienteAccountInfoId} actualizada exitosamente.", id);
                return Ok(ApiResponse<ClienteAccountInfoDto>.Ok(responseDto, "Información de cuenta de cliente actualizada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar la información de cuenta de cliente."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClienteAccountInfo(int id)
        {
            _logger.LogInformation("Eliminando información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
            try
            {
                var result = await _clienteAccountInfoService.DeleteClienteAccountInfo(id);
                if (result)
                {
                    _logger.LogInformation("Información de cuenta de cliente con ID {ClienteAccountInfoId} eliminada exitosamente.", id);
                    return Ok(ApiResponse<string>.Ok(null, "Información de cuenta de cliente eliminada exitosamente."));
                }
                else
                {
                    _logger.LogWarning("No se encontró la información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la información de cuenta de cliente."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al eliminar la información de cuenta de cliente."));
            }
        }
    }
}