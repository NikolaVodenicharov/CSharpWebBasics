﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebServer.GameStoreApplication.Data;

namespace WebServer.GameStoreApplication.Data.Migrations
{
    [DbContext(typeof(GameStoreDbContext))]
    partial class GameStoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebServer.GameStoreApplication.Data.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.Property<decimal>("Price");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<double>("SizeGigabytes");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("TrailerId")
                        .HasMaxLength(11);

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("WebServer.GameStoreApplication.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FullName")
                        .HasMaxLength(50);

                    b.Property<bool>("IsAdministrator");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebServer.GameStoreApplication.Data.Models.UserGame", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("GameId");

                    b.HasKey("UserId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("UsersGames");
                });

            modelBuilder.Entity("WebServer.GameStoreApplication.Data.Models.UserGame", b =>
                {
                    b.HasOne("WebServer.GameStoreApplication.Data.Models.Game", "Game")
                        .WithMany("UsersGames")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebServer.GameStoreApplication.Data.Models.User", "User")
                        .WithMany("UsersGames")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
