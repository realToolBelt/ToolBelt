using Acr.UserDialogs;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace ToolBelt.Services
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> CheckPermissionsAsync(Permission permission, IUserDialogs dialogService);
    }
}