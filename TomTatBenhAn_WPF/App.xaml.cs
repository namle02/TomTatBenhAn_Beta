using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TomTatBenhAn_WPF.DI_Register;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider serviceProvider;
    
    public IServiceProvider ServiceProvider => serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        serviceProvider = services.BuildServiceProvider();

    }

    private void ConfigureServices(IServiceCollection services)
    {
        WindowRegister.Register(services);
        ServicesRegister.Register(services);
        ViewModelRegister.Register(services);
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        var configService = serviceProvider.GetRequiredService<IConfigServices>();
        await configService.GetConfigFromSheet();
        var mainwindow = serviceProvider.GetRequiredService<MainWindow>();

        mainwindow.Show();
        base.OnStartup(e);
    }
}

