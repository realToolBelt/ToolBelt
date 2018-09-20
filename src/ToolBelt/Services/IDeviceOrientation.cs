using Xamarin.Essentials;

namespace ToolBelt.Services
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

    /// <summary>
    /// Implementation of the <see cref="IDeviceOrientation" /> interface.
    /// </summary>
    /// <seealso cref="ToolBelt.Services.IDeviceOrientation" />
    public class DeviceOrientationImplementation : IDeviceOrientation
    {
        /// <summary>
        /// Gets the screen metrics of the device.
        /// </summary>
        public ScreenMetrics ScreenMetrics => DeviceDisplay.ScreenMetrics;
    }
}
