using System.Threading.Tasks;
using Plugin.Permissions.Abstractions;
using Prism.Services;

namespace ToolBelt.Services
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> CheckPermissionsAsync(Permission permission, IPageDialogService dialogService);
    }
}