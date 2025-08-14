using Microsoft.Extensions.DependencyInjection;
using TomTatBenhAn_WPF.View.ControlView;
using TomTatBenhAn_WPF.View.PageView;


namespace TomTatBenhAn_WPF.DI_Register
{
    public static class WindowRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();

            services.AddSingleton<TomTatBenhAnPage>();
            services.AddSingleton<PhacDoPage>();

            services.AddTransient<ReportPage>();
        }
    }
}
