using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Custom;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Repository;

namespace TaskIcosoftBackend.Service
{
    public class RemoteService
    {
        private readonly RemoteRepository _remoteRepository;
        private readonly ILogger<RemoteService> _logger;

        public RemoteService(RemoteRepository remoteRepository, ILogger<RemoteService> logger)
        {
            _remoteRepository = remoteRepository;
            _logger = logger;
        }

        public async Task<Remote> CreateRemote(Remote remote)
        {
            _logger.LogInformation("Creando un nuevo registro remoto.");
            try
            {
                // Nota: La encriptación se maneja en el repositorio
                var result = await _remoteRepository.CreateRemote(remote);
                _logger.LogInformation("Registro remoto creado exitosamente con ID {RemoteId}.", result.IdRemote);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el registro remoto.");
                throw; // Relanzar la excepción para que la capa superior la maneje
            }
        }

        public async Task<Remote> GetRemoteById(int id)
        {
            _logger.LogInformation("Obteniendo registro remoto con ID {RemoteId}.", id);
            try
            {
                var remote = await _remoteRepository.GetRemoteById(id);
                if (remote == null)
                {
                    _logger.LogWarning("No se encontró el registro remoto con ID {RemoteId}.", id);
                }
                else
                {
                    _logger.LogInformation("Registro remoto con ID {RemoteId} obtenido exitosamente.", id);
                }
                return remote;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro remoto con ID {RemoteId}.", id);
                throw;
            }
        }

        public async Task<List<Remote>> GetRemotes()
        {
            _logger.LogInformation("Obteniendo lista de registros remotos.");
            try
            {
                var remotes = await _remoteRepository.GetRemotes();
                _logger.LogInformation("Se obtuvieron {Count} registros remotos exitosamente.", remotes.Count);
                return remotes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de registros remotos.");
                throw;
            }
        }

        public async Task<Remote> UpdateRemote(Remote remote)
        {
            _logger.LogInformation("Actualizando registro remoto con ID {RemoteId}.", remote.IdRemote);
            try
            {
                // Nota: La verificación y encriptación se maneja en el repositorio
                var result = await _remoteRepository.UpdateRemote(remote);
                _logger.LogInformation("Registro remoto con ID {RemoteId} actualizado exitosamente.", remote.IdRemote);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro remoto con ID {RemoteId}.", remote.IdRemote);
                throw;
            }
        }

        public async Task<bool> DeleteRemote(int id)
        {
            _logger.LogInformation("Eliminando registro remoto con ID {RemoteId}.", id);
            try
            {
                var result = await _remoteRepository.DeleteRemote(id);
                if (result)
                {
                    _logger.LogInformation("Registro remoto con ID {RemoteId} eliminado exitosamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar el registro remoto con ID {RemoteId}.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro remoto con ID {RemoteId}.", id);
                throw;
            }
        }
    }
}