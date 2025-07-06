using Microsoft.Extensions.DependencyInjection;
using TomTatBenhAn_WPF.ViewModel;

namespace TomTatBenhAn_WPF.DI_Register
{
    public static class ViewModelRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
        }
    }
}
