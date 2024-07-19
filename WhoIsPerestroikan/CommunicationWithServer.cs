using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.Diagnostics;
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
            Trace.WriteLine("dans InitializeSignalR...");

            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://whoisperestroikan.azurewebsites.net/mappinhub")
                //.ConfigureLogging(logging => {
                //    logging.SetMinimumLevel(LogLevel.Information);
                //    logging.AddSerilog();
                //})
                .Build();

            hubConnection.On("ReceiveMapPin", onReceiveOneMapPin);
            hubConnection.On("HereAreAllMapPins", onReceiveAllMapPins);
            hubConnection.On<string>("test", message =>
            {
                Trace.WriteLine(message);
            });

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
