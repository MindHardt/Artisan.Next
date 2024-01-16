using System.ComponentModel.DataAnnotations;
using Artisan.Next.Client.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artisan.Next.Data.Entities;

public class ManagedFile : IEntityTypeConfiguration<ManagedFile>
{
    public const int MaxUniqueNameLength = 32;
    public const string DefaultAvatarName = "DefaultAvatar.jpg";

    [MaxLength(MaxUniqueNameLength)]
    public required string UniqueName { get; set; }
    [MaxLength(256)]
    public required string OriginalName { get; set; }
    [MaxLength(64)]
    public required string MimeType { get; set; }

    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }

    public Guid? OwnerId { get; set; }
    public ApplicationUser? Owner { get; set; }

    public ManagedFileScope Scope { get; set; }

    public void Configure(EntityTypeBuilder<ManagedFile> builder)
    {
        builder.ToTable("Files");
        builder.HasKey(x => x.UniqueName);
        builder.HasIndex(x => new { x.OwnerId, x.Scope });

        builder.Property(x => x.Scope)
            .HasConversion<string>();
        builder.Property(x => x.Scope)
            .HasMaxLength(Enum.GetValues<ManagedFileScope>().Max(x => x.ToString().Length));

        builder.HasOne(x => x.Owner)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.OwnerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(
        [
            new ManagedFile
            {
                MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                OriginalName = DefaultAvatarName,
                UniqueName = DefaultAvatarName,
                OwnerId = null,
                Scope = ManagedFileScope.Avatar
            }
        ]);
    }
}