using Microsoft.EntityFrameworkCore;
using OmniFit.Core.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OmniFit.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TraineeProfile> TraineeProfiles { get; set; }
        public DbSet<TrainerProfile> TrainerProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.TraineeProfile)
                .WithOne(tp => tp.User)
                .HasForeignKey<TraineeProfile>(tp => tp.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.TrainerProfile)
                .WithOne(tp => tp.User)
                .HasForeignKey<TrainerProfile>(tp => tp.UserId);
        }
    }
}