using Splat;
using System.ComponentModel;
using System.Diagnostics;

namespace ToolBelt.Services
{
    public class DebugLogger : ILogger
    {
        public LogLevel Level
        {
            get;
            set;
        } = LogLevel.Debug;

        public int WriteCallCount
        {
            get;
            private set;
        }

        public void Write([Localizable(false)] string message, LogLevel logLevel)
        {
            Debug.WriteLine($"{logLevel}: {message}");
            WriteCallCount++;
        }
    }
}
