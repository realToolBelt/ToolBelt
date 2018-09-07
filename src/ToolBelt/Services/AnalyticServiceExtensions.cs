using System.Collections.Generic;

namespace ToolBelt.Services
{
    /// <summary>
    /// Extension methods for the <see cref="IAnalyticService" />.
    /// </summary>
    public static class AnalyticServiceExtensions
    {
        /// <summary>
        /// Tracks a screen view.
        /// </summary>
        /// <param name="analyticService">The analytic service.</param>
        /// <param name="screenName">Name of the screen.</param>
        public static void TrackScreen(this IAnalyticService analyticService, string screenName)
        {
            analyticService?.TrackEvent(
                "screen_view",
                new Dictionary<string, string>
                {
                    { "screen_name", screenName }
                });
        }

        /// <summary>
        /// Tracks tap events (button clicks, tap gestures, etc...).
        /// </summary>
        /// <param name="analyticService">The analytic service.</param>
        /// <param name="itemTapped">The item that was tapped.</param>
        public static void TrackTapEvent(this IAnalyticService analyticService, string itemTapped)
        {
            analyticService?.TrackEvent(
                "tapped",
                new Dictionary<string, string>
                {
                    { "tapped_item", itemTapped }
                });
        }
    }
}
