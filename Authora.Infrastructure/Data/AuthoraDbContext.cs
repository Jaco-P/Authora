using Microsoft.EntityFrameworkCore;
using Authora.Domain.Entities;

namespace Authora.Infrastructure.Data
{

    public class AuthoraDbContext : DbContext
    {
        public AuthoraDbContext(DbContextOptions<AuthoraDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<UserGroup> UserGroups => Set<UserGroup>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Permissions)
                .HasForeignKey(p => p.GroupId);
        }
    }
}