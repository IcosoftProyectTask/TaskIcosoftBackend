using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gymconnect_backend.Models;
using Microsoft.EntityFrameworkCore;
using TaskIcosoftBackend.Models;

namespace TaskIcosoftBackend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ImageType> ImageTypes { get; set; }
        public DbSet<SessionType> SessionTypes { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<CompanyEmployees> CompanyEmployees { get; set; }
        public DbSet<SupportTasks> SupportTasks { get; set; }
        public DbSet<StatusTask> StatusTasks { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReply> CommentReplies { get; set; }
        public DbSet<Remote> Remotes { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<ClienteAccountInfo> ClienteAccountInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación User - Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.IdRole)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Image)
                .WithMany()
                .HasForeignKey(u => u.IdImage)
                .OnDelete(DeleteBehavior.SetNull);

            // Relación Session - User
            modelBuilder.Entity<Session>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Session - SessionType
            modelBuilder.Entity<Session>()
                .HasOne(s => s.SessionType)
                .WithMany()
                .HasForeignKey(s => s.IdSessionType)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Image>()
                .HasOne(i => i.ImageType)
                .WithMany()
                .HasForeignKey(i => i.IdImageType)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompanyEmployees>()
                .HasOne(ce => ce.Company)
                .WithMany()
                .HasForeignKey(ce => ce.IdCompany)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupportTasks>()
                .HasOne(st => st.User)
                .WithMany()
                .HasForeignKey(st => st.IdUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupportTasks>()
                .HasOne(st => st.Priority)
                .WithMany()
                .HasForeignKey(st => st.IdPriority)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupportTasks>()
                .HasOne(st => st.StatusTask)
                .WithMany()
                .HasForeignKey(st => st.IdStatus)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupportTasks>()
                .HasOne(st => st.Company)
                .WithMany()
                .HasForeignKey(st => st.IdCompany)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Comment - User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Comment - SupportTasks
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Task)
                .WithMany()
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación CommentReply - User
            modelBuilder.Entity<CommentReply>()
                .HasOne(cr => cr.User)
                .WithMany()
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación CommentReply - Comment
            // Relación CommentReply - Comment
            modelBuilder.Entity<CommentReply>()
                .HasOne(cr => cr.Comment)
                .WithMany(c => c.Replies)
                .HasForeignKey(cr => cr.CommentId)
                .OnDelete(DeleteBehavior.Cascade); // Cambiado de Restrict a Cascade
            // Relación recursiva para respuestas anidadas
            modelBuilder.Entity<CommentReply>()
                .HasOne(cr => cr.ParentReply) // Una respuesta puede tener una respuesta padre
                .WithMany(cr => cr.ChildReplies) // Una respuesta padre puede tener muchas respuestas hijas
                .HasForeignKey(cr => cr.ParentReplyId) // Clave foránea para la respuesta padre
                .OnDelete(DeleteBehavior.Restrict); // Restringir eliminación en cascada
        }
    }
}