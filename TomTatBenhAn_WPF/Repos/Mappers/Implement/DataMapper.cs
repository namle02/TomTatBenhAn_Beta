using Microsoft.Data.SqlClient;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;
using TomTatBenhAn_WPF.Repos.Model;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.Repos.Mappers.Implement
{
    public class DataMapper : IDataMapper
    {
        private readonly IFileServices _fileServices;
        private readonly IConfigServices _configServices;

        public DataMapper(IFileServices fileServices, IConfigServices configServices)
        {
            _fileServices = fileServices;
            _configServices = configServices;
        }

        public async Task<HanhChinhModel> GetHanhChinhData(string SoBenhAn)
        {
            try
            {
                HanhChinhModel ThongTinHanhChinh = new HanhChinhModel();
                string connectionString = _configServices.Get("Db_String");
                string query = _fileServices.GetQuery("HanhChinh.sql");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        // Gán dữ liệu từ sql vào model
                    }
                    return ThongTinHanhChinh;
                }
                
            }
            catch
            {
                throw;
            }
        }
    }
}
