using MailKit.Security;

namespace Artisan.Next.EmailSender;

public record SmtpOptions
{
    public required string Address { get; init; }
    public ushort Port { get; init; } = 465;
    public required string Password { get; init; }
    public required string Host { get; init; }
    public required SecureSocketOptions Security { get; init; }
}