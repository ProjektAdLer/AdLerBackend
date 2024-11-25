﻿using System.Diagnostics.CodeAnalysis;
using AdLerBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdLerBackend.Infrastructure.Repositories.BaseContext;

// Exclude this from code coverage as it is just a wrapper around the DbContext
[ExcludeFromCodeCoverage]
public class BaseAdLerBackendDbContext : DbContext
{
    public BaseAdLerBackendDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<WorldEntity> Worlds { get; set; } = null!;

    private DbSet<H5PLocationEntity> H5PLocations { get; set; } = null!;
    // private DbSet<PlayerData> PlayerData { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorldEntity>()
            .HasMany(x => x.H5PFilesInCourse).WithOne().HasForeignKey("WorldEntityId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<H5PLocationEntity>()
            .Property("Id").UseIdentityColumn();

        modelBuilder.Entity<H5PLocationEntity>()
            .HasKey("Id");
    }
}