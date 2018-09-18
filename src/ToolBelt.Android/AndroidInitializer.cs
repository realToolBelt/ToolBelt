using Prism;
using Prism.Ioc;
using ToolBelt.Droid.Services;
using ToolBelt.Services;

namespace ToolBelt.Droid
{
    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IFirebaseAuthService, FirebaseAuthService>();
            containerRegistry.Register<IDeviceOrientation, DeviceOrientationImplementation>();
        }
    }
}