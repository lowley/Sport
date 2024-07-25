using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIPClient.Utils;

public class Geolocalisation : IGeolocalisation
{
    public CancellationTokenSource Cts { get; set; }
        = new CancellationTokenSource();

    private async Task<bool> RequestLocationPermissionAsync(bool? forcedResponse = null)
    {
        if (forcedResponse.HasValue)
            return forcedResponse.Value;

        var result = await Permissions.RequestAsync<Permissions.LocationAlways>();
        return result == PermissionStatus.Granted;
    }

    public async Task<Geolocalisation> VerifyPermission(bool? forcedPermissionAccepted = null)
    {
        var gotPermission = RequestLocationPermissionAsync(forcedPermissionAccepted).Result;
        if (forcedPermissionAccepted != true)
            throw new Exception("Geolocalisation Permission denied");
            
        return this;
    }

    public async Task<Location> GetLocationAsync(
        Location? forcedLocation = null,
        bool? forcedLocationPermission = null)
    {
        if (forcedLocation != null)
            return forcedLocation;

        if (!RequestLocationPermissionAsync(forcedLocationPermission).Result)
            throw new Exception("Geolocalisation Permission denied");

        return await Geolocation.GetLocationAsync(new GeolocationRequest
        {
            DesiredAccuracy = GeolocationAccuracy.Best,
            Timeout = TimeSpan.FromSeconds(30)
        }) ?? IGeolocalisation.GetNowhere();
    }

    public void StartService()
    {
        throw new NotImplementedException();
    }

    public void StopService()
    {
        Cts?.Cancel();
    }

    public Geolocalisation() { }
}

public static class Tools
{
    public static void UpdateWith(this Location oldLocation, Location newLocation)
    {
        oldLocation.Latitude = newLocation.Latitude;
        oldLocation.Longitude = newLocation.Longitude;
        oldLocation.Altitude = newLocation.Altitude;
        oldLocation.Accuracy = newLocation.Accuracy;
    }
}