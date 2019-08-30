using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace ModuleLibrary.Geo
{
    public class Geo
    {
        public static async Task<double[]> GetPosition() {

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        //await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    var locator = CrossGeolocator.Current;

                    var position = await locator.GetPositionAsync();

                    return new[] { position.Latitude, position.Longitude };
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                    System.Diagnostics.Debug.WriteLine("Location Denied");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Location exception");
            }

            return new[] { 0.0, 0.0 };
        }
    }
}
