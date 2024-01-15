using System.Text.RegularExpressions;
using Artisan.Next.Data.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Artisan.Next.EmailSender;

public partial class MailKitEmailSender(IOptions<SmtpOptions> options, IWebHostEnvironment env) : IEmailSender<ApplicationUser>
{
    private readonly SmtpOptions _options = options.Value;
    private readonly string _templatePath = Path.Combine(env.WebRootPath, "emails", "email.html");

    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        var message = await PrepareMessage("Подтверждение почты", email, confirmationLink, "Продолжить", false);
        await SendMessage(message);
    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        var message = await PrepareMessage("Сброс пароля", email, resetLink, "Продолжить", false);
        await SendMessage(message);
    }

    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        var message = await PrepareMessage("Код сброса пароля", email, "", resetCode, true);
        await SendMessage(message);
    }

    private async Task SendMessage(MimeMessage message)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_options.Address, _options.Port, SecureSocketOptions.SslOnConnect);
        await client.AuthenticateAsync(_options.Host, _options.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    [GeneratedRegex(@"\[[A-Z]+\]")]
    public static partial Regex PreparationRegex();

    private async Task<MimeMessage> PrepareMessage(string action, string email, string link, string button, bool disabled)
    {
        var template = await File.ReadAllTextAsync(_templatePath);
        var html = PreparationRegex().Replace(template, match => match.Value switch
        {
            "[ACTION]" => action,
            "[EMAIL]" => email,
            "[LINK]" => link,
            "[HOST]" => _options.Host,
            "[BUTTON]" => button,
            "[DISABLED]" => disabled.ToString(),
            _ => string.Empty
        });
        return new MimeMessage
        {
            Sender = new MailboxAddress("Artisan.Next", _options.Host),
            To =
            {
                new MailboxAddress(email, email)
            },
            Subject = action,
            Body = new TextPart(TextFormat.Html)
            {
                Text = html
            }
        };
    }
}