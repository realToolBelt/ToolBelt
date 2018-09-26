using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
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
        Theme = "@style/SplashTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static FirebaseApp FirebaseApp { get; private set; }

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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // if we are not hitting the internal "home" button, just return without any action
            if (item.ItemId != Android.Resource.Id.Home)
            {
                return base.OnOptionsItemSelected(item);
            }

            //this one triggers the hardware back button press handler - so we are back in XF without even mentioning it
            OnBackPressed();

            // return true to signal we have handled everything fine
            return true;
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

            base.Window.RequestFeature(WindowFeatures.ActionBar);

            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
            base.SetTheme(Resource.Style.MainTheme);

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

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            var toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolBar != null)
            {
                SetSupportActionBar(toolBar);
            }

            base.OnPostCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            var toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolBar != null)
            {
                SetSupportActionBar(toolBar);
            }

            base.OnResume();
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

            if (FirebaseApp == null)
            {
                FirebaseApp = FirebaseApp.InitializeApp(this, options, "FirebaseSample");
            }
        }
    }
}