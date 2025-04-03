using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.CommentsTask;
using TaskIcosoftBackend.Mappers.TaskIcosoftBackend.Mappers;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Mappers
{
    public static class CommentTaskMapper
    {
        public static CommentDto ToDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                User = comment.User != null ? UserMapper.ToBasicDto(comment.User) : null,
                Likes = comment.Likes,
                Replies = comment.Replies?.Select(r => CommentReplyMapper.ToDto(r)).ToList() ?? new List<CommentReplyDto>()
            };
        }

         

        public static Comment ToModel(this CreateCommentDto createCommentDto)
        {
            return new Comment
            {
                Content = createCommentDto.Content,
                TaskId = createCommentDto.TaskId,
                UserId = createCommentDto.UserId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Likes = 0 
            };
        }
    }
}
