using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Dtos.CommentsTask;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Repository;

namespace TaskIcosoftBackend.Services
{
    public class CommentService
    {
        private readonly CommentRepository _commentRepository;
        private readonly ILogger<CommentService> _logger;

        public CommentService(CommentRepository commentRepository, ILogger<CommentService> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
        }

        // Crear un nuevo comentario
        public async Task<Comment> CreateComment(CreateCommentDto createCommentDto)
        {
            try
            {
                return await _commentRepository.CreateComment(createCommentDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error en el servicio al crear el comentario.");
                throw new ApplicationException("Error en el servicio al crear el comentario.", e);
            }
        }

        // Obtener un comentario por ID
        public async Task<Comment> GetCommentById(int id)
        {
            try
            {
                return await _commentRepository.GetCommentById(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error en el servicio al obtener el comentario con ID {CommentId}.", id);
                throw new ApplicationException($"Error en el servicio al obtener el comentario con ID {id}.", e);
            }
        }

        // Obtener todos los comentarios de una tarea específica
        public async Task<List<Comment>> GetCommentsByTaskId(int taskId)
        {
            try
            {
                return await _commentRepository.GetCommentsByTaskId(taskId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error en el servicio al obtener los comentarios para la tarea con ID {TaskId}.", taskId);
                throw new ApplicationException($"Error en el servicio al obtener los comentarios para la tarea con ID {taskId}.", e);
            }
        }

        // Actualizar un comentario
        public async Task<Comment> UpdateComment(int id, string content)
        {
            try
            {
                return await _commentRepository.UpdateComment(id, content);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error en el servicio al actualizar el comentario con ID {CommentId}.", id);
                throw new ApplicationException($"Error en el servicio al actualizar el comentario con ID {id}.", e);
            }
        }

        // Eliminar un comentario (borrado físico)
        public async Task<bool> DeleteComment(int id)
        {
            try
            {
                return await _commentRepository.DeleteComment(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error en el servicio al eliminar el comentario con ID {CommentId}.", id);
                throw new ApplicationException($"Error en el servicio al eliminar el comentario con ID {id}.", e);
            }
        }
    }
}