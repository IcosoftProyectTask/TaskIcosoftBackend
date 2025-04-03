using TaskIcosoftBackend.Dtos.CommentsTask;

public class CommentReplyDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserBasicDto User { get; set; }
    public int Likes { get; set; }
    public int CommentId { get; set; }
    public int? ParentReplyId { get; set; }
    public ICollection<CommentReplyDto> Replies { get; set; }
}