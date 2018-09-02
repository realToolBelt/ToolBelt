using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Services;
using Xamarin.Forms;

namespace ToolBelt.Services
{
    public class PermissionsService : IPermissionsService
    {
        public async Task<PermissionStatus> CheckPermissionsAsync(Permission permission, IPageDialogService dialogService)
        {
            var title = $"{permission} Permission";
            var message = $"To use the feature the {permission} permission is required.";
            const string positive = "Settings";
            const string negative = "Maybe Later";

            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    var iOSMessage = $"{message} Please go into Settings and turn on {permission} for the app.";
                    var task = dialogService?.DisplayAlertAsync(title, iOSMessage, positive, negative);
                    if (task == null)
                    {
                        return permissionStatus;
                    }

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }

                    return permissionStatus;
                }

                request = true;
            }

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                // check if we should show a rationale for requesting permissions
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Photos))
                {
                    await dialogService.DisplayAlertAsync(title, $"To use this feature the {permission} permission is required.", "OK");
                }

                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                if (!newStatus.ContainsKey(permission))
                {
                    return permissionStatus;
                }

                permissionStatus = newStatus[permission];

                if (permissionStatus != PermissionStatus.Granted)
                {
                    var task = dialogService?.DisplayAlertAsync(title, message, positive, negative);
                    if (task == null)
                    {
                        return permissionStatus;
                    }

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }

                    return permissionStatus;
                }
            }

            return permissionStatus;
        }
    }
}
