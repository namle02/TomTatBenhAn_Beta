using Dapper;
using Microsoft.Data.SqlClient;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Repos._Model.PatientData;
using TomTatBenhAn_WPF.Repos._Model.PatientPhacDo;
using TomTatBenhAn_WPF.Repos._Model.PatientPhacDo.PatientAllDataPhacDo;
using TomTatBenhAn_WPF.Repos._Model.PatientTomTat.PatientData;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;
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

        public async Task<PatientPhacDoAllData> GetAllPatientPhacDoData(string SoBenhAn)
        {
            string connectionString = _configServices.Get("Db_String")!;
            connectionString += ";Connect Timeout=10";
            string NoiTruChamSocQuery = (_fileServices.GetQuery("PatientPhacDoData.NoiTruChamSoc.sql")).Replace("@SoBenhAn_Params", SoBenhAn);
            string NoiTruCLSQuery = (_fileServices.GetQuery("PatientPhacDoData.NoiTruCLS.sql")).Replace("@SoBenhAn_Params", SoBenhAn);
            string NoiTruKhamBenhQuery = (_fileServices.GetQuery("PatientPhacDoData.NoiTruKhamBenh.sql")).Replace("@SoBenhAn_Params", SoBenhAn);
            string NoiTruToaThuocQuery = (_fileServices.GetQuery("PatientPhacDoData.NoiTruToaThuoc.sql")).Replace("@SoBenhAn_Params", SoBenhAn);
            string ChanDoanIcdQuery = (_fileServices.GetQuery("ChanDoanICD.sql")).Replace("@SoBenhAn_Params", SoBenhAn);
            string ThongTinHanhChinhQuery = (_fileServices.GetQuery("ThongTinHanhChinh.sql")).Replace("@SoBenhAn_Params", SoBenhAn);
            string BenhAnTypeQuery = (_fileServices.GetQuery("BenhAnType.sql")).Replace("@SoBenhAn_Params", SoBenhAn);
            PatientPhacDoAllData patient = new PatientPhacDoAllData();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                patient.ChanDoanICD =  (await connection.QueryAsync<ChanDoanICD>(ChanDoanIcdQuery)).FirstOrDefault();
                patient.SoBenhAn = SoBenhAn;
                patient.NoiTruKhamBenh = (await connection.QueryAsync<NoiTruKhamBenh>(NoiTruKhamBenhQuery)).ToList();
                patient.NoiTruChamSoc = (await connection.QueryAsync<NoiTruChamSoc>(NoiTruChamSocQuery)).ToList();
                patient.NoiTruCLS = (await connection.QueryAsync<NoiTruCls>(NoiTruCLSQuery)).ToList();
                patient.NoiTruToaThuoc = (await connection.QueryAsync<NoiTruToaThuoc>(NoiTruToaThuocQuery)).ToList();
                patient.ThongTinHanhChinh = (await connection.QueryAsync<ThongTinHanhChinhModel>(ThongTinHanhChinhQuery)).FirstOrDefault();
                var loaiBenhAn = await connection.QueryFirstOrDefaultAsync<LoaiBenhAnModel>(BenhAnTypeQuery);
                if (loaiBenhAn != null)
                {
                    PatientAllData tempPatient = new PatientAllData
                    {
                        LoaiBenhAn = loaiBenhAn
                    };
                    await GetThongTinKhamBenh(tempPatient, connection);
                    patient.ThongTinKhamBenh = tempPatient.ThongTinKhamBenh;
                }
            }
            return patient;
        }

        public async Task<PatientAllData> GetAllPatientData(string SoBenhAn)
        {
            string connectionString = _configServices.Get("Db_String")!;
            connectionString += ";Connect Timeout=10";

            PatientAllData patient = new PatientAllData();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                foreach (DataPatientType type in Enum.GetValues(typeof(DataPatientType)))
                {
                    await GetQueriesByType(type, patient, SoBenhAn, connection);
                }
                await GetThongTinKhamBenh(patient, connection);

            }
            return patient;
        }


        public async Task<List<BenhAnIdModel>> GetBenhAnList(string MaYTe)
        {
            string connectionString = _configServices.Get("Db_String")!;
            string RawQuery = _fileServices.GetQuery("DanhSachSoBenhAn.sql");
            string query = RawQuery.Replace("@MaYTe_Params", MaYTe);
            List<BenhAnIdModel> SoBenhAnList = new List<BenhAnIdModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SoBenhAnList =  (await connection.QueryAsync<BenhAnIdModel>(query)).ToList();
            }
            return SoBenhAnList;
        }

        private async Task GetQueriesByType(DataPatientType dataType, PatientAllData patient, string soBenhAn, SqlConnection conn)
        {
            string RawQuery;
            string query;
            switch (dataType)
            {
                case DataPatientType.ThongTinHanhChinh:
                    RawQuery = _fileServices.GetQuery("ThongTinHanhChinh.sql");
                    query = RawQuery.Replace("@SoBenhAn_Params", soBenhAn);
                    patient.ThongTinHanhChinh = (await conn.QueryAsync<ThongTinHanhChinhModel>(query)).ToList();
                    if (patient.ThongTinHanhChinh[0].GioiTinh == "T")
                    {
                        patient.ThongTinHanhChinh[0].GioiTinh = "Nam";
                    }
                    else
                    {
                        patient.ThongTinHanhChinh[0].GioiTinh = "Nữ";
                    }
                    break;
                case DataPatientType.KetQuaXetNghiem:
                    RawQuery = _fileServices.GetQuery("KetQuaXetNghiemCLS.sql");
                    query = RawQuery.Replace("@SoBenhAn_Params", soBenhAn);
                    patient.KetQuaXetNghien = (await conn.QueryAsync<KetQuaXetNghiemModel>(query)).ToList();
                    break;
                case DataPatientType.ChanDoanICD:
                    RawQuery = _fileServices.GetQuery("ChanDoanICD.sql");
                    query = RawQuery.Replace("@SoBenhAn_Params", soBenhAn);
                    patient.ChanDoanIcd = (await conn.QueryAsync<ChanDoanICDModel>(query)).ToList();
                    foreach (var item in patient.ChanDoanIcd)
                    {
                        item.MaICDKemTheoRaVien = item.MaICDKemTheoRaVien?.Replace(";", "/");
                    }
                    break;
                case DataPatientType.DienBien:
                    RawQuery = _fileServices.GetQuery("DienBien.sql");
                    query = RawQuery.Replace("@SoBenhAn_Params", soBenhAn);
                    patient.TinhTrangNguoiBenhRaVien = (await conn.QueryAsync<TinhTrangNguoiBenhRaVienModel>(query)).ToList();
                    break;
                case DataPatientType.LoaiBenhAn:
                    RawQuery = _fileServices.GetQuery("BenhAnType.sql");
                    query = RawQuery.Replace("@SoBenhAn_Params", soBenhAn);
                    patient.LoaiBenhAn = await conn.QueryFirstAsync<LoaiBenhAnModel>(query);
                    break;
            }
        }

        private async Task GetThongTinKhamBenh(PatientAllData patient, SqlConnection conn)
        {
            if (patient?.LoaiBenhAn?.LoaiBenhAn_Id is null)
            {
                patient!.ThongTinKhamBenh = new List<ThongTinKhamBenhModel>();
                return;
            }

            string id = patient.LoaiBenhAn.LoaiBenhAn_Id;

                var sqlByType = new Dictionary<string, string>
            {
                ["1"] = "BenhAnTruyenNhiem.sql",
                ["2"] = "BenhAnNoiKhoa.sql",
                ["3"] = "BenhAnBong.sql",
                ["11"] = "BenhAnMat.sql",
                ["15"] = "BenhAnNgoaiKhoa.sql",
                ["16"] = "BenhAnNhiKhoa.sql",
                ["18"] = "BenhAnPhuKhoa.sql",
                ["19"] = "BenhAnRangHamMat.sql",
                ["20"] = "BenhAnSanKhoa.sql",
                ["21"] = "BenhAnSoSinh.sql",
                ["22"] = "BenhAnTaiMuiHong.sql",
                ["43"] = "BenhAnTamBenh.sql",
                ["50"] = "BenhAnUngBuou.sql",
                ["51"] = "BenhAnYHCT_NgoaiTruMoi.sql",
                ["54"] = "BenhAnYHCT_NoiTruMoi.sql",
                ["55"] = "BenhAnYHCT_NgoaiTruMoi.sql",
                ["61"] = "BenhAnPHCN_NoiTru.sql",
            };

            // Nếu chưa có file riêng thì dùng truy vấn chung
            string fileName = sqlByType.TryGetValue(id, out var f)
                ? f
                : "KhamBenhVaoVien.sql";

            string rawQuery;

            if (!string.IsNullOrWhiteSpace(patient.LoaiBenhAn.BenhAnTongQuat_Id))
            {
                // Có BenhAnTongQuat_Id → xử lý như cũ
                rawQuery = _fileServices.GetQuery($"BenhAnType.{fileName}");
                rawQuery = rawQuery.Replace("@ID", patient.LoaiBenhAn.BenhAnTongQuat_Id);
            }
            else
            {
                // Không có BenhAnTongQuat_Id → fallback sang TiepNhan_Id
                rawQuery = _fileServices.GetQuery("BenhAnType.KhamBenhVaoVien.sql");
                rawQuery = rawQuery.Replace("@TiepNhan_Id", patient.LoaiBenhAn.TiepNhan_id);
            }

            var data = await conn.QueryAsync<ThongTinKhamBenhModel>(rawQuery);
            patient.ThongTinKhamBenh = data.ToList();
        }

    }
}
