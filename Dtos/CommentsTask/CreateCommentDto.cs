using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.CommentsTask
{
    public class CreateCommentDto
    {
        public int UserId { get; set; }
        public string Content { get; set; }
        public int TaskId { get; set; }
    }
}