using System.ComponentModel.DataAnnotations;
using Artisan.Next.Client.Features.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artisan.Next.Data.Entities;

public record MapArea : IEntityTypeConfiguration<MapArea>
{
    public const int MaxNameLength = 64;
    public int Id { get; set; }

    public int OwnerId { get; set; }
    public ApplicationUser Owner { get; set; } = null!;

    /// <summary>
    /// This <see cref="MapArea"/>s name. It must be unique per <see cref="Owner"/>.
    /// </summary>
    [MaxLength(MaxNameLength)]
    public required string Name { get; set; }

    public required IArea Area { get; set; }

    public void Configure(EntityTypeBuilder<MapArea> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Owner)
            .WithMany(x => x.MapAreas)
            .HasForeignKey(x => x.OwnerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.Area).HasColumnType("jsonb");
        builder.HasIndex(x => new { x.OwnerId, x.Name }).IsUnique();
    }
}