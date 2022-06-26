﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SimpleForum.Data;

#nullable disable

namespace SimpleForum.Migrations
{
    [DbContext(typeof(SimpleForumContext))]
    [Migration("20220626002154_AddedForumModel")]
    partial class AddedForumModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SimpleForum.Models.Forum", b =>
                {
                    b.Property<string>("ForumId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ForumId");

                    b.ToTable("Forum", (string)null);
                });

            modelBuilder.Entity("SimpleForum.Models.ForumReply", b =>
                {
                    b.Property<string>("ReplyId")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ThreadId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ReplyId");

                    b.HasIndex("ThreadId");

                    b.HasIndex("UserId");

                    b.ToTable("Reply", (string)null);
                });

            modelBuilder.Entity("SimpleForum.Models.ForumThread", b =>
                {
                    b.Property<string>("ThreadId")
                        .HasColumnType("text");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ForumId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ThreadId");

                    b.HasIndex("ForumId");

                    b.HasIndex("UserId");

                    b.ToTable("Thread", (string)null);
                });

            modelBuilder.Entity("SimpleForum.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateJoined")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Username");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("SimpleForum.Models.ForumReply", b =>
                {
                    b.HasOne("SimpleForum.Models.ForumThread", "Thread")
                        .WithMany("Replies")
                        .HasForeignKey("ThreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleForum.Models.User", "User")
                        .WithMany("Replies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Thread");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SimpleForum.Models.ForumThread", b =>
                {
                    b.HasOne("SimpleForum.Models.Forum", "Forum")
                        .WithMany("Threads")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleForum.Models.User", "User")
                        .WithMany("Threads")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forum");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SimpleForum.Models.Forum", b =>
                {
                    b.Navigation("Threads");
                });

            modelBuilder.Entity("SimpleForum.Models.ForumThread", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("SimpleForum.Models.User", b =>
                {
                    b.Navigation("Replies");

                    b.Navigation("Threads");
                });
#pragma warning restore 612, 618
        }
    }
}
