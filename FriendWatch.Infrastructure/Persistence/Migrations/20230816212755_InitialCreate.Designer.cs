﻿// <auto-generated />
using System;
using FriendWatch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FriendWatch.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(FriendWatchDbContext))]
    [Migration("20230816212755_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Circles");
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

                    b.HasIndex("CircleId");

                    b.HasIndex("ReceiverId");

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
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

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
                    b.HasOne("FriendWatch.Domain.Entities.User", "Owner")
                        .WithMany("OwnedCircles")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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
