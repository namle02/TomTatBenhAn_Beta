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
                string rawquery = _fileServices.GetQuery("HanhChinh.sql");
                string query = rawquery.Replace("@SoBenhAn_Params", SoBenhAn);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        // Gán dữ liệu từ sql vào model
                        ThongTinHanhChinh.TenBenhNhan = reader["TenBN"].ToString();
                        ThongTinHanhChinh.NgaySinh= reader["NgaySinh"] == DBNull.Value ? null : reader["NgaySinh"].ToString();
                        ThongTinHanhChinh.Tuoi = reader["Tuoi"] == DBNull.Value ? null : Convert.ToInt32( reader["Tuoi"]);
                        ThongTinHanhChinh.GioiTinh = reader["GioiTinh"] == DBNull.Value ? null : reader["GioiTinh"].ToString();
                        ThongTinHanhChinh.DiaChi = reader["DiaChi"] == DBNull.Value ? null : reader["DiaChi"].ToString();
                        ThongTinHanhChinh.BHYT = reader["SoBHYT"] == DBNull.Value ? null : reader["SoBHYT"].ToString();
                        ThongTinHanhChinh.DanToc = reader["DanToc"] == DBNull.Value ? null : reader["DanToc"].ToString();

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
