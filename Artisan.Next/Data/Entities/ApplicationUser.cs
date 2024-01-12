using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artisan.Next.Data.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<Guid>, IEntityTypeConfiguration<ApplicationUser>
{
    [MaxLength(ManagedFile.MaxUniqueNameLength)]
    public string AvatarName { get; set; } = ManagedFile.DefaultAvatarName;
    public ManagedFile Avatar { get; set; } = null!;

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.AvatarName)
            .HasDefaultValue(ManagedFile.DefaultAvatarName);

        builder.HasOne(x => x.Avatar)
            .WithMany()
            .HasForeignKey(x => x.AvatarName)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

