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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

             // Relación User - Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.IdRole)
                .OnDelete(DeleteBehavior.Restrict);

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

        }
    }
}