using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5003/MapEntitiesHub")
    .Build();

await connection.StartAsync();


connection.On<string>("MapPointAdded", (message) =>
{
    Console.WriteLine($"Received new Map Point: {message}");
});

Console.ReadLine();


