using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Data;
using TaskIcosoftBackend.Dtos.CommentsTask;
using TaskIcosoftBackend.Mappers.TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Repository
{
    public class CommentReplyRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<CommentReplyRepository> _logger;

        public CommentReplyRepository(DataContext context, ILogger<CommentReplyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear una nueva respuesta a un comentario
        // Crear una nueva respuesta a un comentario
        public async Task<CommentReply> CreateReply(CreateReplyDto createReplyDto)
        {
            try
            {
                // Validar que si ParentReplyId tiene valor, exista en la base de datos
                if (createReplyDto.ParentReplyId.HasValue && createReplyDto.ParentReplyId.Value > 0)
                {
                    var parentExists = await _context.CommentReplies
                        .AnyAsync(r => r.Id == createReplyDto.ParentReplyId.Value);

                    if (!parentExists)
                    {
                        _logger.LogWarning("La respuesta padre con ID {ParentId} no existe.", createReplyDto.ParentReplyId.Value);
                        throw new ApplicationException($"La respuesta padre con ID {createReplyDto.ParentReplyId.Value} no existe.");
                    }
                }
                else if (createReplyDto.ParentReplyId.HasValue && createReplyDto.ParentReplyId.Value <= 0)
                {
                    // Si ParentReplyId es 0 o negativo, establecerlo como null
                    _logger.LogInformation("Estableciendo ParentReplyId a null porque su valor era {ParentId}", createReplyDto.ParentReplyId.Value);
                    createReplyDto.ParentReplyId = null;
                }

                // Log para depuración
                _logger.LogInformation("Creando respuesta con CommentId={CommentId}, ParentReplyId={ParentId}, UserId={UserId}",
                    createReplyDto.CommentId, createReplyDto.ParentReplyId, createReplyDto.UserId);

                var reply = createReplyDto.ToModel(); // Ahora el mapper usará valores validados
                await _context.CommentReplies.AddAsync(reply);
                await _context.SaveChangesAsync();
                return reply;
            }
            catch (DbUpdateException dbEx)
            {
                // Capturar específicamente errores de base de datos para dar mejor información
                _logger.LogError(dbEx, "Error de base de datos al crear la respuesta al comentario. ParentReplyId={ParentId}", createReplyDto.ParentReplyId);
                throw new ApplicationException($"Error al crear la respuesta. Verifica que el ID de respuesta padre {createReplyDto.ParentReplyId} sea válido.", dbEx);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear la respuesta al comentario.");
                throw new ApplicationException("Error al crear la respuesta al comentario.", e);
            }
        }
        // Obtener una respuesta por ID
        // Obtener una respuesta por ID
        public async Task<CommentReply> GetReplyById(int id)
        {
            try
            {
                var reply = await _context.CommentReplies
                    .Include(r => r.User)
                        .ThenInclude(u => u.Image) // Incluir la imagen del usuario
                    .Include(r => r.ChildReplies) // Incluir respuestas anidadas si las hay
                        .ThenInclude(cr => cr.User)
                            .ThenInclude(u => u.Image) // Incluir imágenes de usuarios en respuestas anidadas
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (reply == null)
                {
                    _logger.LogWarning("No se encontró la respuesta con ID {ReplyId}.", id);
                    return null;
                }
                return reply;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener la respuesta con ID {ReplyId}.", id);
                throw new ApplicationException($"Error al obtener la respuesta con ID {id}.", e);
            }
        }

        // Actualizar una respuesta
        public async Task<CommentReply> UpdateReply(int id, string content)
        {
            try
            {
                var reply = await _context.CommentReplies.FindAsync(id);
                if (reply == null)
                {
                    _logger.LogWarning("No se encontró la respuesta con ID {ReplyId}.", id);
                    return null;
                }

                reply.Content = content;
                reply.UpdatedAt = DateTime.UtcNow;

                _context.CommentReplies.Update(reply);
                await _context.SaveChangesAsync();
                return reply;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al actualizar la respuesta con ID {ReplyId}.", id);
                throw new ApplicationException($"Error al actualizar la respuesta con ID {id}.", e);
            }
        }

        // Eliminar una respuesta (borrado físico)
        public async Task<bool> DeleteReply(int id)
        {
            try
            {
                var reply = await _context.CommentReplies.FindAsync(id);
                if (reply == null)
                {
                    _logger.LogWarning("No se encontró la respuesta con ID {ReplyId}.", id);
                    return false;
                }

                _context.CommentReplies.Remove(reply); // Borrado físico
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al eliminar la respuesta con ID {ReplyId}.", id);
                throw new ApplicationException($"Error al eliminar la respuesta con ID {id}.", e);
            }
        }
    }
}