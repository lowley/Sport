using Microsoft.AspNetCore.SignalR.Client;

Console.Write("entrer un caractère");
Console.ReadLine();

//var connection = new HubConnectionBuilder().WithUrl("https://localhost:7044/mappinhub").Build();
var connection = new HubConnectionBuilder().WithUrl("https://whoisperestroikan.azurewebsites.net/mappinhub").Build();

connection.StartAsync().Wait();
connection.On<string>("TestRetour", message =>
{
    Console.WriteLine(message);
});

Console.ReadLine();




