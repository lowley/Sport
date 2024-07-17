using Microsoft.AspNetCore.SignalR;
using WhoIsPerestroikan;

namespace PWA3.ApplicationFiles
{
    public class MapPinHub : Hub
    {
        private static List<MapPin> mapPins = [];

        public async Task AddMapPin(MapPin mapPin)
        {
            if (mapPin == null)
                return;

            var pinsCount = mapPins.Count;
            mapPins.Add(mapPin);
            
            if (mapPins.Count > pinsCount)
                await Clients.All.SendAsync("ReceiveMapPin", mapPin);
        }

        public async Task<List<MapPin>> GetMapPins()
        {
            return mapPins;
        }
    }
}
