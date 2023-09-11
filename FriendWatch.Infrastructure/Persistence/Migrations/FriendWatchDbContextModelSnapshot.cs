﻿// <auto-generated />
using System;
using FriendWatch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FriendWatch.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(FriendWatchDbContext))]
    partial class FriendWatchDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CircleUser", b =>
                {
                    b.Property<int>("MembersId")
                        .HasColumnType("int");

                    b.Property<int>("CirclesId")
                        .HasColumnType("int");

                    b.HasKey("MembersId", "CirclesId");

                    b.HasIndex("CirclesId");

                    b.ToTable("CircleUser");
                });

            modelBuilder.Entity("FriendWatch.Domain.Entities.Circle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ImageFileId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ImageFileId")
                        .IsUnique()
                        .HasFilter("[ImageFileId] IS NOT NULL");

                    b.HasIndex("OwnerId");

                    b.HasIndex("Name", "OwnerId")
                        .IsUnique();

                    b.ToTable("Circles");
                });

            modelBuilder.Entity("FriendWatch.Domain.Entities.ImageFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ImageFiles");
                });

            modelBuilder.Entity("FriendWatch.Domain.Entities.Invitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CircleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("CircleId", "ReceiverId")
                        .IsUnique();

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("FriendWatch.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CircleUser", b =>
                {
                    b.HasOne("FriendWatch.Domain.Entities.Circle", null)
                        .WithMany()
                        .HasForeignKey("CirclesId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FriendWatch.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("FriendWatch.Domain.Entities.Circle", b =>
                {
                    b.HasOne("FriendWatch.Domain.Entities.ImageFile", "ImageFile")
                        .WithOne()
                        .HasForeignKey("FriendWatch.Domain.Entities.Circle", "ImageFileId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("FriendWatch.Domain.Entities.User", "Owner")
                        .WithMany("OwnedCircles")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ImageFile");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("FriendWatch.Domain.Entities.Invitation", b =>
                {
                    b.HasOne("FriendWatch.Domain.Entities.Circle", "Circle")
                        .WithMany("SentInvitation")
                        .HasForeignKey("CircleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FriendWatch.Domain.Entities.User", "Receiver")
                        .WithMany("ReceivedInvitations")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Circle");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("FriendWatch.Domain.Entities.Circle", b =>
                {
                    b.Navigation("SentInvitation");
                });

            modelBuilder.Entity("FriendWatch.Domain.Entities.User", b =>
                {
                    b.Navigation("OwnedCircles");

                    b.Navigation("ReceivedInvitations");
                });
#pragma warning restore 612, 618
        }
    }
}
