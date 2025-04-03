using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.CommentsTask
{
    public class LikeDto
    {
        public int UserId { get; set; } // ID del usuario que da "me gusta"
        public int CommentId { get; set; }
        public int CommentReplyId { get; set; }
    }
}