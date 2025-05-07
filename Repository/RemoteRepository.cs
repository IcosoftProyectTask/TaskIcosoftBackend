using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Custom;
using TaskIcosoftBackend.Data;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Repository
{
    public class RemoteRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<RemoteRepository> _logger;

        public RemoteRepository(DataContext context, ILogger<RemoteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Remote> CreateRemote(Remote remote)
        {
            try
            {
                
                await _context.Remotes.AddAsync(remote);
                await _context.SaveChangesAsync();
                return remote;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear el registro remoto.");
                throw new ApplicationException("Error al crear el registro remoto.", e);
            }
        }

        public async Task<Remote> GetRemoteById(int id)
        {
            try
            {
                return await _context.Remotes
                    .Where(r => r.Status == true) // Solo registros activos
                    .FirstOrDefaultAsync(r => r.IdRemote == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al obtener el registro remoto con ID {id}.");
                throw new ApplicationException($"Error al obtener el registro remoto con ID {id}.", e);
            }
        }

        public async Task<List<Remote>> GetRemotes()
        {
            try
            {
                return await _context.Remotes
                    .Where(r => r.Status == true) // Solo registros activos
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener la lista de registros remotos.");
                throw new ApplicationException("Error al obtener la lista de registros remotos.", e);
            }
        }

        public async Task<Remote> UpdateRemote(Remote remote)
        {
            try
            {  
                _context.Remotes.Update(remote);
                await _context.SaveChangesAsync();
                return remote;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al actualizar el registro remoto con ID {remote.IdRemote}.");
                throw new ApplicationException($"Error al actualizar el registro remoto con ID {remote.IdRemote}.", e);
            }
        }

        public async Task<bool> DeleteRemote(int id)
        {
            try
            {
                var remote = await _context.Remotes.FindAsync(id);
                if (remote == null)
                {
                    _logger.LogWarning($"No se encontró el registro remoto con ID {id}.");
                    return false;
                }
                // Eliminación lógica: actualizar el Status a false
                remote.Status = false;
                _context.Remotes.Update(remote);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al eliminar el registro remoto con ID {id}.");
                throw new ApplicationException($"Error al eliminar el registro remoto con ID {id}.", e);
            }
        }
    }
}