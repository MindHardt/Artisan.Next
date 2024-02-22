using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Artisan.Next.Client.Contracts.Files;
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

    public required ManagedFileScope Scope { get; set; }
    public required HashString Hash { get; set; }
    
    public required bool IsDefaultFile { get; set; }

    public void Configure(EntityTypeBuilder<ManagedFile> builder)
    {
        builder.ToTable("Files");
        builder.HasKey(x => x.UniqueName);
        builder.HasIndex(x => new { x.OwnerId, x.Scope });

        builder.Property(x => x.Scope)
            .HasConversion<string>();
        builder.Property(x => x.Scope)
            .HasMaxLength(Enum.GetValues<ManagedFileScope>().Max(x => x.ToString().Length));

        var unixEpoch = new DateTimeOffset(new DateTime(1970, 1, 1), TimeSpan.Zero);
        builder.Property(x => x.DateCreated).HasDefaultValue(unixEpoch);
        builder.Property(x => x.DateUpdated).HasDefaultValue(unixEpoch);

        builder.HasOne(x => x.Owner)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.OwnerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.Hash)
            .HasConversion<HashString.EfCoreValueConverter, HashString.EfCoreValueComparer>()
            .HasMaxLength(HashString.Length);

        builder.HasData(
        [
            new ManagedFile
            {
                MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                OriginalName = DefaultAvatarName,
                UniqueName = DefaultAvatarName,
                OwnerId = null,
                Scope = ManagedFileScope.Avatar,
                Hash = HashString.From("2e51a70ff807c3368eadebd3c223e96418d90ce22093bcfbde8a087ab96227d6"),
                IsDefaultFile = true
            }
        ]);
    }
}