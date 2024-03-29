﻿using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using EntitiesPresenter.Interfaces;
using EntitiesPresenter.MapDataProviders;
using EntitiesPresenter.ViewModels;
using Microsoft.Extensions.Hosting;

namespace EntitiesPresenter;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }
    public App()
    {
        AppHost = Host
            .CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddScoped<EntitiesPresenterViewModel>();
                services.AddScoped<IMapDataProvider, HttpMapDataProvider>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();

        base.OnExit(e);
    }

}