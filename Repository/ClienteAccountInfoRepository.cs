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
    public class ClienteAccountInfoRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ClienteAccountInfoRepository> _logger;

        public ClienteAccountInfoRepository(DataContext context, ILogger<ClienteAccountInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ClienteAccountInfo> CreateClienteAccountInfo(ClienteAccountInfo clienteAccountInfo)
        {
            try
            {
                
                await _context.ClienteAccountInfos.AddAsync(clienteAccountInfo);
                await _context.SaveChangesAsync();
                return clienteAccountInfo;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear la información de cuenta del cliente.");
                throw new ApplicationException("Error al crear la información de cuenta del cliente.", e);
            }
        }

        public async Task<ClienteAccountInfo> GetClienteAccountInfoById(int id)
        {
            try
            {
                return await _context.ClienteAccountInfos
                    .Where(c => c.Status == true) // Solo cuentas activas
                    .FirstOrDefaultAsync(c => c.IdClienteAccountInfo == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al obtener la información de cuenta del cliente con ID {id}.");
                throw new ApplicationException($"Error al obtener la información de cuenta del cliente con ID {id}.", e);
            }
        }

        public async Task<List<ClienteAccountInfo>> GetClienteAccountInfos()
        {
            try
            {
                return await _context.ClienteAccountInfos
                    .Where(c => c.Status == true) // Solo cuentas activas
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener la lista de información de cuentas de clientes.");
                throw new ApplicationException("Error al obtener la lista de información de cuentas de clientes.", e);
            }
        }

        public async Task<ClienteAccountInfo> UpdateClienteAccountInfo(ClienteAccountInfo clienteAccountInfo)
        {
            try
            {
                
                _context.ClienteAccountInfos.Update(clienteAccountInfo);
                await _context.SaveChangesAsync();
                return clienteAccountInfo;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al actualizar la información de cuenta del cliente con ID {clienteAccountInfo.IdClienteAccountInfo}.");
                throw new ApplicationException($"Error al actualizar la información de cuenta del cliente con ID {clienteAccountInfo.IdClienteAccountInfo}.", e);
            }
        }

        public async Task<bool> DeleteClienteAccountInfo(int id)
        {
            try
            {
                var clienteAccountInfo = await _context.ClienteAccountInfos.FindAsync(id);
                if (clienteAccountInfo == null)
                {
                    _logger.LogWarning($"No se encontró la información de cuenta del cliente con ID {id}.");
                    return false;
                }
                // Eliminación lógica: actualizar el Status a false
                clienteAccountInfo.Status = false;
                _context.ClienteAccountInfos.Update(clienteAccountInfo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al eliminar la información de cuenta del cliente con ID {id}.");
                throw new ApplicationException($"Error al eliminar la información de cuenta del cliente con ID {id}.", e);
            }
        }
    }
}