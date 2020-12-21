﻿// <auto-generated />
using System;
using Kosystem.Repository.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kosystem.Migrations
{
    [DbContext(typeof(KosystemDbContext))]
    [Migration("20201122230927_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Kosystem.Repository.EF.DbPerson", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("EnqueuedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int?>("RoomDisplayId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoomDisplayId");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Kosystem.Repository.EF.DbRoom", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DisplayId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DisplayId")
                        .IsUnique();

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Kosystem.Repository.EF.DbPerson", b =>
                {
                    b.HasOne("Kosystem.Repository.EF.DbRoom", "Room")
                        .WithMany("People")
                        .HasForeignKey("RoomDisplayId")
                        .HasPrincipalKey("DisplayId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Kosystem.Repository.EF.DbRoom", b =>
                {
                    b.Navigation("People");
                });
#pragma warning restore 612, 618
        }
    }
}