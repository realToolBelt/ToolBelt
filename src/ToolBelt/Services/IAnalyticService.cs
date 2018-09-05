using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolBelt.Services
{
    /// <summary>
    /// Interface for the analytics service.
    /// </summary>
    public interface IAnalyticService
    {
        /// <summary>
        /// Determines whether the analytics service is enabled or not.
        /// </summary>
        /// <returns><c>true</c> if the service is enabled; otherwise, <c>false</c>.</returns>
        Task<bool> IsEnabledAsync();

        /// <summary>
        /// Enables or disables the analytics service.
        /// </summary>
        /// <param name="enabled">If set to <c>true</c>, the service will be enabled.</param>
        /// <returns>An awaitable task.</returns>
        Task SetEnabledAsync(bool enabled);

        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <param name="name">The event name.</param>
        /// <param name="properties">The properties related to the event.</param>
        void TrackEvent(string name, IDictionary<string, string> properties = null);
    }
}
