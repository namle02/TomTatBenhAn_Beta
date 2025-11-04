using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
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
            services.AddSingleton<IFileServices, FileServices>();
            services.AddSingleton<IDataMapper, DataMapper>();
            services.AddSingleton<IConfigServices, ConfigServices>();
            services.AddSingleton<IDataMapper, DataMapper>();
            services.AddSingleton<IAiService,AiService>();
            services.AddSingleton<IPhacDoServices, PhacDoServices>();
            services.AddSingleton<IReportService, ReportService>();
            services.AddSingleton<IBenhNhanService, BenhNhanService>();
            services.AddSingleton<IBangKiemServices, BangKiemServices>();
            services.AddSingleton<IPhacDoReportServices, PhacDoReportServices>();

            services.AddSingleton(new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:3000"),
                Timeout = TimeSpan.FromSeconds(100)
            });

            services.AddSingleton<IKiemTraPhacDoServices, KiemTraPhacDoServices>();
            
            // Update service
            services.AddSingleton<IUpdateService, UpdateService>();
            
            // Register IServiceProvider factory để có thể inject vào ViewModels
            services.AddSingleton<IServiceProvider>(sp => sp);
        }
    }
}
