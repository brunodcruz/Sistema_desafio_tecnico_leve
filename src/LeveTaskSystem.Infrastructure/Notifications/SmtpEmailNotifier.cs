using System.Net;
using System.Net.Mail;
using LeveTaskSystem.Domain.Services;
using Microsoft.Extensions.Options;

namespace LeveTaskSystem.Infrastructure.Notifications;

public class SmtpEmailNotifier(IOptions<EmailSettings> options) : IEmailNotifier
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var senderEmail = NormalizeEmail(_settings.SenderEmail);
        var recipient = NormalizeEmail(to);
        if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(recipient))
        {
            return;
        }

        if (!IsPlausibleMailbox(senderEmail))
        {
            throw new InvalidOperationException(
                "Email:SenderEmail deve ser um endereco verificado no SendGrid (ex.: seu@gmail.com), nao a API key SG.");
        }

        if (!IsPlausibleMailbox(recipient))
        {
            throw new InvalidOperationException("E-mail do destinatario invalido.");
        }

        var displayName = SafeDisplayName(_settings.SenderName);
        MailAddress from;
        try
        {
            from = string.IsNullOrEmpty(displayName)
                ? new MailAddress(senderEmail)
                : new MailAddress(senderEmail, displayName);
        }
        catch (FormatException exception)
        {
            throw new InvalidOperationException(
                "Remetente ou nome de exibicao invalido para e-mail. Verifique Email:SenderEmail e Email:SenderName.",
                exception);
        }

        if (string.IsNullOrWhiteSpace(_settings.Host))
        {
            return;
        }

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl
        };

        if (!string.IsNullOrWhiteSpace(_settings.Username))
        {
            client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
        }

        using var message = new MailMessage(senderEmail, recipient, subject, body)
        {
            IsBodyHtml = false,
            From = from
        };

        await client.SendMailAsync(message, cancellationToken);
    }

    private static string NormalizeEmail(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }

        var s = raw.Trim().Trim('"').Split('\r', '\n', ';')[0].Trim();
        foreach (var prefix in new[] { "Email:", "SenderEmail:", "To:", "email:", "mailto:" })
        {
            if (s.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                s = s[prefix.Length..].Trim();
            }
        }

        return s.Trim();
    }

    private static string SafeDisplayName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }

        return name.Trim().Replace(":", " ").Replace("\r", "").Replace("\n", "");
    }

    private static bool IsPlausibleMailbox(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (value.StartsWith("SG.", StringComparison.Ordinal))
        {
            return false;
        }

        var at = value.IndexOf('@');
        if (at <= 0 || at >= value.Length - 1)
        {
            return false;
        }

        if (value.IndexOf('@', at + 1) >= 0)
        {
            return false;
        }

        var local = value[..at];
        var domain = value[(at + 1)..];
        if (string.IsNullOrWhiteSpace(local) || string.IsNullOrWhiteSpace(domain))
        {
            return false;
        }

        if (!domain.Contains('.'))
        {
            return false;
        }

        return true;
    }
}
