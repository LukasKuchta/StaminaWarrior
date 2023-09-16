using Microsoft.Extensions.Logging;

namespace Backend.Application.Extensions;
internal static class LoggerExtension
{
    private static readonly Action<ILogger, string, Exception?> _debug =
            LoggerMessage.Define<string>(
                LogLevel.Debug,
                new EventId(1),
                "{Message}");

    private static readonly Action<ILogger, string, Exception> _error =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(0),
            "{Message}");

    private static readonly Func<ILogger, string, IDisposable?> _appendMethod = LoggerMessage.DefineScope<string>("{Method}");

    public static IDisposable? AppendStateScope(this ILogger logger, string method) => _appendMethod(logger, method);

    public static void CommandExecuting(this ILogger logger, string commandName) => _debug(logger, "Executing command", default);

    public static void CommandExecutionError(this ILogger logger, Exception ex) => _error(logger, "Host has finished running", ex);
}
