using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Data;
using TaskIcosoftBackend.Dtos.CommentsTask;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Repository
{
    public class CommentRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(DataContext context, ILogger<CommentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear un nuevo comentario
        public async Task<Comment> CreateComment(CreateCommentDto createCommentDto)
        {
            try
            {
                var comment = createCommentDto.ToModel(); // Usa el mapper para convertir DTO a modelo
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return comment;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al crear el comentario.");
                throw new ApplicationException("Error al crear el comentario.", e);
            }
        }

        // Obtener un comentario por ID
        public async Task<Comment> GetCommentById(int id)
        {
            try
            {
                var comment = await _context.Comments
                    .Include(c => c.User) // Incluir la información del usuario
                    .Include(c => c.Replies) // Incluir las respuestas
                    .ThenInclude(r => r.User) // Incluir la información del usuario en las respuestas
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (comment == null)
                {
                    _logger.LogWarning("No se encontró el comentario con ID {CommentId}.", id);
                    return null;
                }

                return comment;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener el comentario con ID {CommentId}.", id);
                throw new ApplicationException($"Error al obtener el comentario con ID {id}.", e);
            }
        }

        // Obtener todos los comentarios de una tarea específica
        public async Task<List<Comment>> GetCommentsByTaskId(int taskId)
        {
            try
            {
                var comments = await _context.Comments
            .Include(c => c.User)
                .ThenInclude(u => u.Image) // Incluir la imagen del usuario
            .Include(c => c.Replies) // Incluir las respuestas
                .ThenInclude(r => r.User) // Incluir el usuario en las respuestas
                    .ThenInclude(u => u.Image) // Incluir la imagen del usuario de la respuesta
            .Where(c => c.TaskId == taskId)
            .ToListAsync();

                if (!comments.Any())
                {
                    _logger.LogWarning("No se encontraron comentarios para la tarea con ID {TaskId}.", taskId);
                }

                return comments;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al obtener los comentarios para la tarea con ID {TaskId}.", taskId);
                throw new ApplicationException($"Error al obtener los comentarios para la tarea con ID {taskId}.", e);
            }
        }

        // Actualizar un comentario
        public async Task<Comment> UpdateComment(int id, string content)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment == null)
                {
                    _logger.LogWarning("No se encontró el comentario con ID {CommentId}.", id);
                    return null;
                }

                comment.Content = content;
                comment.UpdatedAt = DateTime.UtcNow;
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
                return comment;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al actualizar el comentario con ID {CommentId}.", id);
                throw new ApplicationException($"Error al actualizar el comentario con ID {id}.", e);
            }
        }

        public async Task<bool> DeleteComment(int id)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment == null)
                {
                    _logger.LogWarning("No se encontró el comentario con ID {CommentId}.", id);
                    return false;
                }

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al eliminar el comentario con ID {CommentId}.", id);
                throw new ApplicationException($"Error al eliminar el comentario con ID {id}.", e);
            }
        }

        public async Task<Comment> AddLikeToComment(int commentId)
        {
            try
            {
                // Buscar el comentario por ID
                var comment = await _context.Comments.FindAsync(commentId);
                if (comment == null)
                {
                    _logger.LogWarning("No se encontró el comentario con ID {CommentId}.", commentId);
                    throw new ApplicationException($"No se encontró el comentario con ID {commentId}.");
                }

                // Incrementar el número de "likes"
                comment.Likes++;
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();

                return comment;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al agregar un like al comentario con ID {CommentId}.", commentId);
                throw new ApplicationException($"Error al agregar un like al comentario con ID {commentId}.", e);
            }
        }

        public async Task<Comment> RemoveLikeFromComment(int commentId)
        {
            try
            {
                // Buscar el comentario por ID
                var comment = await _context.Comments.FindAsync(commentId);
                if (comment == null)
                {
                    _logger.LogWarning("No se encontró el comentario con ID {CommentId}.", commentId);
                    throw new ApplicationException($"No se encontró el comentario con ID {commentId}.");
                }

                // Decrementar el número de "likes" (no permitir valores negativos)
                if (comment.Likes > 0)
                {
                    comment.Likes--;
                }

                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();

                return comment;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al quitar un like del comentario con ID {CommentId}.", commentId);
                throw new ApplicationException($"Error al quitar un like del comentario con ID {commentId}.", e);
            }
        }


    }
}