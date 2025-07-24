using Microsoft.Extensions.DependencyInjection;
using TomTatBenhAn_WPF.Repos.Mappers.Implement;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;
using TomTatBenhAn_WPF.Services.Implement;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.DI_Register
{
    public static class ServicesRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IFileServices, FileServices>();
            services.AddTransient<IDataMapper, DataMapper>();
            services.AddSingleton<IConfigServices, ConfigServices>();
            services.AddSingleton<IDataMapper, DataMapper>();
            services.AddSingleton<IAiService,AiService>();
            services.AddSingleton<ILoadingService, LoadingService>();


        }
    }
}
