﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleQueue.Data;

#nullable disable

namespace SimpleQueue.Data.Migrations
{
    [DbContext(typeof(SimpleQueueDBContext))]
    partial class SimpleQueueDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SimpleQueue.Domain.Entities.Conversation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ConversationId");

                    b.Property<DateTime>("Time")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("UserFirstId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserSecondId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserFirstId");

                    b.HasIndex("UserSecondId");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("SimpleQueue.Domain.Entities.ConversationMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ConversationMessageId");

                    b.Property<string>("ContentText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("SendTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("SenderUserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("SenderUserId");

                    b.ToTable("ConversationMessages");
                });

            modelBuilder.Entity("SimpleQueue.Domain.Entities.Queue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("QueueId");

                    b.Property<bool>("Chat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float?>("Latitude")
                        .HasColumnType("float");

                    b.Property<float?>("Longitude")
                        .HasColumnType("float");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StartTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<bool>("isFrozen")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Queues");
                });

            modelBuilder.Entity("SimpleQueue.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("UserId");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<bool>("IsEmailConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(21)
                        .HasColumnType("varchar(21)");

                    b.Property<string>("Surname")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SimpleQueue.Domain.Entities.UserInQueue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("UserInQueueId");

                    b.Property<DateTime>("JoinTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid?>("NextId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("QueueId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("NextId");

                    b.HasIndex("QueueId");

                    b.HasIndex("UserId");

                    b.ToTable("UserInQueues");
                });

            modelBuilder.Entity("SimpleQueue.Domain.Entities.Conversation", b =>
                {
                    b.HasOne("SimpleQueue.Domain.Entities.User", "UserFirst")
                        .WithMany()
                        .HasForeignKey("UserFirstId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleQueue.Domain.Entities.User", "UserSecond")
                        .WithMany()
                        .HasForeignKey("UserSecondId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserFirst");

                    b.Navigation("UserSecond");
                });

            modelBuilder.Entity("SimpleQueue.Domain.Entities.ConversationMessage", b =>
                {
                    b.HasOne("SimpleQueue.Domain.Entities.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleQueue.Domain.Entities.User", "SenderUser")
                        .WithMany()
                        .HasForeignKey("SenderUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("SenderUser");
                });

            modelBuilder.Entity("SimpleQueue.Domain.Entities.UserInQueue", b =>
                {
                    b.HasOne("SimpleQueue.Domain.Entities.UserInQueue", "Next")
                        .WithMany()
                        .HasForeignKey("NextId");

                    b.HasOne("SimpleQueue.Domain.Entities.Queue", "Queue")
                        .WithMany("UserInQueues")
                        .HasForeignKey("QueueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleQueue.Domain.Entities.User", "User")
                        .WithMany("UserInQueues")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Next");

                    b.Navigation("Queue");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SimpleQueue.Domain.Entities.Queue", b =>
                {
                    b.Navigation("UserInQueues");
                });

            modelBuilder.Entity("SimpleQueue.Domain.Entities.User", b =>
                {
                    b.Navigation("UserInQueues");
                });
#pragma warning restore 612, 618
        }
    }
}
