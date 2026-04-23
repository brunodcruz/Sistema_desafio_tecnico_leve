namespace LeveTaskSystem.Domain.Services;

public interface IEmailNotifier
{
    Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
