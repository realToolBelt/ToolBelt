using Android.Content;
using Android.Runtime;
using Android.Views;
using ToolBelt.Services;

namespace ToolBelt.Droid.Services
{
    public class DeviceOrientationImplementation : IDeviceOrientation
    {
        public DeviceOrientations GetOrientation()
        {
            IWindowManager windowManager = Android.App.Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            var rotation = windowManager.DefaultDisplay.Rotation;
            bool isLandscape = rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270;
            return isLandscape ? DeviceOrientations.Landscape : DeviceOrientations.Portrait;
        }
    }
}