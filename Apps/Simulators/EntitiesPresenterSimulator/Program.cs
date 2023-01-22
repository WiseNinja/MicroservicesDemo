using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:50060/MapEntitiesHub")
    .Build();

await connection.StartAsync();


connection.On<string>("MapPointAdded", (message) =>
{
    Console.WriteLine($"Received new Map Point: {message}");
});

Console.ReadLine();


