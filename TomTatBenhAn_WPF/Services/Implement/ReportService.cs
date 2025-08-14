using System.IO;
using System.Text;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public class ReportService : IReportService
    {
        public string GetTemplateHtmlPath()
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(appDirectory, "Templates", "ReportTemplate.html");
        }

        public async Task<string> GenerateHtmlReportAsync(PatientAllData patientData)
        {
            try
            {
                var templatePath = GetTemplateHtmlPath();
                if (!File.Exists(templatePath))
                {
                    throw new FileNotFoundException($"Không tìm thấy file template: {templatePath}");
                }

                var htmlContent = await File.ReadAllTextAsync(templatePath, Encoding.UTF8);
                
                // Thay thế các bookmark với dữ liệu thực
                htmlContent = ReplaceBookmarksWithDataAsync(htmlContent, patientData);
                
                return htmlContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo HTML report: {ex.Message}", ex);
            }
        }

        private string ReplaceBookmarksWithDataAsync(string htmlContent, PatientAllData patientData)
        {
            var result = htmlContent;

            try
            {
                // Lấy thông tin hành chính (lấy record đầu tiên)
                var hanhChinh = patientData.ThongTinHanhChinh?.FirstOrDefault();
                var khamBenh = patientData.ThongTinKhamBenh?.FirstOrDefault();
                var chanDoan = patientData.ChanDoanIcd?.FirstOrDefault();
                var tomTat = patientData.ThongTinTomTat?.FirstOrDefault();
                var tinhTrangRaVien = patientData.TinhTrangNguoiBenhRaVien?.FirstOrDefault();

                // I. HÀNH CHÍNH
                result = result.Replace("{{TenBenhNhan}}", hanhChinh?.TenBN ?? "");
                result = result.Replace("{{NgaySinh}}", FormatDate(hanhChinh?.NgaySinh));
                result = result.Replace("{{Tuoi}}", hanhChinh?.Tuoi?.ToString() ?? "");
                result = result.Replace("{{GioiTinh}}", hanhChinh?.GioiTinh ?? "");
                result = result.Replace("{{DanToc}}", hanhChinh?.DanToc ?? "");
                result = result.Replace("{{DiaChi}}", hanhChinh?.DiaChi ?? "");
                result = result.Replace("{{BHYT}}", hanhChinh?.SoBHYT ?? "");
                result = result.Replace("{{CCCD}}", hanhChinh?.Cccd ?? "");
                result = result.Replace("{{SoBenhAn}}", hanhChinh?.SoBenhAn ?? "");
                result = result.Replace("{{MaYTe}}", hanhChinh?.MaYTe ?? "");
                result = result.Replace("{{VaoVien}}", FormatDateTime(hanhChinh?.NgayVaoVien));
                result = result.Replace("{{RaVien}}", FormatDateTime(hanhChinh?.NgayRaVien));

                // II. CHẨN ĐOÁN
                result = result.Replace("{{BenhChinhVaoVien}}", chanDoan?.BenhChinhVaoVien ?? "");
                result = result.Replace("{{IcdBenhChinhVaoVien}}", chanDoan?.MaICDChinhVaoVien ?? "");
                result = result.Replace("{{BenhPhuVaoVien}}", chanDoan?.BenhPhuVaoVien ?? "");
                result = result.Replace("{{IcdBenhPhuVaoVien}}", chanDoan?.MaICDPhuVaoVien ?? "");
                result = result.Replace("{{BenhChinhRaVien}}", chanDoan?.BenhChinhRaVien ?? "");
                result = result.Replace("{{IcdBenhChinhRaVien}}", chanDoan?.MaICDChinhRaVien ?? "");
                result = result.Replace("{{BenhPhuRaVien}}", chanDoan?.BenhKemTheoRaVien ?? "");
                result = result.Replace("{{IcdBenhPhuRaVien}}", chanDoan?.MaICDKemTheoRaVien ?? "");

                // III. TÓM TẮT QUÁ TRÌNH ĐIỀU TRỊ
                result = result.Replace("{{LiDoVaoVien}}", khamBenh?.LyDoVaoVien ?? "");
                result = result.Replace("{{AiQuaTrinh}}", tomTat?.TomTatQuaTrinhBenhLy ?? "");
                result = result.Replace("{{TienSuBenh}}", khamBenh?.TienSuBenh ?? "");
                result = result.Replace("{{DauHieuLamSang}}", tomTat?.TomTatDauHieuLamSang ?? "");
                result = result.Replace("{{KetQuaXetNghiemCLS}}", tomTat?.TomTatKetQuaXN ?? "");

                // Xử lý checkbox phương pháp điều trị
                result = ProcessTreatmentMethodCheckboxes(result, khamBenh, tinhTrangRaVien);

                result = result.Replace("{{PhuongPhapDieuTri}}", khamBenh?.HuongDieuTri ?? "");

                // Xử lý checkbox tình trạng ra viện  
                result = ProcessDischargeStatusCheckboxes(result, hanhChinh);

                result = result.Replace("{{TinhTrangRaVien}}", tomTat?.TomTatTinhTrangNguoiBenhRaVien ?? "");
                result = result.Replace("{{HuongDieuTriTiepTheo}}", tomTat?.TomTatHuongDieuTriTiepTheo ?? "");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thay thế bookmark: {ex.Message}", ex);
            }
        }

        private string ProcessTreatmentMethodCheckboxes(string content, 
            TomTatBenhAn_WPF.Repos._Model.PatientData.ThongTinKhamBenhModel? khamBenh,
            TomTatBenhAn_WPF.Repos._Model.PatientData.TinhTrangNguoiBenhRaVienModel? tinhTrang)
        {
            var result = content;

            // Xử lý checkbox Nội khoa
            var hasNoiKhoa = !string.IsNullOrWhiteSpace(khamBenh?.HuongDieuTri) && 
                            khamBenh.HuongDieuTri.ToLower().Contains("nội khoa");
            
            result = result.Replace("{{CheckBoxNoiKhoaTrue}}", hasNoiKhoa ? "checked" : "");
            result = result.Replace("{{CheckBoxNoiKhoaFalse}}", !hasNoiKhoa ? "checked" : "");
            result = result.Replace("{{LydoNoiKhoaTrue}}", hasNoiKhoa ? (khamBenh?.HuongDieuTri ?? "") : "");

            // Xử lý checkbox Phẫu thuật, thủ thuật
            var hasPTTT = !string.IsNullOrWhiteSpace(tinhTrang?.Ppdt) || 
                         (!string.IsNullOrWhiteSpace(khamBenh?.HuongDieuTri) && 
                          (khamBenh.HuongDieuTri.ToLower().Contains("phẫu thuật") || 
                           khamBenh.HuongDieuTri.ToLower().Contains("thủ thuật")));
            
            result = result.Replace("{{CheckBoxPTTTTrue}}", hasPTTT ? "checked" : "");
            result = result.Replace("{{CheckBoxPTTTFalse}}", !hasPTTT ? "checked" : "");
            result = result.Replace("{{LydoPTTTTrue}}", hasPTTT ? (tinhTrang?.Ppdt ?? khamBenh?.HuongDieuTri ?? "") : "");

            return result;
        }

        private string ProcessDischargeStatusCheckboxes(string content, 
            TomTatBenhAn_WPF.Repos._Model.PatientData.ThongTinHanhChinhModel? hanhChinh)
        {
            var result = content;
            var ketQua = hanhChinh?.KetQuaDieuTri?.ToLower() ?? "";

            // Reset tất cả checkbox trước
            result = result.Replace("{{IsCheckedKhoi}}", "");
            result = result.Replace("{{IsCheckedDo}}", "");
            result = result.Replace("{{IsCheckedKhongThayDoi}}", "");
            result = result.Replace("{{IsCheckedNangHon}}", "");
            result = result.Replace("{{IsCheckedTuVong}}", "");
            result = result.Replace("{{IsCheckedTienLuongNangXinVe}}", "");
            result = result.Replace("{{IsCheckedChuaXacDinh}}", "");

            // Set checkbox tương ứng
            switch (ketQua)
            {
                case var s when s.Contains("khỏi"):
                    result = result.Replace("{{IsCheckedKhoi}}", "checked");
                    break;
                case var s when s.Contains("đỡ"):
                    result = result.Replace("{{IsCheckedDo}}", "checked");
                    break;
                case var s when s.Contains("không thay đổi"):
                    result = result.Replace("{{IsCheckedKhongThayDoi}}", "checked");
                    break;
                case var s when s.Contains("nặng hơn"):
                    result = result.Replace("{{IsCheckedNangHon}}", "checked");
                    break;
                case var s when s.Contains("tử vong"):
                    result = result.Replace("{{IsCheckedTuVong}}", "checked");
                    break;
                case var s when s.Contains("tiên lượng nặng"):
                    result = result.Replace("{{IsCheckedTienLuongNangXinVe}}", "checked");
                    break;
                default:
                    result = result.Replace("{{IsCheckedChuaXacDinh}}", "checked");
                    break;
            }

            return result;
        }

        private string FormatDate(string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return "";

            if (DateTime.TryParse(dateString, out DateTime date))
            {
                return date.ToString("dd/MM/yyyy");
            }

            return dateString;
        }

        private string FormatDateTime(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return "";

            return dateTime.Value.ToString("dd/MM/yyyy HH:mm");
        }
    }
}
