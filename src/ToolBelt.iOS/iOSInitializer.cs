using Prism;
using Prism.Ioc;
using ToolBelt.iOS.Services;
using ToolBelt.Services;

namespace ToolBelt.iOS
{
    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IFirebaseAuthService, FirebaseAuthService>();
        }
    }
}
