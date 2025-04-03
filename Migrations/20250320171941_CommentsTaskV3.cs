using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskIcosoftBackend.Migrations
{
    /// <inheritdoc />
    public partial class CommentsTaskV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReplies_CommentReplies_ParentReplyId",
                table: "CommentReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentReplies_Comments_CommentId",
                table: "CommentReplies");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReplies_CommentReplies_ParentReplyId",
                table: "CommentReplies",
                column: "ParentReplyId",
                principalTable: "CommentReplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReplies_Comments_CommentId",
                table: "CommentReplies",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReplies_CommentReplies_ParentReplyId",
                table: "CommentReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentReplies_Comments_CommentId",
                table: "CommentReplies");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReplies_CommentReplies_ParentReplyId",
                table: "CommentReplies",
                column: "ParentReplyId",
                principalTable: "CommentReplies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReplies_Comments_CommentId",
                table: "CommentReplies",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
