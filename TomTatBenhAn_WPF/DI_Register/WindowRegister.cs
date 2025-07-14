using Microsoft.Extensions.DependencyInjection;
using TomTatBenhAn_WPF.View.ControlView;


namespace TomTatBenhAn_WPF.DI_Register
{
    public static class WindowRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();

            services.AddSingleton<Sidebar>();
            services.AddSingleton<content>();
        }
    }
}
