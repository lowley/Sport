using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WhoIsPerestroikan;

namespace PWA6.ApplicationFiles;

public class Datas
{
    public static List<MapPinDTO> MapPins = [];
}

public class MapPinHub : Hub
{
    public async Task AddMapPin(MapPinDTO mapPinDTO)
    {
        if (mapPinDTO == null)
        {
            await Clients.All.SendAsync("TestRetour", $"mapPin reçu null");
            return;
        }

        await Clients.All.SendAsync("TestRetour", $"mapPin reçu: {mapPinDTO}");

        await Clients.All.SendAsync("TestRetour", $"mapPin: {mapPinDTO?.Label ?? "null"}");

        if (mapPinDTO == null)
            return;

        var pinsCount = Datas.MapPins.Count;

        if (Datas.MapPins.All(p => p.Id != mapPinDTO.Id))
            Datas.MapPins.Add(mapPinDTO);

        if (Datas.MapPins.Count > pinsCount)
            await Clients.All.SendAsync("ReceiveMapPin", mapPinDTO);
    }

    public async Task GetMapPins()
    {
        await Clients.Caller.SendAsync("HereAreAllMapPins", Datas.MapPins);
    }

    public async Task TestAller(string message)
    {
        await Clients.All.SendAsync("TestRetour", $"{message}!");
    }
}


