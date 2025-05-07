using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Common;
using TaskIcosoftBackend.Dtos.LicenseDto;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Service;

namespace TaskIcosoftBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/License")]
    public class LicenseController : ControllerBase
    {
        private readonly LicenseService _licenseService;
        private readonly ILogger<LicenseController> _logger;

        public LicenseController(LicenseService licenseService, ILogger<LicenseController> logger)
        {
            _licenseService = licenseService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLicense([FromBody] CreateLicenseDto licenseDto)
        {
            _logger.LogInformation("Creando una nueva licencia.");
            try
            {
                var license = licenseDto.ToModel();
                var result = await _licenseService.CreateLicense(license);
                
                // Asegurarse de no devolver el número de licencia encriptado en la respuesta
                var responseDto = result.ToDto();
                
                _logger.LogInformation("Licencia creada exitosamente con ID {LicenseId}.", result.IdLicense);
                return Ok(ApiResponse<LicenseDto>.Ok(responseDto, "Licencia creada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la licencia.");
                return StatusCode(500, ApiResponse<string>.Error("Error al crear la licencia."));
            }
        }

    [HttpPost("bulk")]
public async Task<IActionResult> BulkCreateLicenses([FromBody] List<CreateLicenseDto> licenseDtos)
{
    _logger.LogInformation("Creando múltiples licencias ({Count}).", licenseDtos.Count);
    try
    {
        if (licenseDtos == null || !licenseDtos.Any())
        {
            _logger.LogWarning("Se recibió una solicitud de importación masiva sin datos.");
            return BadRequest(ApiResponse<string>.Error("No se proporcionaron datos para la importación."));
        }

        var results = new List<LicenseDto>();
        var errors = new List<string>();
        var successCount = 0;

        foreach (var licenseDto in licenseDtos)
        {
            try
            {
                var license = licenseDto.ToModel();
                var result = await _licenseService.CreateLicense(license);

                var responseDto = result.ToDto();
                results.Add(responseDto);
                successCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la licencia para el cliente {Client}.", licenseDto.Client);
                errors.Add($"Error al crear licencia para {licenseDto.Client}: {ex.Message}");
            }
        }

        if (successCount == 0)
        {
            _logger.LogError("Fallaron todas las licencias. No se pudo importar ninguna.");
            return BadRequest(ApiResponse<string>.Error("No se pudo importar ninguna licencia."));
        }

        if (errors.Any())
        {
            _logger.LogWarning("Importación masiva completada con {SuccessCount} éxitos y {ErrorCount} errores.", successCount, errors.Count);
            return Ok(ApiResponse<object>.Ok(
                new { SuccessCount = successCount, ErrorCount = errors.Count, Errors = errors, Results = results },
                $"Importación completada parcialmente: {successCount} éxitos y {errors.Count} errores."
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
        _logger.LogError(ex, "Error en la importación masiva de licencias.");
        return StatusCode(500, ApiResponse<string>.Error("Error al procesar la importación masiva de licencias."));
    }
}




     [HttpGet("{id}")]
        public async Task<IActionResult> GetLicenseById(int id)
        {
            _logger.LogInformation("Obteniendo licencia con ID {LicenseId}.", id);
            try
            {
                var license = await _licenseService.GetLicenseById(id);
                if (license == null)
                {
                    _logger.LogWarning("No se encontró la licencia con ID {LicenseId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la licencia."));
                }
                
                // Asegurarse de no devolver el número de licencia encriptado en la respuesta
                var responseDto = license.ToDto();
              
                
                return Ok(ApiResponse<LicenseDto>.Ok(responseDto, "Licencia encontrada."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la licencia con ID {LicenseId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la licencia."));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLicenses()
        {
            _logger.LogInformation("Obteniendo lista de licencias.");
            try
            {
                var licenses = await _licenseService.GetLicenses();
                
                // Ocultar números de licencia en la lista de respuesta
                var responseDtos = licenses.Select(l => {
                    var dto = l.ToDto();
                    return dto;
                });
                
                return Ok(ApiResponse<IEnumerable<LicenseDto>>.Ok(responseDtos, "Lista de licencias obtenida."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de licencias.");
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la lista de licencias."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicense(int id, [FromBody] UpdateLicenseDto licenseDto)
        {
            _logger.LogInformation("Actualizando licencia con ID {LicenseId}.", id);
            try
            {
                var existingLicense = await _licenseService.GetLicenseById(id);
                if (existingLicense == null)
                {
                    _logger.LogWarning("No se encontró la licencia con ID {LicenseId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la licencia."));
                }

                existingLicense.Client = licenseDto.Client;
                existingLicense.DeviceName = licenseDto.DeviceName;
                existingLicense.LicenseNumber = licenseDto.LicenseNumber; // La encriptación se maneja en el repositorio
                existingLicense.Type = licenseDto.Type;
                existingLicense.InstallationDate = licenseDto.InstallationDate;
                existingLicense.UpdatedAt = DateTime.UtcNow;

                var updatedLicense = await _licenseService.UpdateLicense(existingLicense);
                
                // Asegurarse de no devolver el número de licencia encriptado en la respuesta
                var responseDto = updatedLicense.ToDto();
               
                
                _logger.LogInformation("Licencia con ID {LicenseId} actualizada exitosamente.", id);
                return Ok(ApiResponse<LicenseDto>.Ok(responseDto, "Licencia actualizada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la licencia con ID {LicenseId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar la licencia."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicense(int id)
        {
            _logger.LogInformation("Eliminando licencia con ID {LicenseId}.", id);
            try
            {
                var result = await _licenseService.DeleteLicense(id);
                if (result)
                {
                    _logger.LogInformation("Licencia con ID {LicenseId} eliminada exitosamente.", id);
                    return Ok(ApiResponse<string>.Ok(null, "Licencia eliminada exitosamente."));
                }
                else
                {
                    _logger.LogWarning("No se encontró la licencia con ID {LicenseId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la licencia."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la licencia con ID {LicenseId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al eliminar la licencia."));
            }
        }
    }
}