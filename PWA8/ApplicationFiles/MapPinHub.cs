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

        //await Clients.All.SendAsync("TestRetour", $"mapPin reçu: {mapPinDTO}");
        //await Clients.All.SendAsync("TestRetour", $"mapPin: {mapPinDTO?.Label ?? "null"}");

        var pinsCount = Datas.MapPins.Count;

        //todo faire mieux
        if (Datas.MapPins.All(p => p.Label != mapPinDTO.Label))
            Datas.MapPins.Add(mapPinDTO);

        await Clients.Caller.SendAsync("HereAreAllMapPins", Datas.MapPins);
        await TestAller($"{Datas.MapPins.Count} mapPinDTOs pour {mapPinDTO.Label}");

        if (Datas.MapPins.Count > pinsCount)
        {
            await Clients.Others.SendAsync("HereAreAllMapPins", Datas.MapPins);
            var msg = $"{Datas.MapPins.Count} mapPinDTOs pour ";
            msg += string.Join(',', Datas.MapPins.Select(pindto => pindto.Label));
            await TestAller(msg);
        }
    }

    public async Task GetMapPins()
    {
        await Clients.Caller.SendAsync("HereAreAllMapPins", Datas.MapPins);
        await TestAller($"Caller reçu {Datas.MapPins.Count} mapPinDTOs");
    }

    public async Task TestAller(string message)
    {
        await Clients.All.SendAsync("TestRetour", $"{message}!");
    }

    public async Task ClearPinDTOs()
    {
        await TestAller($"Reçu demande vidage");

        Datas.MapPins.Clear();
        await Clients.All.SendAsync("HereAreAllMapPins", Datas.MapPins);

        await TestAller($"Caller demandé vidage. Tous reçu {Datas.MapPins.Count} mapPinDTOs");
    }
}


