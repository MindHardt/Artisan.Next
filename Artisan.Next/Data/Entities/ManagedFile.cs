using System.ComponentModel.DataAnnotations;
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

    public void Configure(EntityTypeBuilder<ManagedFile> builder)
    {
        builder.ToTable("Files");
        builder.HasKey(x => x.UniqueName);

        builder.HasData(
        [
            new ManagedFile
            {
                MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                OriginalName = DefaultAvatarName,
                UniqueName = DefaultAvatarName
            }
        ]);
    }
}