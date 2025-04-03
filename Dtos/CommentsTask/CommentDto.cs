using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.ImageDtos;

namespace TaskIcosoftBackend.Dtos.CommentsTask
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserBasicDto User { get; set; }
        public int Likes { get; set; }
        public ICollection<CommentReplyDto> Replies { get; set; }
    }
}