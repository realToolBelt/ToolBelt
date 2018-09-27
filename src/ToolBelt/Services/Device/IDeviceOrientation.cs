using Xamarin.Essentials;

namespace ToolBelt.Services.Device
{
    /// <summary>
    /// Service exposing device orientation information.
    /// </summary>
    public interface IDeviceOrientation
    {
        /// <summary>
        /// Gets the screen metrics of the device.
        /// </summary>
        ScreenMetrics ScreenMetrics { get; }
    }
}
