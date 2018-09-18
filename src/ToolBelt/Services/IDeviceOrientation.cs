namespace ToolBelt.Services
{
    public enum DeviceOrientations
    {
        Undefined,
        Landscape,
        Portrait
    }

    /// <summary>
    /// Service exposing device orientation information.
    /// </summary>
    public interface IDeviceOrientation
    {
        /// <summary>
        /// Gets the orientation of the device.
        /// </summary>
        /// <returns>The orientation of the device.</returns>
        DeviceOrientations GetOrientation();
    }
}
