using Acr.UserDialogs;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using DeviceInfo = Xamarin.Forms;

namespace ToolBelt.Services
{
    public class PermissionsService : IPermissionsService
    {
        public async Task<PermissionStatus> CheckPermissionsAsync(Permission permission, IUserDialogs dialogService)
        {
            var title = $"{permission} Permission";
            var message = $"To use the feature the {permission} permission is required.";
            const string positive = "Settings";
            const string negative = "Maybe Later";

            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (DeviceInfo.Device.RuntimePlatform == DeviceInfo.Device.iOS)
                {
                    var iOSMessage = $"{message} Please go into Settings and turn on {permission} for the app.";
                    var task = dialogService?.ConfirmAsync(
                        new ConfirmConfig
                        {
                            Title = title,
                            Message = iOSMessage,
                            OkText = positive,
                            CancelText = negative
                        });
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
                    await dialogService.AlertAsync(
                        new AlertConfig
                        {
                            Title = title,
                            Message = $"To use this feature the {permission} permission is required.",
                            OkText = "OK"
                        });
                }

                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                if (!newStatus.ContainsKey(permission))
                {
                    return permissionStatus;
                }

                permissionStatus = newStatus[permission];

                if (permissionStatus != PermissionStatus.Granted)
                {
                    var task = dialogService?.ConfirmAsync(
                        new ConfirmConfig
                        {
                            Title = title,
                            Message = message,
                            OkText = positive,
                            CancelText = negative
                        });
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
