using System.Security.Claims;
using Artisan.Next.Data;
using Artisan.Next.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Artisan.Next.Components.Account;

internal sealed class IdentityUserAccessor(DataContext dataContext, IdentityRedirectManager redirectManager)
{
    public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context, CancellationToken ct = default)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) is { } name
            ? Guid.Parse(name)
            : (Guid?)null;
        var user = userId.HasValue
            ? await dataContext.Users.FirstOrDefaultAsync(x => x.Id == userId, ct)
            : null;

        if (userId is null || user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userId}'.", context);
        }

        return user;
    }
}
