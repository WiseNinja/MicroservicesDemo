using Common.Logging;

namespace Common.Infrastructure.Logging;

public class SeqLoggingService : ILoggingService
{
    public Task LogFatalAsync(string message)
    {
        throw new NotImplementedException();
    }

    public Task LogErrorAsync(string message)
    {
        throw new NotImplementedException();
    }

    public Task LogWarningAsync(string message)
    {
        throw new NotImplementedException();
    }

    public Task LogInformationAsync(string message)
    {
        return Task.CompletedTask;
    }

    public Task LogDebugAsync(string message)
    {
        throw new NotImplementedException();
    }
}