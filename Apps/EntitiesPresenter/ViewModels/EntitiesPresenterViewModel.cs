using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using EntitiesPresenter.DTOs;
using EntitiesPresenter.Interfaces;
using EntitiesPresenter.Models;
using Newtonsoft.Json;
using XDMessaging;

namespace EntitiesPresenter.ViewModels
{
    public class EntitiesPresenterViewModel : IEntitiesPresenterViewModel, INotifyPropertyChanged
    {
        private readonly IXDListener _listener;
        public ObservableCollection<EntityModel> EntitiesToShowInCanvas { get; set; }

        public EntitiesPresenterViewModel(XDMessagingClient client)
        {
            EntitiesToShowInCanvas = new ObservableCollection<EntityModel>();
            _listener = client.Listeners
                .GetListenerForMode(XDTransportMode.HighPerformanceUI);
            _listener.RegisterChannel("entityDetails");
            Subscribe();
        }

        public void Subscribe()
        {
            try
            {
                _listener.MessageReceived += (o, e) =>
                {
                    if (e.DataGram.Channel == "entityDetails")
                    {
                        EntityDetailsDto? receivedEntityDto = JsonConvert.DeserializeObject<EntityDetailsDto>(e.DataGram.Message);
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

                    }
                };
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
