using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskIcosoftBackend.Migrations
{
    /// <inheritdoc />
    public partial class CommentsTaskv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentReplyId",
                table: "CommentReplies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentReplies_ParentReplyId",
                table: "CommentReplies",
                column: "ParentReplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReplies_CommentReplies_ParentReplyId",
                table: "CommentReplies",
                column: "ParentReplyId",
                principalTable: "CommentReplies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReplies_CommentReplies_ParentReplyId",
                table: "CommentReplies");

            migrationBuilder.DropIndex(
                name: "IX_CommentReplies_ParentReplyId",
                table: "CommentReplies");

            migrationBuilder.DropColumn(
                name: "ParentReplyId",
                table: "CommentReplies");
        }
    }
}
