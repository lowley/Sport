using System;
using System.Threading.Tasks;
using System.Threading;

public class LocationService
{
    public CancellationTokenSource _cts { get; set; }

    public async Task<bool> RequestLocationPermission()
    {
        var result = await Permissions.RequestAsync<Permissions.LocationAlways>();
        return result == PermissionStatus.Granted;

    }

    public void StopLocationUpdates()
    {
        _cts?.Cancel();
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