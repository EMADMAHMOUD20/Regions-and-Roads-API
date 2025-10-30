using App.Core.Domain.IdentityModels;
using App.Core.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace App.Infrastructure.Db;

public class RoadCityDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
{
    public RoadCityDbContext(DbContextOptions<RoadCityDbContext> options) : base(options)
    {
    }

    protected RoadCityDbContext()
    {
    }


    public DbSet<Road> roads { get; set; }
    public DbSet<Region> regions { get; set; }
    public DbSet<Difficulty> difficulties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

      
        var region1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var region2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var region3Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var region4Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var region5Id = Guid.Parse("55555555-5555-5555-5555-555555555555");

        var road1Id = Guid.Parse("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
        var road2Id = Guid.Parse("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaa2");
        var road3Id = Guid.Parse("aaaaaaa3-aaaa-aaaa-aaaa-aaaaaaaaaaa3");
        var road4Id = Guid.Parse("aaaaaaa4-aaaa-aaaa-aaaa-aaaaaaaaaaa4");
        var road5Id = Guid.Parse("aaaaaaa5-aaaa-aaaa-aaaa-aaaaaaaaaaa5");

        var difficulty1Id = Guid.Parse("ddddddd1-dddd-dddd-dddd-ddddddddddd1");
        var difficulty2Id = Guid.Parse("ddddddd2-dddd-dddd-dddd-ddddddddddd2");
        var difficulty3Id = Guid.Parse("ddddddd3-dddd-dddd-dddd-ddddddddddd3");
       

        // Regions
        modelBuilder.Entity<Region>().HasData(
            new Region { id = region1Id, Code = "RG-001", Name = "Northern Highlands", RegionImageUrl = "https://example.com/images/northern_hills.jpg" },
            new Region { id = region2Id, Code = "RG-002", Name = "Desert Plains", RegionImageUrl = "https://example.com/images/desert_plains.jpg" },
            new Region { id = region3Id, Code = "RG-003", Name = "Coastal Edge", RegionImageUrl = "https://example.com/images/coastal_edge.jpg" },
            new Region { id = region4Id, Code = "RG-004", Name = "Forest Path", RegionImageUrl = "https://example.com/images/forest_path.jpg" },
            new Region { id = region5Id, Code = "RG-005", Name = "Urban Loop", RegionImageUrl = "https://example.com/images/urban_loop.jpg" }
        );

        // Roads
        modelBuilder.Entity<Road>().HasData(
            new Road { id = road1Id, Name = "Hilltop Pass",   Description = "A scenic mountain road with sharp curves.", LengthInKm = 12.5, RoadImageUrl = "https://example.com/images/hilltop_pass.jpg", regionId = region1Id , difficultyID = difficulty2Id},
            new Road { id = road2Id, Name = "Sandy Trail",    Description = "A flat road through the desert, good for beginners.", LengthInKm = 25.0, RoadImageUrl = "https://example.com/images/sandy_trail.jpg", regionId = region2Id , difficultyID = difficulty2Id },
            new Road { id = road3Id, Name = "Ocean Drive",    Description = "Beautiful road along the coastline with sea views.", LengthInKm = 18.2, RoadImageUrl = "https://example.com/images/ocean_drive.jpg", regionId = region3Id, difficultyID = difficulty2Id },
            new Road { id = road4Id, Name = "Pinewood Route", Description = "A quiet road through dense pine forests.", LengthInKm = 9.7, RoadImageUrl = "https://example.com/images/pinewood_route.jpg", regionId = region4Id , difficultyID = difficulty3Id },
            new Road { id = road5Id, Name = "City Circuit",   Description = "Circular road inside the city, heavy traffic.", LengthInKm = 5.3, RoadImageUrl = "https://example.com/images/city_circuit.jpg", regionId = region5Id, difficultyID = difficulty1Id }
        );

        // Difficulties
        modelBuilder.Entity<Difficulty>().HasData(
            new Difficulty { id = difficulty1Id, Name = "Easy"},
            new Difficulty { id = difficulty2Id, Name = "Medium" },
            new Difficulty { id = difficulty3Id, Name = "Hard" }
          
        );
        // make id's generating automatically
        modelBuilder.Entity<Region>()
            .Property(r => r.id)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<Road>()
            .Property(r => r.id)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<Difficulty>()
           .Property(d => d.id)
           .HasDefaultValueSql("NEWSEQUENTIALID()");


        // relations 
        modelBuilder.Entity<Region>()
            .HasMany(r => r.roads)
            .WithOne(r => r.region)
            .HasForeignKey(r => r.regionId);

        // many to one relationship between difficulty and road
        modelBuilder.Entity<Difficulty>()
            .HasMany(d => d.roads)
            .WithOne(r => r.difficulty)
            .HasForeignKey(r => r.difficultyID);
    }


}
