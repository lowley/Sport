using Microsoft.AspNetCore.SignalR;
using WhoIsPerestroikan;

namespace PWA4.ApplicationFiles;

public class Datas
{
    public static List<MapPin> MapPins = [];
}

public class MapPinHub : Hub
{
    public async Task AddMapPin(MapPin mapPin)
    {
        if (mapPin == null)
            return;

        var pinsCount = Datas.MapPins.Count;

        if(Datas.MapPins.All(p => p.Id != mapPin.Id))
            Datas.MapPins.Add(mapPin);

        if (Datas.MapPins.Count > pinsCount)
            await Clients.All.SendAsync("ReceiveMapPin", mapPin);
    }

    public async Task GetMapPins()
    {
        await Clients.Caller.SendAsync("HereAreAllMapPins", Datas.MapPins);
    }
}


