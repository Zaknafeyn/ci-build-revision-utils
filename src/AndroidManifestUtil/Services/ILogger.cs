namespace AndroidManifestUtil.Services
{
    public interface ILogger
    {
        void Log(string msg, LogSeverity logSeverity = LogSeverity.Info);
    }
}