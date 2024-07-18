using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.Linq;

namespace WhoIsPerestroikan
{
    public class CommunicationWithServer
    {
        private HubConnection hubConnection { get; set; }
        private Logger Logger { get; set; }

        public async Task InitializeSignalR(
            Action<MapPin> onReceiveOneMapPin,
            Action<List<MapPin>> onReceiveAllMapPins)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri("http://peres-334945290772.herokuapp.com:52077/mappinhub"))
                .ConfigureLogging(logging => {
                    logging.SetMinimumLevel(LogLevel.Information);
                    logging.AddSerilog();
                })
                .Build();

            hubConnection.On("ReceiveMapPin", onReceiveOneMapPin);
            hubConnection.On("HereAreAllMapPins", onReceiveAllMapPins);

            await hubConnection.StartAsync();
        }

        public async Task AddMapPin(MapPin mapPin)
        {
            await hubConnection.InvokeAsync("AddMapPin", mapPin);
        }

        public async Task GetMapPins()
        {
            await hubConnection.InvokeAsync("GetMapPins");
        }

        public CommunicationWithServer(Logger logger) 
        {
            
        }
    }
}
