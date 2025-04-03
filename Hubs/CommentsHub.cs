using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TaskIcosoftBackend.Dtos.CommentsTask;

namespace TaskIcosoftBackend.Hubs
{
    public class CommentsHub : Hub
    {
        // Notificar nuevo comentario a todos los clientes de una tarea específica
        public async Task NotifyNewComment(int taskId, CommentDto comment)
        {
            await Clients.Group($"Task_{taskId}").SendAsync("ReceiveNewComment", comment);
        }

        // Notificar nueva respuesta a todos los clientes de una tarea específica
        public async Task NotifyNewReply(int taskId, CommentReplyDto reply)
        {
            await Clients.Group($"Task_{taskId}").SendAsync("ReceiveNewReply", reply);
        }

        // Notificar eliminación de comentario
        public async Task NotifyCommentDeleted(int taskId, int commentId)
        {
            await Clients.Group($"Task_{taskId}").SendAsync("CommentDeleted", commentId);
        }

        // Notificar eliminación de respuesta
        public async Task NotifyReplyDeleted(int taskId, int replyId)
        {
            await Clients.Group($"Task_{taskId}").SendAsync("ReplyDeleted", replyId);
        }

        // Unirse al grupo de una tarea específica
        public async Task JoinTaskGroup(int taskId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Task_{taskId}");
            await Clients.Caller.SendAsync("JoinedGroup", $"Task_{taskId}");
        }

        // Salir del grupo de una tarea específica
        public async Task LeaveTaskGroup(int taskId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Task_{taskId}");
        }
    }
}