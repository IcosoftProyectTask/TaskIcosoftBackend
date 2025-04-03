using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskIcosoftBackend.Common;
using TaskIcosoftBackend.Dtos.CommentsTask;
using TaskIcosoftBackend.Dtos.ImageDtos;
using TaskIcosoftBackend.Dtos.ImageType;
using TaskIcosoftBackend.Hubs;
using TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Service;
using TaskIcosoftBackend.Services;

namespace TaskIcosoftBackend.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/commentsTask")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;
        private readonly ILogger<CommentController> _logger;
        private readonly UserService _userService; // Asegúrate de inyectar el servicio de usuario
        private readonly IHubContext<CommentsHub> _hubContext;

        public CommentController(CommentService commentService, ILogger<CommentController> logger, IHubContext<CommentsHub> hubContext, UserService userService)
        {
            _userService = userService; // Asegúrate de inyectar el servicio de usuario
            _commentService = commentService;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            _logger.LogInformation("Creando un nuevo comentario.");
            try
            {
                var comment = await _commentService.CreateComment(createCommentDto);
                var commentDto = comment.ToDto(); // Asegúrate que este método incluya la info completa del usuario

                // Verificar y asegurar que commentDto.User no sea null
                if (commentDto.User == null)
                {
                    var user = await _userService.GetUserByIdAsync(createCommentDto.UserId);
                    commentDto.User = new UserBasicDto
                    {
                        Id = user.IdUser,
                        Name = user.Name,
                        Avatar = user.Image?.Base64Image ?? string.Empty,
                    };
                }

                // Broadcast el nuevo comentario
                await _hubContext.Clients.Group($"Task_{createCommentDto.TaskId}")
                    .SendAsync("ReceiveNewComment", commentDto);

                _logger.LogInformation("Comentario creado con ID {CommentId}.", comment.Id);
                return Ok(ApiResponse<CommentDto>.Ok(commentDto, "Comentario creado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el comentario.");
                return StatusCode(500, ApiResponse<string>.Error("Error al crear el comentario."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] string content)
        {
            _logger.LogInformation("Actualizando comentario con ID {CommentId}.", id);
            try
            {
                var comment = await _commentService.UpdateComment(id, content);
                if (comment == null)
                {
                    _logger.LogWarning("No se encontró el comentario con ID {CommentId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el comentario."));
                }

                var commentDto = comment.ToDto();

                // Obtener el TaskId del comentario para enviarlo al grupo correcto
                int taskId = comment.TaskId; // Asumiendo que el modelo Comment tiene una propiedad TaskId

                // Broadcast la actualización solo a los clientes en el grupo de la tarea
                await _hubContext.Clients.Group($"Task_{taskId}")
                    .SendAsync("ReceiveCommentUpdate", commentDto);

                return Ok(ApiResponse<CommentDto>.Ok(commentDto, "Comentario actualizado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el comentario con ID {CommentId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al actualizar el comentario."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            _logger.LogInformation("Eliminando comentario con ID {CommentId}.", id);
            try
            {
                // Obtener el TaskId antes de eliminar
                var comment = await _commentService.GetCommentById(id);
                if (comment == null)
                {
                    _logger.LogWarning("No se encontró el comentario con ID {CommentId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el comentario."));
                }

                int taskId = comment.TaskId; // Asumiendo que el modelo Comment tiene una propiedad TaskId

                var result = await _commentService.DeleteComment(id);
                if (!result)
                {
                    _logger.LogWarning("Error al eliminar el comentario con ID {CommentId}.", id);
                    return NotFound(ApiResponse<string>.Error("Error al eliminar el comentario."));
                }

                // Broadcast la eliminación solo a los clientes en el grupo de la tarea
                await _hubContext.Clients.Group($"Task_{taskId}")
                    .SendAsync("ReceiveCommentDeletion", id);

                return Ok(ApiResponse<string>.Ok(null, "Comentario eliminado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el comentario con ID {CommentId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al eliminar el comentario."));
            }
        }


        // Obtener un comentario por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            _logger.LogInformation("Obteniendo comentario con ID {CommentId}.", id);
            try
            {
                // Obtén el comentario desde el servicio
                var comment = await _commentService.GetCommentById(id);
                
                if (comment == null)
                {
                    _logger.LogWarning("No se encontró el comentario con ID {CommentId}.", id);
                    return NotFound(ApiResponse<string>.Error("No se encontró el comentario."));
                }

                // Usa el mapeador para convertir Comment a CommentDto
                var commentDto = comment.ToDto();

                // Devuelve la respuesta con el DTO
                return Ok(ApiResponse<CommentDto>.Ok(commentDto, "Comentario encontrado."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el comentario con ID {CommentId}.", id);
                return StatusCode(500, ApiResponse<string>.Error("Error al obtener el comentario."));
            }
        }

        [HttpGet("task/{taskId}")]
public async Task<IActionResult> GetCommentsByTaskId(int taskId)
{
    _logger.LogInformation("Obteniendo comentarios para la tarea con ID {TaskId}.", taskId);
    try
    {
        var comments = await _commentService.GetCommentsByTaskId(taskId);
        
        // Supongamos que tienes un método ToDto para convertir Comment a CommentDto
        var commentDtos = comments.Select(c => {
            var dto = new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                User = new UserBasicDto
                {
                    Id = c.User.IdUser,
                    Name = c.User.Name,
                    Avatar = c.User.Image?.Base64Image  // Añadir explícitamente el Avatar
                },
                Likes = c.Likes,
                Replies = c.Replies?.Select(r => new CommentReplyDto
                {
                    Id = r.Id,
                    Content = r.Content,
                    User = new UserBasicDto
                        {
                            Id = r.User.IdUser,
                            Name = r.User.Name,
                            Avatar = r.User.Image?.Base64Image  // Añadir explícitamente el Avatar
                        },
                    CreatedAt = r.CreatedAt,
                    // Mapear propiedades de replies
                }).ToList() ?? new List<CommentReplyDto>()
            };
            return dto;
        }).ToList();
        
        return Ok(ApiResponse<IEnumerable<CommentDto>>.Ok(commentDtos, "Comentarios obtenidos exitosamente."));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al obtener los comentarios para la tarea con ID {TaskId}.", taskId);
        return StatusCode(500, ApiResponse<string>.Error("Error al obtener los comentarios."));
    }
}

        /*
                // Obtener todos los comentarios de una tarea específica
                [HttpGet("task/{taskId}")]
                public async Task<IActionResult> GetCommentsByTaskId(int taskId)
                {
                    _logger.LogInformation("Obteniendo comentarios para la tarea con ID {TaskId}.", taskId);
                    try
                    {
                        var comments = await _commentService.GetCommentsByTaskId(taskId);
                        return Ok(ApiResponse<IEnumerable<CommentDto>>.Ok(comments.Select(c => c.ToDto()), "Comentarios obtenidos exitosamente."));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al obtener los comentarios para la tarea con ID {TaskId}.", taskId);
                        return StatusCode(500, ApiResponse<string>.Error("Error al obtener los comentarios."));
                    }
                }

        */
    }

}