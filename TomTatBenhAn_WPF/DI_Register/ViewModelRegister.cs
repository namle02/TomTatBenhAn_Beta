using Microsoft.Extensions.DependencyInjection;
using TomTatBenhAn_WPF.ViewModel;
using TomTatBenhAn_WPF.ViewModel.ControlViewModel;
//using TomTatBenhAn_WPF.ViewModel.PageViewModel;

namespace TomTatBenhAn_WPF.DI_Register
{
    public static class ViewModelRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<SideBarViewModel>();
            services.AddSingleton<ContentViewModel>();
            //services.AddTransient<PageViewModel>();
        }
    }
}
