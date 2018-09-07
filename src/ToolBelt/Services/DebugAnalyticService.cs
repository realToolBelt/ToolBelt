using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ToolBelt.Services
{
    /// <summary>
    /// Implementation of an <see cref="IAnalyticService" /> that is intended for debugging purposes.
    /// This implementation outputs events to <see cref="System.Diagnostics.Debug" />.
    /// </summary>
    /// <seealso cref="ToolBelt.Services.IAnalyticService" />
    public class DebugAnalyticService : IAnalyticService
    {
        public Task<bool> IsEnabledAsync() => Task.FromResult(true);

        public Task SetEnabledAsync(bool enabled) => Task.CompletedTask;

        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
            StringBuilder sb = new StringBuilder(name);
            foreach (var property in properties)
            {
                sb.AppendLine().AppendFormat("    {0}: {1}", property.Key, property.Value);
            }

            System.Diagnostics.Debug.WriteLine(sb.ToString());
        }
    }
}
