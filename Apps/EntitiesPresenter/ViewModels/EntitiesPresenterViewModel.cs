using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using EntitiesPresenter.DTOs;
using EntitiesPresenter.Interfaces;
using EntitiesPresenter.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace EntitiesPresenter.ViewModels
{
    public class EntitiesPresenterViewModel : IEntitiesPresenterViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<EntityModel> EntitiesToShowInCanvas { get; set; }

        public EntitiesPresenterViewModel()
        {
            EntitiesToShowInCanvas = new ObservableCollection<EntityModel>();
            Subscribe();
        }

        public async void Subscribe()
        {
            try
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5003/MapEntitiesHub")
                    .Build();

                await connection.StartAsync();


                connection.On<string>("MapPointAdded", (message) =>
                {
                    EntityDetailsDto? receivedEntityDto = JsonConvert.DeserializeObject<EntityDetailsDto>(message);
                    if (receivedEntityDto != null)
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
                    else
                    {
                        Console.WriteLine("Was not able to deserialize entity details");
                    }
                });
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

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
