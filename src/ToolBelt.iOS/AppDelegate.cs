using FFImageLoading.Forms.Platform;
using Foundation;
using System;
using ToolBelt.iOS.Services;
using UIKit;

namespace ToolBelt.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        // This method is invoked when the application has loaded and is ready to run. In this method
        // you should instantiate the window, load the UI into it and then make the window visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            Firebase.Core.App.Configure();

            Flex.FlexButton.Init();
            CachedImageRenderer.Init();
            CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();
            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }

        // For iOS 9 or newer
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return OpenUrl(url);
        }

        // For iOS 8 and older
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return OpenUrl(url);
        }

        private static bool OpenUrl(NSUrl url)
        {
            Uri uri_netfx = new Uri(url.AbsoluteString);

            // load redirect_url Page for parsing
            FirebaseAuthService.XAuth.OnPageLoading(uri_netfx);

            return true;
        }
    }
}
