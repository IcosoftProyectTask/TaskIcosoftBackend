using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskIcosoftBackend.Common;
using TaskIcosoftBackend.Dtos.CommentsTask;
using TaskIcosoftBackend.Hubs;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Mappers.TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Service;
using TaskIcosoftBackend.Services;

namespace TaskIcosoftBackend.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/commentReplaysTask")]
    public class CommentReplyController : ControllerBase
    {
        private readonly CommentReplyService _commentReplyService;
        private readonly UserService _userService;
        private readonly CommentService _commentService;
        private readonly ILogger<CommentReplyController> _logger;
        private readonly IHubContext<CommentsHub> _hubContext;

        public CommentReplyController(CommentReplyService commentReplyService, ILogger<CommentReplyController> logger, IHubContext<CommentsHub> hubContext, CommentService commentService, UserService userService)
        {
            _userService = userService;
            _commentService = commentService;
            _commentReplyService = commentReplyService;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReply([FromBody] CreateReplyDto createReplyDto)
        {
            _logger.LogInformation("Creando una nueva respuesta al comentario.");
            try
            {
                var reply = await _commentReplyService.CreateReply(createReplyDto);

                // Obtener información completa del usuario
                var user = await _userService.GetUserByIdAsync(createReplyDto.UserId);

                // Crear un DTO completo manualmente
                var replyDto = new CommentReplyDto
                {
                    Id = reply.Id,
                    Content = reply.Content,
                    CreatedAt = reply.CreatedAt,
                    CommentId = reply.CommentId,
                    ParentReplyId = reply.ParentReplyId ?? 0,
                    Likes = reply.Likes,
                    User = new UserBasicDto
                    {
                        Id = user.IdUser,
                        Name = user.Name,
                        Avatar = user.Image?.Base64Image ?? string.Empty, // o la propiedad que almacene la imagen
                    }
                };

                // Obtener el TaskId para notificar al grupo correcto
                var comment = await _commentService.GetCommentById(createReplyDto.CommentId);
                int taskId = comment.TaskId;

                // Enviar la notificación con la información completa
                await _hubContext.Clients.Group($"Task_{taskId}")
                    .SendAsync("ReceiveNewReply", replyDto);

                _logger.LogInformation("Respuesta creada con ID {ReplyId}.", reply.Id);
                return Ok(ApiResponse<CommentReplyDto>.Ok(replyDto, "Respuesta creada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la respuesta al comentario.");
                return StatusCode(500, ApiResponse<string>.Error("Error al crear la respuesta al comentario."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReply(int id, [FromBody] string content)
        {
            _logger.LogInformation("Actualizando respuesta con ID {ReplyId}.", id);
            try
            {
                var reply = await _commentReplyService.UpdateReply(id, content);
                if (reply == null)
                {
                    _logger.LogWarning("No se encontró la respuesta con ID {ReplyId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la respuesta."));
                }

                var replyDto = reply.ToDto();

                // Obtener el TaskId del comentario asociado
                var comment = await _commentService.GetCommentById(reply.CommentId);
                int taskId = comment.TaskId;

                // Broadcast la actualización solo a los clientes en el grupo de la tarea
                await _hubContext.Clients.Group($"Task_{taskId}")
                    .SendAsync("ReceiveReplyUpdate", replyDto);

                return Ok(ApiResponse<CommentReplyDto>.Ok(replyDto, "Respuesta actualizada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la respuesta con ID {ReplyId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar la respuesta."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReply(int id)
        {
            _logger.LogInformation("Eliminando respuesta con ID {ReplyId}.", id);
            try
            {
                // Obtener el reply y el comentario asociado antes de eliminar
                var reply = await _commentReplyService.GetReplyById(id);
                if (reply == null)
                {
                    _logger.LogWarning("No se encontró la respuesta con ID {ReplyId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la respuesta."));
                }

                // Obtener el TaskId del comentario asociado
                var comment = await _commentService.GetCommentById(reply.CommentId);
                int taskId = comment.TaskId;

                var result = await _commentReplyService.DeleteReply(id);
                if (!result)
                {
                    _logger.LogWarning("No se encontró la respuesta con ID {ReplyId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la respuesta."));
                }

                // Broadcast la eliminación solo a los clientes en el grupo de la tarea
                await _hubContext.Clients.Group($"Task_{taskId}")
                    .SendAsync("ReceiveReplyDeletion", id);

                return Ok(ApiResponse<string>.Ok(null, "Respuesta eliminada exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la respuesta con ID {ReplyId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al eliminar la respuesta."));
            }
        }

        // Obtener una respuesta por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReply(int id)
        {
            _logger.LogInformation("Obteniendo respuesta con ID {ReplyId}.", id);
            try
            {
                var reply = await _commentReplyService.GetReplyById(id);
                if (reply == null)
                {
                    _logger.LogWarning("No se encontró la respuesta con ID {ReplyId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró la respuesta."));
                }

                // Crear el DTO manualmente para incluir el Avatar
                var replyDto = new CommentReplyDto
                {
                    Id = reply.Id,
                    Content = reply.Content,
                    CreatedAt = reply.CreatedAt,
                    Likes = reply.Likes,
                    User = new UserBasicDto
                    {
                        Id = reply.User.IdUser,
                        Name = reply.User.Name,
                        Avatar = reply.User.Image?.Base64Image // Asignar Avatar desde la imagen
                    },
                    Replies = reply.ChildReplies?.Select(r => new CommentReplyDto
                    {
                        Id = r.Id,
                        Content = r.Content,
                        CreatedAt = r.CreatedAt,
                        Likes = r.Likes,
                        User = new UserBasicDto
                        {
                            Id = r.User.IdUser,
                            Name = r.User.Name,
                            Avatar = r.User.Image?.Base64Image // También asignar Avatar para respuestas anidadas
                        }
                        // No incluimos otro nivel de respuestas anidadas para evitar recursividad infinita
                    }).ToList() ?? new List<CommentReplyDto>()
                };

                return Ok(ApiResponse<CommentReplyDto>.Ok(replyDto, "Respuesta encontrada."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la respuesta con ID {ReplyId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener la respuesta."));
            }
        }
    }
}