using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Util;
using Firebase.Auth;
using Splat;
using System;
using System.Threading.Tasks;
using ToolBelt.Droid.Activities;
using ToolBelt.Services;

namespace ToolBelt.Droid.Services
{
    public class FirebaseAuthService : IFirebaseAuthService, IEnableLogger
    {
        public const string KEY_AUTH = "auth";
        public const int REQ_AUTH = 9999;

        public string getAuthKey()
        {
            return KEY_AUTH;
        }

        public string GetCurrentUserId()
        {
            var user = GetFirebaseAuthInstance().CurrentUser;
            return user.Uid;
        }

        public bool IsUserSigned()
        {
            var user = GetFirebaseAuthInstance().CurrentUser;
            return user != null;
        }

        public async Task<bool> Logout()
        {
            try
            {
                GetFirebaseAuthInstance().SignOut();
                await GoogleLoginActivity.SignOut();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("TOOLBELT", ex.ToString());
                this.Log().ErrorException("Error when logging out", ex);
                return false;
            }
        }

        public async Task<bool> SignIn(string email, string password)
        {
            try
            {
                await GetFirebaseAuthInstance().SignInWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("TOOLBELT", ex.ToString());
                this.Log().ErrorException("Error signing in", ex);
                return false;
            }
        }

        public Task<bool> SignInWithGoogle()
        {
            Log.Debug("TOOLBELT", "Signing in with Google");
            var activity = MainActivity.Instance;
            var listener = new ActivityResultListener(activity);

            Log.Debug("TOOLBELT", "Starting google login activity");
            var googleIntent = new Intent(activity, typeof(GoogleLoginActivity));
            activity.StartActivityForResult(googleIntent, REQ_AUTH);

            return listener.Task.ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully && !string.IsNullOrWhiteSpace(t.Result))
                {
                    return SignInWithGoogle(t.Result);
                }

                if (t.Exception != null)
                {
                    Log.Error("TOOLBELT", t.Exception.ToString());
                    this.Log().ErrorException("Error signing in with Google", t.Exception);
                }

                return Task.FromResult(false);
            }).Unwrap();
        }

        public async Task<bool> SignUp(string email, string password)
        {
            try
            {
                await GetFirebaseAuthInstance().CreateUserWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("TOOLBELT", ex.ToString());
                this.Log().ErrorException("Error signing up", ex);
                return false;
            }
        }

        private FirebaseAuth GetFirebaseAuthInstance() => FirebaseAuth.GetInstance(MainActivity.FirebaseApp);

        private async Task<bool> SignInWithGoogle(string token)
        {
            try
            {
                Log.Debug("TOOLBELT", $"Signing in with Google using token: {token}");
                AuthCredential credential = GoogleAuthProvider.GetCredential(token, null);

                Log.Debug("TOOLBELT", $"Signing in to Firebase");
                await GetFirebaseAuthInstance().SignInWithCredentialAsync(credential);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("TOOLBELT", ex.ToString());
                this.Log().ErrorException("Error signing in with Google", ex);
                return false;
            }
        }

        private class ActivityResultListener
        {
            private readonly TaskCompletionSource<string> _complete = new TaskCompletionSource<string>();

            public ActivityResultListener(MainActivity activity)
            {
                // subscribe to activity results
                activity.ActivityResult += OnActivityResult;
            }

            public Task<string> Task => _complete.Task;

            private void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                // unsubscribe from activity results
                MainActivity.Instance.ActivityResult -= OnActivityResult;

                Log.Debug("TOOLBELT", $"Activity Result: Request Code = {requestCode}, Result Code = {resultCode}");

                if (requestCode == FirebaseAuthService.REQ_AUTH)
                {
                    if (resultCode == Result.Ok)
                    {
                        GoogleSignInAccount sg = (GoogleSignInAccount)data.GetParcelableExtra("result");
                        _complete.TrySetResult(sg.IdToken);
                    }
                    else
                    {
                        _complete.TrySetResult(null);
                    }
                }
            }
        }
    }
}