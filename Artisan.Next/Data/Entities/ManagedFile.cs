using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Artisan.Next.Client.Contracts.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artisan.Next.Data.Entities;

public class ManagedFile : IEntityTypeConfiguration<ManagedFile>
{
    public const int MaxUniqueNameLength = 32;
    public const string DefaultAvatarName = "DefaultAvatar.jpg";
    public const int HashLength = SHA256.HashSizeInBytes * 2;

    [MaxLength(MaxUniqueNameLength)]
    public required string UniqueName { get; set; }
    [MaxLength(256)]
    public required string OriginalName { get; set; }
    [MaxLength(64)]
    public required string MimeType { get; set; }

    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }

    public int? OwnerId { get; set; }
    public ApplicationUser? Owner { get; set; }

    public ManagedFileScope Scope { get; set; }
    
    [MinLength(HashLength), MaxLength(HashLength)]
    public required string Hash { get; set; }

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
                Scope = ManagedFileScope.Avatar,
                Hash = "2e51a70ff807c3368eadebd3c223e96418d90ce22093bcfbde8a087ab96227d6".ToUpper()
            }
        ]);
    }
}