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

        }
    }
}