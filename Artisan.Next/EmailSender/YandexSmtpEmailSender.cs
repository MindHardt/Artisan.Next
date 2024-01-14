using System.Text.RegularExpressions;
using Artisan.Next.Data.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Artisan.Next.EmailSender;

public partial class YandexSmtpEmailSender(IOptions<SmtpOptions> options) : IEmailSender<ApplicationUser>
{
    private readonly SmtpOptions _options = options.Value;

    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        var message = PrepareMessage("Подтверждение почты", email, confirmationLink);
        using var client = new SmtpClient();
        await client.ConnectAsync(_options.Address, _options.Port, SecureSocketOptions.SslOnConnect);
        await client.AuthenticateAsync(_options.Host, _options.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    [GeneratedRegex(@"\[[A-Z]+\]")]
    public static partial Regex PreparationRegex();

    private MimeMessage PrepareMessage(string action, string email, string link)
    {
        var html = PreparationRegex().Replace(Template, match => match.Value switch
        {
            "[ACTION]" => action,
            "[EMAIL]" => email,
            "[LINK]" => link,
            "[HOST]" => _options.Host,
            _ => string.Empty
        });
        return new MimeMessage
        {
            Sender = new MailboxAddress(_options.Host, _options.Host),
            To =
            {
                new MailboxAddress(email, email)
            },
            Subject = action,
            Body = new TextPart
            {
                Text = html
            }
        };
    }

    private const string Template =
        """
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <title>[ACTION]</title>
            <!--suppress JSUnresolvedLibraryURL -->
            <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
        </head>
        <body>
        <div class="card m-5">
            <div class="card-header">
                <h3>[ACTION]</h3>
            </div>
            <div class="card-body">
                <h5 class="card-title">[EMAIL],</h5>
                <p class="card-text">
                    Вы получили это письмо так как запросили <strong>[ACTION]</strong>.
                    Чтобы продолжить нажмите на кнопку ниже.
                </p>
                <p class="card-text">
                    Если это были не вы то проигнорируйте это письмо.
                </p>
                <!--suppress HtmlUnknownTarget -->
                <a href="[LINK]" class="btn btn-primary">Продолжить</a>
            </div>
            <div class="card-footer text-muted text-end">
                Отправлено [HOST] на почту [EMAIL].
            </div>
        </div>
        
        </body>
        </html>                            
        """;
}