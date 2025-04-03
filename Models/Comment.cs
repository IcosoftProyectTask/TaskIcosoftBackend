using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User User { get; set; }
        public int TaskId { get; set; }
        public SupportTasks Task { get; set; }
        public int Likes { get; set; }
        public ICollection<CommentReply> Replies { get; set; }
    }
}