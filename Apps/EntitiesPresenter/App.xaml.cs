using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using EntitiesPresenter.Interfaces;
using EntitiesPresenter.ViewModels;
using Microsoft.Extensions.Hosting;
using XDMessaging;

namespace EntitiesPresenter
{
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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<IEntitiesPresenterViewModel, EntitiesPresenterViewModel>();
                    services.AddTransient<XDMessagingClient>();
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
}
