using Microsoft.Data.SqlClient;
using TomTatBenhAn_WPF.Message;
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

        public async Task<List<LoadData>> GetSoBenhAnData(string maYTe)
        {
            try
            {
                List<LoadData> loadDatas = new();

                string connectionString = _configServices.Get("Db_String");
                string rawquery = _fileServices.GetQuery("SoBenhAn.sql");
                string query = rawquery.Replace("@SoVaoVien_Params", maYTe);

                using SqlConnection connection = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand(query, connection);
                await connection.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    string? soBenhAnValue = reader["SoBenhAn"]?.ToString();

                    if (!string.IsNullOrWhiteSpace(soBenhAnValue))
                    {
                        var data = new LoadData(soBenhAnValue, maYTe);
                        loadDatas.Add(data);
                    }
                }

                return loadDatas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu số bệnh án: {ex.Message}", ex);
            }
        }
        
        public async Task<ChuanDoanModel> GetChuanDoanData(string SoBenhAn)
        {
            try
            {
                ChuanDoanModel ThongTinchuanDoan = new ChuanDoanModel();
                string connectionString = _configServices.Get("Db_String");
                string rawquery = _fileServices.GetQuery("chuandoanravaovien.sql");
                string query = rawquery.Replace("@SoBenhAn_Params", SoBenhAn);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        // Gán dữ liệu từ sql vào model
                        ThongTinchuanDoan.benhChinhVaoVien = reader["ChanDoanVaoKhoaBenhChinh"] == DBNull.Value ? null : reader["ChanDoanVaoKhoaBenhChinh"].ToString();

                        ThongTinchuanDoan.benhPhuVaoVien = reader["ICDVaoKhoa_TenPhu"] == DBNull.Value ? null : reader["ICDVaoKhoa_TenPhu"].ToString();
                        ThongTinchuanDoan.icdVaoKhoaChinh = reader["ICDVaoKhoa_Chinh"] == DBNull.Value ? null : reader["ICDVaoKhoa_Chinh"].ToString();
                        ThongTinchuanDoan.icdVaoKhoaPhu = reader["ICDVaoKhoa_Phu"] == DBNull.Value ? null : reader["ICDVaoKhoa_Phu"].ToString();

                        ThongTinchuanDoan.benhChinhRaVien = reader["ChanDoanRaVienBenhChinh"] == DBNull.Value ? null : reader["ChanDoanRaVienBenhChinh"].ToString();
                        ThongTinchuanDoan.benhPhuRaVien = reader["ChanDoanRaVienBenhPhu"] == DBNull.Value ? null : reader["ChanDoanRaVienBenhPhu"].ToString();
                        ThongTinchuanDoan.icdRaVienChinh = reader["ICDRaVien_Chinh"] == DBNull.Value ? null : reader["ICDRaVien_Chinh"].ToString();
                        ThongTinchuanDoan.icdRaVienPhu = reader["ICDRaVien_Phu"] == DBNull.Value ? null : reader["ICDRaVien_Phu"].ToString();
                    }
                    return ThongTinchuanDoan;
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task<ThongTinBenhNhan> GetThongTinBenhNhanData(string SoBenhAn)
        {
            try
            {
                ThongTinBenhNhan ThongTinBenhNhan = new ThongTinBenhNhan();
                string connectionString = _configServices.Get("Db_String");
                string rawquery = _fileServices.GetQuery("ThongTinBenhNhan.sql");
                string query = rawquery.Replace("@SoBenhAn_Params", SoBenhAn);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        // Gán dữ liệu từ sql vào model
                        ThongTinBenhNhan.TenBenhNhan = reader["TenBN"].ToString();
                        ThongTinBenhNhan.NgaySinh = reader["NgaySinh"] == DBNull.Value ? null : reader["NgaySinh"].ToString();
                        ThongTinBenhNhan.Tuoi = reader["Tuoi"] == DBNull.Value ? null : Convert.ToInt32(reader["Tuoi"]);
                        var gioiTinhValue = reader["GioiTinh"] == DBNull.Value ? null : reader["GioiTinh"].ToString();
                        if (gioiTinhValue == "T")
                        {
                            gioiTinhValue = "Nam";
                        }
                        else if (gioiTinhValue == "G")
                        {
                            gioiTinhValue = "Nữ";
                        }
                        ThongTinBenhNhan.GioiTinh = gioiTinhValue;
                        ThongTinBenhNhan.DiaChi = reader["DiaChi"] == DBNull.Value ? null : reader["DiaChi"].ToString();
                        ThongTinBenhNhan.BHYT = reader["SoBHYT"] == DBNull.Value ? null : reader["SoBHYT"].ToString();
                        ThongTinBenhNhan.DanToc = reader["DanToc"] == DBNull.Value ? null : reader["DanToc"].ToString();
                        ThongTinBenhNhan.MaYTe = reader["MaYTe"] == DBNull.Value ? null : reader["MaYTe"].ToString();
                        ThongTinBenhNhan.SoBenhAn = reader["SoBenhAn"] == DBNull.Value ? null : reader["SoBenhAn"].ToString();


                    }
                    return ThongTinBenhNhan;
                }

            }
            catch
            {
                throw;
            }
        }
        public async Task<BenhAnTypeModel> GetBenhAnTypeData(string SoBenhAn)
        {
            try
            {
                BenhAnTypeModel benhAnType = new BenhAnTypeModel();
                string connectionString = _configServices.Get("Db_String");
                string rawquery = _fileServices.GetQuery("BenhAnType.sql");
                string query = rawquery.Replace("@SoBenhAn_Params", SoBenhAn);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        // Gán dữ liệu từ sql vào model

                        benhAnType.LoaiBenhAn = reader["LoaiBenhAn"] == DBNull.Value ? null : reader["LoaiBenhAn"].ToString();
                        benhAnType.BenhAnTongQuatId = reader["BenhAnTongQuat_Id"] == DBNull.Value ? null : reader["BenhAnTongQuat_Id"].ToString();
                        benhAnType.TiepNhanId = reader["TiepNhan_Id"] == DBNull.Value ? null : reader["TiepNhan_Id"].ToString();

                    }
                    return benhAnType;
                }

            }
            catch
            {
                throw;
            }
        }


        public async Task<BenhAnChiTietModel> GetBenhAnChiTietAsync(string loaiBenhAn, string benhAnTongQuatId, string tiepNhanId)
        {
            try
            {
                string sqlQuery = string.Empty;

                // 🔹 Trường hợp có LoaiBenhAn và BenhAnTongQuatId
                if (!string.IsNullOrWhiteSpace(loaiBenhAn) && !string.IsNullOrWhiteSpace(benhAnTongQuatId))
                {
                    if (!QueryStorage.Instance.Storage.TryGetValue(loaiBenhAn.ToLower(), out object? rawQueryObj))
                        throw new Exception($"Không tìm thấy câu truy vấn cho loại bệnh án: {loaiBenhAn}");

                    sqlQuery = rawQueryObj?.ToString()?.Replace("@ID", benhAnTongQuatId);
                }
                else
                {
                    // 🔹 Trường hợp fallback: Dùng TiepNhanId thay cho @TiepNhan_Id
                    if (string.IsNullOrWhiteSpace(tiepNhanId))
                        throw new ArgumentException("Thiếu thông tin ID bệnh án hoặc tiếp nhận.");

                    if (!QueryStorage.Instance.Storage.TryGetValue("Khám bệnh vào viện", out object? rawQueryObj))
                        throw new Exception("Không tìm thấy câu truy vấn mặc định cho trường hợp dùng TiepNhan_Id.");
                

                    sqlQuery = rawQueryObj?.ToString()?.Replace("@TiepNhan_Id", tiepNhanId);
                }

                if (string.IsNullOrWhiteSpace(sqlQuery))
                    throw new Exception("Câu truy vấn SQL rỗng sau khi thay thế ID.");

                string connectionString = _configServices.Get("Db_String");

                BenhAnChiTietModel benhAnChiTiet = new BenhAnChiTietModel();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                benhAnChiTiet.LyDoVaoVien = reader["LyDoVaoVien"]?.ToString();
                                benhAnChiTiet.QuaTrinhBenhLy = reader["QuaTrinhBenhLy"]?.ToString();
                                benhAnChiTiet.TienSuBenh = reader["TienSuBenh"]?.ToString();
                                benhAnChiTiet.HuongDieuTri = reader["HuongDieuTri"]?.ToString();
                            }
                        }
                    }
                }

                return benhAnChiTiet;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết bệnh án: " + ex.Message, ex);
            }
        }



        public async Task<List<KetQuaXetNghiemCLSModel>> GetKetQuaXetNghiemModelData(string SoBenhAn)
        {
            try
            {
                List<KetQuaXetNghiemCLSModel> ketQuaXetNghiem = new();

                string connectionString = _configServices.Get("Db_String");
                string rawquery = _fileServices.GetQuery("KetQuaXetNghiemCLS.sql");
                string query = rawquery.Replace("@SoBenhAn_Params", SoBenhAn);

                using SqlConnection connection = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand(query, connection);
                await connection.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var ketQua = new KetQuaXetNghiemCLSModel
                    {
                        TenNhomDichVu = reader["TenNhomDichVu"] as string,
                        NoiDungChiTiet = reader["NoiDungChiTiet"] as string,
                        TenPhongBan = reader["TenPhongBan"] as string,
                        TenDichvu = reader["TenDichVu"] as string,
                        KetQua = reader["KetQua"] as string,
                        MucBinhThuong = reader["MucBinhThuong"] as string,
                        MucBinhThuongMin = reader["MucBinhThuongMin"] as string,
                        MucBinhThuongMax = reader["MucBinhThuongMax"] as string,
                        BatThuong = reader["BatThuong"] as string,
                        ThoiGianThucHIen = reader["ThoiGianThucHien"] as string,
                        KetLuan = reader["KetLuan"] as string,
                        MoTa = reader["MoTa_Text"] as string
                    };

                    ketQuaXetNghiem.Add(ketQua);
                }

                return ketQuaXetNghiem;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách kết quả xét nghiệm: {ex.Message}", ex);
            }
        }

        public async Task<HanhChinhModel> GetHanhChinhData(string SoBenhAn)
        {
            try
            {
                HanhChinhModel HanhChinh = new HanhChinhModel();
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

                        HanhChinh.ThoiGianVaoVien = reader["ThoiGianVaoVien"] == DBNull.Value ? null : reader["ThoiGianVaoVien"].ToString();
                        HanhChinh.ThoiGianRaVien = reader["ThoiGianRaVien"] == DBNull.Value ? null : reader["ThoiGianRaVien"].ToString();
                        HanhChinh.KetQuaDieuTri = reader["KetQuaDieuTri"] == DBNull.Value ? null : reader["KetQuaDieuTri"].ToString();

                    }
                    return HanhChinh;
                }

            }
            catch
            {
                throw;
            }
        }
        public Task<CheckBoxModel> UpdateCheckBoxesFromKetQuaDieuTri(string ketQuaDieuTri)
        {
            var model = new CheckBoxModel();

            if (string.IsNullOrWhiteSpace(ketQuaDieuTri))
                return Task.FromResult(model); // return model with all properties false

            ketQuaDieuTri = ketQuaDieuTri.Trim().ToLower(); // normalize input

            if (ketQuaDieuTri == "khỏi")
                model.IsCheckedKhoi = true;
            else if (ketQuaDieuTri == "đỡ" || ketQuaDieuTri == "do")
                model.IsCheckedDo = true;
            else if (ketQuaDieuTri == "không thay đổi" || ketQuaDieuTri == "khong thay doi")
                model.IsCheckedKhongThayDoi = true;
            else if (ketQuaDieuTri == "nặng hơn" || ketQuaDieuTri == "nang hon")
                model.IsCheckedNangHon = true;
            else if (ketQuaDieuTri == "tử vong" || ketQuaDieuTri == "tu vong")
                model.IsCheckedTuVong = true;
            else if (ketQuaDieuTri == "tiên lượng nặng, xin về" || ketQuaDieuTri == "tien luong nang xin ve")
                model.IsCheckedTienLuongNangXinVe = true;
            else if (ketQuaDieuTri == "chưa xác định" || ketQuaDieuTri == "chua xac dinh")
                model.IsCheckedChuaXacDinh = true;

            return Task.FromResult(model);
        }
        public async Task<List<DienBienModel>> GetDienBienData(string SoBenhAn)
        {
            try
            {
                List<DienBienModel> dienBienList = new();

                string connectionString = _configServices.Get("Db_String");
                string rawquery = _fileServices.GetQuery("DienBien.sql");
                string query = rawquery.Replace("@SoBenhAn_Params", SoBenhAn);

                using SqlConnection connection = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand(query, connection);
                await connection.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var dienBien = new DienBienModel
                    {
                        DienBien = reader["DienBien"]?.ToString(),
                        LoiDanThayThuoc = reader["LoiDanThayThuoc"]?.ToString()
                    };

                    dienBienList.Add(dienBien);
                }

                return dienBienList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu diễn biến: {ex.Message}", ex);
            }
        }
        
    }
}
