using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Dtos.CommentsTask
{
    public class CreateReplyDto
{
    public int UserId { get; set; }
    public string Content { get; set; }
    public int CommentId { get; set; }
    public int? ParentReplyId { get; set; } // Nueva propiedad para respuestas anidadas
}
}