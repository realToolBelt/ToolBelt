using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using FFImageLoading.Forms.Platform;
using Firebase;
using Plugin.Permissions;
using System;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace ToolBelt.Droid
{
    [Activity(
        Label = "ToolBelt",
        Icon = "@mipmap/ic_launcher",
        RoundIcon = "@mipmap/ic_round_launcher",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static FirebaseApp app;

        public event Action<int, Result, Intent> ActivityResult;

        internal static MainActivity Instance { get; private set; }

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.DecorView.SystemUiVisibility = 0;
                var statusBarHeightInfo = typeof(Xamarin.Forms.Platform.Android.FormsAppCompatActivity)
                    .GetField("_statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                statusBarHeightInfo?.SetValue(this, 0);
                Window.SetStatusBarColor(new Android.Graphics.Color(0, 0, 0, 255)); // Change color as required.
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            ActivityResult?.Invoke(requestCode, resultCode, data);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            InitializeFirebaseAuthentication();

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Acr.UserDialogs.UserDialogs.Init(this);

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            CachedImageRenderer.Init(enableFastRenderer: false);
            CarouselView.FormsPlugin.Android.CarouselViewRenderer.Init();
            LoadApplication(new App(new AndroidInitializer()));

            Xamarin.Forms.Application.Current
                .On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        private void InitializeFirebaseAuthentication()
        {
#if DEBUG
            var options = new FirebaseOptions.Builder()
               .SetApplicationId("1:59954122652:android:09a3c126e798bd59")
               .SetApiKey("AIzaSyDQQ6QWx4luoEbDMkjS-l5Mn9-rZ7HZg0A")
               .Build();
#else
            var options = new FirebaseOptions.Builder()
               .SetApplicationId("1:711636366543:android:09a3c126e798bd59")
               .SetApiKey("AIzaSyDMjWYUtEAsGvQs0bMpmYQnyYraYrk_zyk")
               .Build();
#endif
            
            if (app == null)
            {
                app = FirebaseApp.InitializeApp(this, options, "FirebaseSample");
            }
        }
    }
}