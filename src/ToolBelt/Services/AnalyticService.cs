using Microsoft.AppCenter.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolBelt.Services
{
    /// <summary>
    /// An implementation of the <see cref="IAnalyticService" /> that wraps the
    /// <see cref="Microsoft.AppCenter.Analytics.Analytics" /> functionality.
    /// </summary>
    /// <seealso cref="ToolBelt.Services.IAnalyticService" />
    /// <seealso cref="Microsoft.AppCenter.Analytics.Analytics" />
    public class AnalyticService : IAnalyticService
    {
        /// <summary>
        /// Determines whether the analytics service is enabled or not.
        /// </summary>
        /// <returns><c>true</c> if the service is enabled; otherwise, <c>false</c>.</returns>
        public Task<bool> IsEnabledAsync()
        {
            return Analytics.IsEnabledAsync();
        }

        /// <summary>
        /// Enables or disables the analytics service.
        /// </summary>
        /// <param name="enabled">If set to <c>true</c>, the service will be enabled.</param>
        /// <returns>An awaitable task.</returns>
        public Task SetEnabledAsync(bool enabled)
        {
            return Analytics.SetEnabledAsync(enabled);
        }

        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <param name="name">The event name.</param>
        /// <param name="properties">The properties related to the event.</param>
        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
            Analytics.TrackEvent(name, properties);
        }
    }
}
