
using TaskIcosoftBackend.Dtos.CommentsTask;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    namespace TaskIcosoftBackend.Mappers
    {
        public static class CommentReplyMapper
        {

            /*
            public static CommentReplyDto ToDto(this CommentReply commentReply)
            {
                return new CommentReplyDto
                {
                    Id = commentReply.Id,
                    Content = commentReply.Content,
                    CreatedAt = commentReply.CreatedAt,
                    User = commentReply.User != null ? UserMapper.ToBasicDto(commentReply.User) : null,
                    Likes = commentReply.Likes
                };
            }

            */

            public static CommentReplyDto ToDto(this CommentReply commentReply)
            {
                return new CommentReplyDto
                {
                    Id = commentReply.Id,
                    Content = commentReply.Content,
                    CreatedAt = commentReply.CreatedAt,
                    User = commentReply.User != null ? UserMapper.ToBasicDto(commentReply.User) : null,
                    Likes = commentReply.Likes,
                    CommentId = commentReply.CommentId, // AÑADIR ESTA LÍNEA
                    ParentReplyId = commentReply.ParentReplyId // AÑADIR ESTA LÍNEA
                };
            }




            public static CommentReply ToModel(this CreateReplyDto createReplyDto)
            {
                return new CommentReply
                {
                    Content = createReplyDto.Content,
                    CommentId = createReplyDto.CommentId,
                    UserId = createReplyDto.UserId,
                    ParentReplyId = createReplyDto.ParentReplyId, // Nueva propiedad
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Likes = 0 // Inicialmente, no tiene likes
                };
            }
        }
    }
}