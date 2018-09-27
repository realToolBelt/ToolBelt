using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolBelt.Services.Crash
{
    /// <summary>
    /// An implementation of the <see cref="ICrashService" /> that wraps the
    /// <see cref="Microsoft.AppCenter.Crashes.Crashes" /> functionality.
    /// </summary>
    /// <seealso cref="ToolBelt.Services.ICrashService" />
    /// <seealso cref="Microsoft.AppCenter.Crashes.Crashes" />
    public class CrashService : ICrashService
    {
        /// <summary>
        /// Determines whether the crash service is enabled or not.
        /// </summary>
        /// <returns><c>true</c> if the service is enabled; otherwise, <c>false</c>.</returns>
        public Task<bool> IsEnabledAsync()
        {
            return Crashes.IsEnabledAsync();
        }

        /// <summary>
        /// Enables or disables the crash service.
        /// </summary>
        /// <param name="enabled">If set to <c>true</c>, the service will be enabled.</param>
        /// <returns>An awaitable task.</returns>
        public Task SetEnabledAsync(bool enabled)
        {
            return Crashes.SetEnabledAsync(enabled);
        }

        /// <summary>
        /// Tracks the error.
        /// </summary>
        /// <param name="exception">The error to track.</param>
        /// <param name="properties">The properties related to the event.</param>
        public void TrackError(Exception exception, IDictionary<string, string> properties = null)
        {
            Crashes.TrackError(exception, properties);
        }
    }
}
