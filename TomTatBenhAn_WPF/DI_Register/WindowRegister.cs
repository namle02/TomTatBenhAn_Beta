using Microsoft.Extensions.DependencyInjection;


namespace TomTatBenhAn_WPF.DI_Register
{
    public static class WindowRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
     
        }
    }
}
