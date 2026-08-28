using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Patrimonio.Financas.Wpf;
using Patrimonio.Wpf.Navigation;
using System.Windows;

namespace Patrimonio.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var baseAddress = context.Configuration.GetValue<string>("Api:BaseAddress")
                    ?? throw new InvalidOperationException("A configuração 'Api:BaseAddress' não foi encontrada.");

                services.ConfigureHttpClientDefaults(http =>
                {
                    http.ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new Uri(baseAddress);
                    });
                });

                services.AddFinancasModule();

                services.AddSingleton<AppNavigation>();
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        await _host.StartAsync();

        var window = _host.Services.GetRequiredService<MainWindow>();

        window.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }
}
