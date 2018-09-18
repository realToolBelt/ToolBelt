using ToolBelt.Services;
using UIKit;

namespace ToolBelt.iOS.Services
{
    public class DeviceOrientationImplementation : IDeviceOrientation
    {
        public DeviceOrientations GetOrientation()
        {
            var currentOrientation = UIApplication.SharedApplication.StatusBarOrientation;
            bool isPortrait = currentOrientation == UIInterfaceOrientation.Portrait
                || currentOrientation == UIInterfaceOrientation.PortraitUpsideDown;

            return isPortrait ? DeviceOrientations.Portrait : DeviceOrientations.Landscape;
        }
    }
}