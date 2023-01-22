using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:60244/MapEntitiesHub")
    .Build();

await connection.StartAsync();
//connection.Closed += async _ =>
//{
//    await Task.Delay(new Random().Next(0, 5) * 1000);
//    await connection.StartAsync();
//};

connection.On<string>("ReceiveMessage", (message) =>
{
    Console.WriteLine($"Received Message: {message}");
});

Console.ReadLine();


