﻿using FriendWatch.Domain.Entities;

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

            modelBuilder.Entity<User>()
                .HasOne(user => user.Avatar)
                .WithOne()
                .IsRequired(false)
                .HasForeignKey<User>(user => user.AvatarId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Circle
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

            modelBuilder.Entity<Circle>()
                .HasOne(circle => circle.ImageFile)
                .WithOne()
                .IsRequired(false)
                .HasForeignKey<Circle>(circle => circle.ImageFileId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Invitation
            modelBuilder.Entity<Invitation>()
                .HasOne(invitation => invitation.Receiver)
                .WithMany(user => user.ReceivedInvitations)
                .IsRequired(true)
                .HasForeignKey(invitation => invitation.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Invitation>()
                .HasIndex(invitation => new { invitation.CircleId, invitation.ReceiverId })
                .IsUnique();
            #endregion

            #region Watch
            modelBuilder.Entity<Watch>()
                .HasOne(watch => watch.Creator)
                .WithMany()
                .IsRequired()
                .HasForeignKey(watch => watch.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Watch>()
                .HasOne(watch => watch.Circle)
                .WithMany()
                .IsRequired()
                .HasForeignKey(watch => watch.CircleId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Comment
            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.Watch)
                .WithMany(watch => watch.Comments)
                .HasForeignKey(comment => comment.WatchId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.Commenter)
                .WithMany()
                .IsRequired()
                .HasForeignKey(comment => comment.CommenterId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Circle> Circles { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<ImageFile> ImageFiles { get; set; }
        public DbSet<Watch> Watches { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
