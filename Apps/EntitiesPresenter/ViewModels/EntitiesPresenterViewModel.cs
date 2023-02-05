using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using EntitiesPresenter.DTOs;
using EntitiesPresenter.Interfaces;
using EntitiesPresenter.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace EntitiesPresenter.ViewModels;

public class EntitiesPresenterViewModel : INotifyPropertyChanged
{
    private readonly IMapDataProvider _mapDataProvider;
    public ObservableCollection<EntityModel> EntitiesToShowInCanvas { get; set; }
    public Image? MissionMap { get; set; }

    public EntitiesPresenterViewModel(IMapDataProvider mapDataProvider)
    {
        _mapDataProvider = mapDataProvider;
        EntitiesToShowInCanvas = new ObservableCollection<EntityModel>();
        SetInitialMissionMap();
        Subscribe();
    }

    private async void SetInitialMissionMap()
    {
        string missionMapName = await _mapDataProvider.GetMissionMapNameAsync();
        if (!string.IsNullOrWhiteSpace(missionMapName))
        {
            await Application.Current.Dispatcher.Invoke(async () =>
            {
                MissionMap = await GetMissionMapImageAsync(missionMapName);
            });
            OnPropertyChanged(nameof(MissionMap));
        }
        else
        {
            MissionMap = new Image();
        }
    }

    public async void Subscribe()
    {
        try
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5003/MapEntitiesHub")
                .Build();

            connection.On<string>("MapPointAdded", (message) =>
            {
                try
                {
                    EntityDetailsDto? receivedEntityDto =
                        JsonConvert.DeserializeObject<EntityDetailsDto>(message);
                    if (receivedEntityDto != null)
                    {
                        AddNewEntityToMap(receivedEntityDto);
                    }
                    else
                    {
                        Console.WriteLine("Was not able to deserialize entity details");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while deserializing entity details, details: {ex}");
                }
            });
            connection.On<string>("MissionMapSet", async (message) =>
            {
                try
                {
                    MissionMapDto? receivedMissionMapDto = JsonConvert.DeserializeObject<MissionMapDto>(message);
                    if (receivedMissionMapDto != null)
                    {
                        await SetMainMissionMapAsync(receivedMissionMapDto);
                    }
                    else
                    {
                        Console.WriteLine("Was not able to deserialize mission map");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while deserializing mission map, details: {ex}");
                }
            });
            await connection.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred during entities processing, details:{ex}");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #region Private Methods
    private void AddNewEntityToMap(EntityDetailsDto receivedEntityDto)
    {
        EntityModel entityModel = new EntityModel
        {
            Name = receivedEntityDto.Name,
            X = receivedEntityDto.X,
            Y = receivedEntityDto.Y
        };
        Application.Current.Dispatcher.Invoke(() => EntitiesToShowInCanvas.Add(entityModel));
        OnPropertyChanged(nameof(EntitiesToShowInCanvas));
    }

    private async Task SetMainMissionMapAsync(MissionMapDto receivedMissionMapDto)
    {
        await Application.Current.Dispatcher.Invoke(async () =>
        {
            MissionMap = await GetMissionMapImageAsync(receivedMissionMapDto.MissionMapName);
            EntitiesToShowInCanvas.Clear();
        });
        OnPropertyChanged(nameof(MissionMap));
        OnPropertyChanged(nameof(EntitiesToShowInCanvas));
    }

    private async Task<Image> GetMissionMapImageAsync(string? missionMapName)
    {
        Image missionMapImage = new Image();
        string missionMapData = await _mapDataProvider.GetMissionMapDataAsync(missionMapName);
        byte[] missionMapDataBinary = Convert.FromBase64String(missionMapData);
        BitmapImage missionMapBitmap = new BitmapImage();
        missionMapBitmap.BeginInit();
        missionMapBitmap.StreamSource = new MemoryStream(missionMapDataBinary);
        missionMapBitmap.EndInit();
        missionMapImage.Source = missionMapBitmap;
        return missionMapImage;
    } 
    #endregion


}