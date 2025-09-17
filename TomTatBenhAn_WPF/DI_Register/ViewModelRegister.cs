using Microsoft.Extensions.DependencyInjection;
using TomTatBenhAn_WPF.ViewModel;
using TomTatBenhAn_WPF.ViewModel.ControlViewModel;
using TomTatBenhAn_WPF.ViewModel.PageViewModel;


namespace TomTatBenhAn_WPF.DI_Register
{
    public static class ViewModelRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddTransient<SideBarViewModel>();
            services.AddTransient<ContentViewModel>();
            services.AddTransient<HeaderViewModel>();
            services.AddTransient<TomTatBenhAnVM>();
            services.AddTransient<PhacDoVM>();
            services.AddTransient<SideBarNavigationViewModel>();
            services.AddTransient<BangKiemVM>();
            services.AddTransient<KiemTraPhacDoVM>();
            services.AddTransient<DashBoardVM>();
        }
    }
}
