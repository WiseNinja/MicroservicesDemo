using System;
using System.Threading.Tasks;

namespace EntitiesPresenter.Interfaces;

public interface IMapDataProvider
{
    public Task<string> GetMissionMapNameAsync();
    public Task<string> GetMissionMapDataAsync(string mapName);
}