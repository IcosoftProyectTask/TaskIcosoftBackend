using TaskIcosoftBackend.Models;

public class CommentReply
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int CommentId { get; set; }
    public Comment Comment { get; set; }
    public int? ParentReplyId { get; set; } // Nueva propiedad para respuestas anidadas
    public CommentReply ParentReply { get; set; } // Referencia a la respuesta padre
    public ICollection<CommentReply> ChildReplies { get; set; } // Respuestas hijas
    public int Likes { get; set; }
}