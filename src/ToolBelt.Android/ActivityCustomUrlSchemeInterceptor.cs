using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;
using ToolBelt.Services;

namespace ToolBelt.Droid
{
    [Activity(Label = "ActivityCustomUrlSchemeInterceptor", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [
        IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[]
                    {
                    Intent.CategoryDefault,
                    Intent.CategoryBrowsable
                    },
            DataSchemes = new[]
                    {
                    "com.toolbelt.toolbelt",
                    "com.googleusercontent.apps.59954122652-tcgdvnlbd18pkucfuih43csfru3tq6gg",
                    "fb1324833817652478",
                    "xamarin-auth",
                /*
                "urn:ietf:wg:oauth:2.0:oob",
                "urn:ietf:wg:oauth:2.0:oob.auto",
                "http://localhost:PORT",
                "https://localhost:PORT",
                "http://127.0.0.1:PORT",
                "https://127.0.0.1:PORT",
                "http://[::1]:PORT",
                "https://[::1]:PORT",
                */
                    },

            //DataHost = "localhost",
            //DataHosts = new[]     // NOTE: When this is uncommented, google redirects to google search, not the app...
            //        {
            //         "localhost",
            //         "authorize",                // Facebook in fb1889013594699403://authorize
            //},
            DataPaths = new[]
                    {
                    "/",                        // Facebook
                    "/oauth2redirect",          // Google
                    "/oauth2redirectpath",      // MeetUp
            },
            AutoVerify = true
        )
    ]
    public class ActivityCustomUrlSchemeInterceptor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Android.Net.Uri uri_android = Intent.Data;

#if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("ActivityCustomUrlSchemeInterceptor.OnCreate()");
            sb.Append("     uri_android = ").AppendLine(uri_android.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            Uri uri_netfx = new Uri(uri_android.ToString());

            // load redirect_url Page
            AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

            // return to the main activity
            new System.Threading.Tasks.Task
            (
                () =>
                {
                    var activity = new Intent(Application.Context, typeof(MainActivity));
                    activity.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
                    StartActivity(activity);
                }
            ).Start();

            // finish this activity
            this.Finish();
        }
    }
}