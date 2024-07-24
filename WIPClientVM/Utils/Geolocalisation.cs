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

    public async Task<bool> RequestLocationPermissionAsync(bool? forcedResponse = null)
    {
        if (forcedResponse.HasValue)
            return forcedResponse.Value;

        var result = await Permissions.RequestAsync<Permissions.LocationAlways>();
        return result == PermissionStatus.Granted;
    }

    public async Task<Location> GetLocationAsync(Location? forcedResponse = null)
    {
        if (forcedResponse != null)
            return forcedResponse;

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

    public Geolocalisation()
    {
        if (!RequestLocationPermissionAsync().Result)
            throw new Exception("Location permission denied");
    }
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