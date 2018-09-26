using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Support.V7.App;
using Firebase.Auth;
using Plugin.CurrentActivity;
using System;
using System.Threading.Tasks;

namespace ToolBelt.Droid.Activities
{
    [Activity(Label = "GoogleLogin", Theme = "@style/MainTheme")]
    public class GoogleLoginActivity : AppCompatActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        private const string KEY_IS_RESOLVING = "is_resolving";
        private const string KEY_SHOULD_RESOLVE = "should_resolve";
        private const int RC_SIGN_IN = 9001;
        private const string TAG = "GoogleLoginActivity";
        private static GoogleSignInAccount mAuth;
        private static GoogleApiClient mGoogleApiClient;

        public static async Task SignOut()
        {
            if (mGoogleApiClient == null)
            {
                mGoogleApiClient = BuildGoogleApiClient();
            }

            mGoogleApiClient.Connect();
            while (mGoogleApiClient.IsConnecting && !mGoogleApiClient.IsConnected)
            {
                await Task.Delay(250);

                // TODO: Should time out?
            }

            await Auth.GoogleSignInApi.SignOut(mGoogleApiClient);
            mGoogleApiClient.Disconnect();
        }

        private FirebaseAuth _firebaseAuth;
        private bool mIsResolving = false;

        private bool mShouldResolve = false;

        public void OnConnected(Bundle connectionHint)
        {
            UpdateData(false);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            if (!mIsResolving && mShouldResolve)
            {
                if (result.HasResolution)
                {
                    try
                    {
                        result.StartResolutionForResult(this, RC_SIGN_IN);
                        mIsResolving = true;
                    }
                    catch (IntentSender.SendIntentException e)
                    {
                        mIsResolving = false;
                        mGoogleApiClient.Connect();
                    }
                }
                else
                {
                    ShowErrorDialog(result);
                }
            }
            else
            {
                UpdateData(false);
            }
        }

        public void OnConnectionSuspended(int cause)
        {
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Result returned from launching the Intent from GoogleSignInApi.getSignInIntent(...);
            if (requestCode == RC_SIGN_IN)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if (result.IsSuccess)
                {
                    // Google Sign In was successful, authenticate with Firebase
                    HandleResult(result.SignInAccount);
                }
                else
                {
                    //GoogleSignInStatusCodes.Canceled
                    var status = result.Status;
                    if (status != null)
                    {
                        Android.Util.Log.Error("TOOLBELT", $"Sign in failed: Status Code = {result.Status.StatusCode}, Message = {result.Status.StatusMessage}");
                    }

                    // Google Sign In failed, update UI appropriately [START_EXCLUDE]
                    HandleResult(null);

                    // [END_EXCLUDE]
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (mGoogleApiClient != null)
            {
                mGoogleApiClient.Dispose();
                mGoogleApiClient = null;
            }

            mGoogleApiClient = BuildGoogleApiClient();

            _firebaseAuth = FirebaseAuth.GetInstance(MainActivity.FirebaseApp);

            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
            StartActivityForResult(signInIntent, RC_SIGN_IN);
        }

        private static GoogleApiClient BuildGoogleApiClient()
        {
#if DEBUG
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken("59954122652-54snfl95i3cigj67phdjpvovvrokn4ec.apps.googleusercontent.com")
                .RequestEmail()
                .Build();
#else
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken("711636366543-0tmtrgmtp43d4q60c31alhur9j2m8ha2.apps.googleusercontent.com")
                .RequestEmail()
                .Build();
#endif

            var builder = new GoogleApiClient.Builder(CrossCurrentActivity.Current.Activity)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso);

            if (CrossCurrentActivity.Current.Activity is GoogleApiClient.IConnectionCallbacks connectionCallbackHandler)
            {
                builder.AddConnectionCallbacks(connectionCallbackHandler);
            }

            if (CrossCurrentActivity.Current.Activity is GoogleApiClient.IOnConnectionFailedListener connectionFailedHandler)
            {
                builder.AddOnConnectionFailedListener(connectionFailedHandler);
            }

            return builder.Build();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutBoolean(KEY_IS_RESOLVING, mIsResolving);
            outState.PutBoolean(KEY_SHOULD_RESOLVE, mIsResolving);
        }

        protected override void OnStart()
        {
            base.OnStart();
            mGoogleApiClient.Connect();
        }

        protected override void OnStop()
        {
            base.OnStop();
            mGoogleApiClient.Disconnect();
        }

        private void HandleResult(GoogleSignInAccount result)
        {
            if (result != null)
            {
                Intent myIntent = new Intent(this, typeof(GoogleLoginActivity));
                myIntent.PutExtra("result", result);
                SetResult(Result.Ok, myIntent);
            }

            Finish();
        }

        private void ShowErrorDialog(ConnectionResult connectionResult)
        {
            int errorCode = connectionResult.ErrorCode;

            if (GoogleApiAvailability.Instance.IsUserResolvableError(errorCode))
            {
                var listener = new DialogInterfaceOnCancelListener();
                listener.OnCancelImpl = (dialog) =>
                {
                    mShouldResolve = false;
                };
                GoogleApiAvailability.Instance.GetErrorDialog(this, errorCode, RC_SIGN_IN, listener).Show();
            }
            else
            {
                mShouldResolve = false;
            }
            HandleResult(mAuth);
        }

        private async void UpdateData(bool isSignedIn)
        {
            if (isSignedIn)
            {
                HandleResult(mAuth);
            }
            else
            {
                await System.Threading.Tasks.Task.Delay(2000);
                mShouldResolve = true;
                mGoogleApiClient.Connect();
            }
        }

        private class DialogInterfaceOnCancelListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
        {
            public Action<IDialogInterface> OnCancelImpl { get; set; }

            public void OnCancel(IDialogInterface dialog)
            {
                OnCancelImpl(dialog);
            }
        }
    }
}