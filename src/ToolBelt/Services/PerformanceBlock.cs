using Splat;
using System;
using System.Diagnostics;
using System.Threading;

namespace ToolBelt.Services
{
    /// <summary>
    /// Used to time the execution of a block of code.
    /// </summary>
    public struct PerformanceBlock : IDisposable
    {
        /// <summary>
        /// An empty performance block.
        /// </summary>
        public static readonly PerformanceBlock Empty = new PerformanceBlock();

        private readonly string _message;
        private readonly ILogger _owner;
        private readonly Stopwatch _stopwatch;
        private int _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceBlock" /> struct.
        /// </summary>
        /// <param name="owner">The owning <see cref="ILogger" /> instance.</param>
        /// <param name="message">The message to output when the block completes.</param>
        public PerformanceBlock(ILogger owner, string message)
        {
            _owner = owner;
            _message = message;

            // NOTE: it's vital we use 0 to represent already disposed so that the Empty performance
            // block does nothing
            _disposed = 1;
            _stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Completes the block. That is, stops the timer and outputs the execution time to the
        /// owning <see cref="ILogger" /> instance.
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 0, 1) != 1)
            {
                return;
            }

            _stopwatch.Stop();
            _owner.Write(
                $"{_message} [{_stopwatch.Elapsed} ({_stopwatch.ElapsedMilliseconds}ms)]",
                LogLevel.Debug);
        }
    }
}
