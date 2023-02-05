using System;
using System.Net.Http;
using System.Threading.Tasks;
using EntitiesPresenter.Interfaces;

namespace EntitiesPresenter.MapDataProviders;

public class HttpMapDataProvider : IMapDataProvider
{
    public async Task<string> GetMissionMapNameAsync()
    {
        using HttpClient httpClient = new HttpClient();
        return await httpClient.GetStringAsync(new Uri($"http://localhost:5003/api/MapsRepository/GetMissionMapName"));
    }

    public async Task<string> GetMissionMapDataAsync(string? mapName)
    {
        using HttpClient httpClient = new HttpClient();
        return await httpClient.GetStringAsync(new Uri($"http://localhost:5003/api/MapsRepository/GetMapDataByMapName?mapName={mapName}"));
    }
}