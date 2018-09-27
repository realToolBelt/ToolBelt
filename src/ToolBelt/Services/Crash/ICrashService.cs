using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolBelt.Services.Crash
{
    /// <summary>
    /// Interface for the crash reporting service.
    /// </summary>
    public interface ICrashService
    {
        /// <summary>
        /// Determines whether the crash service is enabled or not.
        /// </summary>
        /// <returns><c>true</c> if the service is enabled; otherwise, <c>false</c>.</returns>
        Task<bool> IsEnabledAsync();

        /// <summary>
        /// Enables or disables the crash service.
        /// </summary>
        /// <param name="enabled">If set to <c>true</c>, the service will be enabled.</param>
        /// <returns>An awaitable task.</returns>
        Task SetEnabledAsync(bool enabled);

        /// <summary>
        /// Tracks the error.
        /// </summary>
        /// <param name="exception">The error to track.</param>
        /// <param name="properties">The properties related to the event.</param>
        void TrackError(Exception exception, IDictionary<string, string> properties = null);
    }
}
