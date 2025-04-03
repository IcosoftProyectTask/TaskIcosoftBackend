using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskIcosoftBackend.Dtos.CommentsTask;
using TaskIcosoftBackend.Models;
using TaskIcosoftBackend.Repository;

namespace TaskIcosoftBackend.Services
{
    public class CommentReplyService
    {
        private readonly CommentReplyRepository _commentReplyRepository;
        private readonly ILogger<CommentReplyService> _logger;

        public CommentReplyService(CommentReplyRepository commentReplyRepository, ILogger<CommentReplyService> logger)
        {
            _commentReplyRepository = commentReplyRepository;
            _logger = logger;
        }

        // Crear una nueva respuesta a un comentario
        public async Task<CommentReply> CreateReply(CreateReplyDto createReplyDto)
        {
            try
            {
                return await _commentReplyRepository.CreateReply(createReplyDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error en el servicio al crear la respuesta al comentario.");
                throw new ApplicationException("Error en el servicio al crear la respuesta al comentario.", e);
            }
        }

        // Obtener una respuesta por ID
        public async Task<CommentReply> GetReplyById(int id)
        {
            try
            {
                return await _commentReplyRepository.GetReplyById(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error en el servicio al obtener la respuesta con ID {ReplyId}.", id);
                throw new ApplicationException($"Error en el servicio al obtener la respuesta con ID {id}.", e);
            }
        }

        // Actualizar una respuesta
        public async Task<CommentReply> UpdateReply(int id, string content)
        {
            try
            {
                return await _commentReplyRepository.UpdateReply(id, content);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error en el servicio al actualizar la respuesta con ID {ReplyId}.", id);
                throw new ApplicationException($"Error en el servicio al actualizar la respuesta con ID {id}.", e);
            }
        }

        // Eliminar una respuesta (borrado físico)
        public async Task<bool> DeleteReply(int id)
        {
            try
            {
                return await _commentReplyRepository.DeleteReply(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error en el servicio al eliminar la respuesta con ID {ReplyId}.", id);
                throw new ApplicationException($"Error en el servicio al eliminar la respuesta con ID {id}.", e);
            }
        }
    }
}