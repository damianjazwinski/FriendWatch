using FriendWatch.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FriendWatch.Infrastructure.Persistence
{
    public class FriendWatchDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public FriendWatchDbContext(DbContextOptions<FriendWatchDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conntectionString = _configuration.GetConnectionString("MSSQL") 
                ?? throw new Exception("Error on reading connection string");

            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(conntectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region User
            modelBuilder.Entity<User>()
                .HasMany(receiver => receiver.ReceivedInvitations)
                .WithOne(invitation => invitation.Receiver)
                .HasForeignKey(invitation => invitation.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Circles)
                .WithMany(circle => circle.Members)
                .UsingEntity(
                    "CircleUser",
                    l => l.HasOne(typeof(Circle)).WithMany().HasForeignKey("CirclesId").HasPrincipalKey(nameof(Circle.Id)).OnDelete(DeleteBehavior.NoAction),
                    r => r.HasOne(typeof(User)).WithMany().HasForeignKey("MembersId").HasPrincipalKey(nameof(User.Id)).OnDelete(DeleteBehavior.NoAction),
                    j => j.HasKey("MembersId", "CirclesId"));

            modelBuilder.Entity<User>()
                .HasIndex(user => user.Username)
                .IsUnique();

            #endregion

            modelBuilder.Entity<Circle>()
                .HasOne(circle => circle.Owner)
                .WithMany(owner => owner.OwnedCircles)
                .HasForeignKey(circle => circle.OwnerId);

            modelBuilder.Entity<Circle>()
                .HasMany(circle => circle.SentInvitation)
                .WithOne(invitation => invitation.Circle)
                .HasForeignKey(invitation => invitation.CircleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Circle>()
                .HasIndex(circle => new { circle.Name, circle.OwnerId })
                .IsUnique();


        }

        public DbSet<User> Users { get; set; }
        public DbSet<Circle> Circles { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
    }
}
