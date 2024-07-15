using System;
using System.Threading.Tasks;
using System.Threading;

public class LocationService
{
    public string LocationStatus { get; set; }
    public CancellationTokenSource Cts { get; set; }

    public async Task<bool> RequestLocationPermission()
    {
        var result = await Permissions.RequestAsync<Permissions.LocationAlways>();
        return result == PermissionStatus.Granted;

    }

    public void CreateNewCancellationTokenSource()
    {
        Cts = new CancellationTokenSource();
    }

    public async Task<Location> GetLastKnownLocationAsync()
        => await Geolocation.GetLastKnownLocationAsync();

    public async Task<Location> GetLocationAsync()
        => await Geolocation.GetLocationAsync(new GeolocationRequest
        {
            DesiredAccuracy = GeolocationAccuracy.Best,
            Timeout = TimeSpan.FromSeconds(30)
        });

    public void StopLocationUpdates()
    {
        Cts?.Cancel();
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