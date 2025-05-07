using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Repository;

namespace TaskIcosoftBackend.Service
{
    public class ClienteAccountInfoService
    {
        private readonly ClienteAccountInfoRepository _clienteAccountInfoRepository;
        private readonly ILogger<ClienteAccountInfoService> _logger;

        public ClienteAccountInfoService(ClienteAccountInfoRepository clienteAccountInfoRepository, ILogger<ClienteAccountInfoService> logger)
        {
            _clienteAccountInfoRepository = clienteAccountInfoRepository;
            _logger = logger;
        }

        public async Task<ClienteAccountInfo> CreateClienteAccountInfo(ClienteAccountInfo clienteAccountInfo)
        {
            _logger.LogInformation("Creando una nueva información de cuenta de cliente.");
            try
            {
                // Nota: La encriptación se maneja en el repositorio
                var result = await _clienteAccountInfoRepository.CreateClienteAccountInfo(clienteAccountInfo);
                _logger.LogInformation("Información de cuenta de cliente creada exitosamente con ID {ClienteAccountInfoId}.", result.IdClienteAccountInfo);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la información de cuenta de cliente.");
                throw; // Relanzar la excepción para que la capa superior la maneje
            }
        }

        public async Task<ClienteAccountInfo> GetClienteAccountInfoById(int id)
        {
            _logger.LogInformation("Obteniendo información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
            try
            {
                var clienteAccountInfo = await _clienteAccountInfoRepository.GetClienteAccountInfoById(id);
                if (clienteAccountInfo == null)
                {
                    _logger.LogWarning("No se encontró la información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
                }
                else
                {
                    _logger.LogInformation("Información de cuenta de cliente con ID {ClienteAccountInfoId} obtenida exitosamente.", id);
                }
                return clienteAccountInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
                throw;
            }
        }

        public async Task<List<ClienteAccountInfo>> GetClienteAccountInfos()
        {
            _logger.LogInformation("Obteniendo lista de información de cuentas de clientes.");
            try
            {
                var clienteAccountInfos = await _clienteAccountInfoRepository.GetClienteAccountInfos();
                _logger.LogInformation("Se obtuvieron {Count} información de cuentas de clientes exitosamente.", clienteAccountInfos.Count);
                return clienteAccountInfos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de información de cuentas de clientes.");
                throw;
            }
        }

        public async Task<ClienteAccountInfo> UpdateClienteAccountInfo(ClienteAccountInfo clienteAccountInfo)
        {
            _logger.LogInformation("Actualizando información de cuenta de cliente con ID {ClienteAccountInfoId}.", clienteAccountInfo.IdClienteAccountInfo);
            try
            {
                // Nota: La verificación y encriptación se maneja en el repositorio
                var result = await _clienteAccountInfoRepository.UpdateClienteAccountInfo(clienteAccountInfo);
                _logger.LogInformation("Información de cuenta de cliente con ID {ClienteAccountInfoId} actualizada exitosamente.", clienteAccountInfo.IdClienteAccountInfo);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la información de cuenta de cliente con ID {ClienteAccountInfoId}.", clienteAccountInfo.IdClienteAccountInfo);
                throw;
            }
        }

        public async Task<bool> DeleteClienteAccountInfo(int id)
        {
            _logger.LogInformation("Eliminando información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
            try
            {
                var result = await _clienteAccountInfoRepository.DeleteClienteAccountInfo(id);
                if (result)
                {
                    _logger.LogInformation("Información de cuenta de cliente con ID {ClienteAccountInfoId} eliminada exitosamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar la información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la información de cuenta de cliente con ID {ClienteAccountInfoId}.", id);
                throw;
            }
        }
    }
}