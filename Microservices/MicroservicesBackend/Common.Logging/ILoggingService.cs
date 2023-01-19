using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public interface ILoggingService
    {
        Task LogFatalAsync(string message);
        Task LogErrorAsync(string message);
        Task LogWarningAsync(string message);
        Task LogInformationAsync(string message);
        Task LogDebugAsync(string message);
    }
}
