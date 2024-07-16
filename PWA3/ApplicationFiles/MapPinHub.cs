using Microsoft.AspNetCore.SignalR;
using WhoIsPerestroikan;

namespace PWA3.ApplicationFiles
{
    public class MapPinHub : Hub
    {
        private static List<MapPin> mapPins = [];

        public async Task AddMapPin(MapPin mapPin)
        {
            mapPins.Add(mapPin);
            await Clients.All.SendAsync("ReceiveMapPin", mapPin);
        }

        public async Task<List<MapPin>> GetMapPins()
        {
            return mapPins;
        }
    }
}
