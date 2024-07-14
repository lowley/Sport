using System;
using System.Threading.Tasks;
using System.Threading;

public class LocationService
{
    public CancellationTokenSource _cts { get; set; }

    public async Task StartLocationUpdatesAsync()
    {
        _cts = new CancellationTokenSource();

        try
        {
            await Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });

                    if (location != null)
                    {
                        // Traitement de la localisation
                        Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                    }

                    // Attendez un certain temps avant de demander une nouvelle localisation
                    await Task.Delay(TimeSpan.FromMinutes(1), _cts.Token);
                }
            }, _cts.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public void StopLocationUpdates()
    {
        _cts?.Cancel();
    }
}
