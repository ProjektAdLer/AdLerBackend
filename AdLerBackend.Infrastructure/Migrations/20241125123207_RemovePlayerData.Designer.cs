﻿// <auto-generated />
using System;
using AdLerBackend.Infrastructure.Repositories;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AdLerBackend.Infrastructure.Migrations
{
    [DbContext(typeof(ProductionContext))]
    [Migration("20241125123207_RemovePlayerData")]
    partial class RemovePlayerData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("AdLerBackend.Domain.Entities.H5PLocationEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1L)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ElementId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("WorldEntityId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorldEntityId");

                    b.ToTable("H5PLocationEntity");
                });

            modelBuilder.Entity("AdLerBackend.Domain.Entities.WorldEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AtfJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LmsWorldId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Worlds");
                });

            modelBuilder.Entity("AdLerBackend.Domain.Entities.H5PLocationEntity", b =>
                {
                    b.HasOne("AdLerBackend.Domain.Entities.WorldEntity", null)
                        .WithMany("H5PFilesInCourse")
                        .HasForeignKey("WorldEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AdLerBackend.Domain.Entities.WorldEntity", b =>
                {
                    b.Navigation("H5PFilesInCourse");
                });
#pragma warning restore 612, 618
        }
    }
}