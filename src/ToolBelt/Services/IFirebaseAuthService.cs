using System.Threading.Tasks;

namespace ToolBelt.Services
{
    /// <summary>
    /// A service used to authenticate with Firebase.
    /// </summary>
    public interface IFirebaseAuthService
    {
        string getAuthKey();

        string GetCurrentUserId();

        bool IsUserSigned();

        Task<bool> Logout();

        Task<bool> SignIn(string email, string password);

        Task<bool> SignInWithGoogle();

        Task<bool> SignUp(string email, string password);
    }
}
