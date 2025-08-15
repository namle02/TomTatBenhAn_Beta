using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Services.Interface;
using Word = Microsoft.Office.Interop.Word;
using System.IO;
using Microsoft.IdentityModel.Tokens;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public class ReportService : IReportService
    {
        public void PrintFileWord(string templateFilePath, PatientAllData patient)
        {
            Word.Application app = null;
            Word.Document doc = null;
            string outputFilePath = "";

            try
            {
                app = new Word.Application();
                app.Visible = false; // Ẩn Word application

                // Tạo đường dẫn thư mục lưu file theo tháng
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string currentMonth = DateTime.Now.Month.ToString();
                string baseDirectory = Path.Combine(desktopPath, "HoSoTomTat", $"Thang{currentMonth}");
                
                // Tạo thư mục nếu chưa tồn tại
                Directory.CreateDirectory(baseDirectory);

                // Tạo tên file với ReportNumber và SoBenhAn
                string reportNumber = patient.ReportNumber ?? "RPT";
                string soBenhAn = patient.ThongTinHanhChinh?[0]?.SoBenhAn ?? "Unknown";
                string fileName = $"{reportNumber}_{soBenhAn}.docx";
                
                outputFilePath = Path.Combine(baseDirectory, fileName);

                // Sao chép file template thành file mới
                File.Copy(templateFilePath, outputFilePath, true);

                // Mở file đã sao chép
                doc = app.Documents.Open(outputFilePath);

                // Tạo dictionary chứa tất cả dữ liệu cần thiết
                Dictionary<string, string> data = CreateBookmarkData(patient);

                // Thay thế dữ liệu vào các bookmarks với bảo toàn format
                foreach (var kvp in data)
                {
                    // Xử lý checkbox trước
                    ReplaceBookmarkText(doc, kvp.Key, kvp.Value ?? "");

                    // Sau đó xử lý bookmark text thông thường
                    if (doc.Bookmarks.Exists(kvp.Key))
                    {
                        Word.Bookmark bookmark = doc.Bookmarks[kvp.Key];
                        Word.Range range = bookmark.Range;

                        // Lưu format hiện tại
                        object font = range.Font.Name;
                        object fontSize = range.Font.Size;
                        object bold = range.Font.Bold;
                        object italic = range.Font.Italic;
                        object underline = range.Font.Underline;
                        object color = range.Font.Color;
                        object alignment = range.ParagraphFormat.Alignment;

                        // Thay thế text
                        range.Text = kvp.Value ?? "";

                        // Khôi phục format cho toàn bộ text mới
                        range.Font.Name = font.ToString();
                        range.Font.Size = (float)fontSize;
                        range.Font.Bold = (int)bold;
                        range.Font.Italic = (int)italic;
                        range.Font.Underline = (Word.WdUnderline)underline;
                        range.Font.Color = (Word.WdColor)color;
                        range.ParagraphFormat.Alignment = (Word.WdParagraphAlignment)alignment;

                        // Tạo lại bookmark sau khi thay thế text
                        doc.Bookmarks.Add(kvp.Key, range);
                    }
                }

                // Lưu file đã chỉnh sửa
                doc.Save();

                // Hiển thị Word để người dùng có thể xem và in
                app.Visible = true;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                throw new Exception($"Lỗi khi in file Word: {ex.Message}", ex);
            }
            finally
            {
                // Cleanup resources nếu có lỗi xảy ra
                if (doc != null && !app.Visible)
                {
                    doc.Close(false);
                    doc = null;
                }
                if (app != null && !app.Visible)
                {
                    app.Quit();
                    app = null;
                }
            }
        }

        private Dictionary<string, string> CreateBookmarkData(PatientAllData patient)
        {
            var data = new Dictionary<string, string>();

            // Thông tin hành chính
            if (patient.ThongTinHanhChinh != null && patient.ThongTinHanhChinh.Count > 0)
            {
                var hanhChinh = patient.ThongTinHanhChinh[0];
                data.Add("BN_SoBenhAn", hanhChinh.SoBenhAn ?? "");
                data.Add("BN_SoVaoVien", hanhChinh.SoVaoVien ?? "");
                data.Add("BN_CCCD", hanhChinh.Cccd ?? "");
                data.Add("BN_Ten", hanhChinh.TenBN ?? "");
                data.Add("BN_NgaySinh", hanhChinh.NgaySinh ?? "");
                data.Add("BN_Tuoi", hanhChinh.Tuoi?.ToString() ?? "");
                data.Add("BN_GioiTinh", hanhChinh.GioiTinh ?? "");
                data.Add("BN_DiaChi", hanhChinh.DiaChi ?? "");
                data.Add("BN_SoBHYT", hanhChinh.SoBHYT ?? "");
                data.Add("BN_NgayVaoVien", hanhChinh.NgayVaoVien?.ToString("dd/MM/yyyy") ?? "");
                data.Add("BN_NgayRaVien", hanhChinh.NgayRaVien?.ToString("dd/MM/yyyy") ?? "");
                data.Add("BN_DanToc", hanhChinh.DanToc ?? "");
                data.Add("BN_MaYTe", hanhChinh.MaYTe ?? "");
                data.Add("BN_ThoiGianVaoVien", hanhChinh.ThoiGianVaoVien ?? "");
                data.Add("BN_ThoiGianRaVien", hanhChinh.ThoiGianRaVien ?? "");
                data.Add("BN_KetQuaDieuTri", hanhChinh.KetQuaDieuTri ?? "");
            }

            // Thông tin khám bệnh
            if (patient.ThongTinKhamBenh != null && patient.ThongTinKhamBenh.Count > 0)
            {
                var khamBenh = patient.ThongTinKhamBenh[0];
                data.Add("KB_LyDoVaoVien", khamBenh.LyDoVaoVien ?? "");
                data.Add("KB_QuaTrinhBenhLy", khamBenh.QuaTrinhBenhLy ?? "");
                data.Add("KB_TienSuBenh", khamBenh.TienSuBenh ?? "");
                data.Add("KB_HuongDieuTri", khamBenh.HuongDieuTri ?? "");
            }

            // Chẩn đoán ICD
            if (patient.ChanDoanIcd != null && patient.ChanDoanIcd.Count > 0)
            {
                var chanDoan = patient.ChanDoanIcd[0];
                data.Add("CD_BenhChinhVaoVien", chanDoan.BenhChinhVaoVien ?? "");
                data.Add("CD_MaICDChinhVaoVien", chanDoan.MaICDChinhVaoVien ?? "");
                data.Add("CD_BenhPhuVaoVien", chanDoan.BenhPhuVaoVien ?? "");
                data.Add("CD_MaICDPhuVaoVien", chanDoan.MaICDPhuVaoVien ?? "");
                data.Add("CD_BenhChinhRaVien", chanDoan.BenhChinhRaVien ?? "");
                data.Add("CD_MaICDChinhRaVien", chanDoan.MaICDChinhRaVien ?? "");
                data.Add("CD_BenhKemTheoRaVien", chanDoan.BenhKemTheoRaVien ?? "");
                data.Add("CD_MaICDKemTheoRaVien", chanDoan.MaICDKemTheoRaVien ?? "");
            }

            // Tình trạng người bệnh ra viện
            if (patient.TinhTrangNguoiBenhRaVien != null && patient.TinhTrangNguoiBenhRaVien.Count > 0)
            {
                var tinhTrang = patient.TinhTrangNguoiBenhRaVien[0];
                data.Add("TT_DienBien", tinhTrang.DienBien ?? "");
                data.Add("TT_LoiDanThayThuoc", tinhTrang.LoiDanThayThuoc ?? "");
                data.Add("TT_PPDT", tinhTrang.Ppdt ?? "");
            }

            // Thông tin tóm tắt
            if (patient.ThongTinTomTat != null && patient.ThongTinTomTat.Count > 0)
            {
                var tomTat = patient.ThongTinTomTat[0];
                data.Add("TT_TomTatQuaTrinhBenhLy", tomTat.TomTatQuaTrinhBenhLy ?? "");
                data.Add("TT_TomTatDauHieuLamSang", tomTat.TomTatDauHieuLamSang ?? "");
                data.Add("TT_TomTatKetQuaXN", tomTat.TomTatKetQuaXN ?? "");
                data.Add("TT_TomTatTinhTrangNguoiBenhRaVien", tomTat.TomTatTinhTrangNguoiBenhRaVien ?? "");
                data.Add("TT_TomTatHuongDieuTriTiepTheo", tomTat.TomTatHuongDieuTriTiepTheo ?? "");
            }

            // Kết quả xét nghiệm (tổng hợp tất cả kết quả)
            if (patient.KetQuaXetNghien != null && patient.KetQuaXetNghien.Count > 0)
            {
                var ketQuaXN = string.Join("\n", patient.KetQuaXetNghien
                    .Where(x => !string.IsNullOrEmpty(x.KetQua))
                    .Select(x => $"{x.TenDichVu}: {x.KetQua} {x.MucBinhThuong}"));
                data.Add("XN_KetQuaTongHop", ketQuaXN);

                // Kết luận tổng hợp
                var ketLuan = string.Join("\n", patient.KetQuaXetNghien
                    .Where(x => !string.IsNullOrEmpty(x.KetLuan))
                    .Select(x => x.KetLuan));
                data.Add("XN_KetLuanTongHop", ketLuan);
            }

            // Thông tin báo cáo
            if (patient.DoctorName != null || patient.ReportNumber != null)
            {
                data.Add("ReportNumber", patient.ReportNumber ?? "");
                data.Add("DoctorName", patient.DoctorName ?? "");
            }

            // Thêm các trường thời gian hiện tại
            data.Add("NgayInBaoCao", DateTime.Now.ToString("dd/MM/yyyy"));
            data.Add("GioInBaoCao", DateTime.Now.ToString("HH:mm"));

            return data;
        }

        /// <summary>
        /// Xử lý thay thế bookmark text và tick checkbox
        /// </summary>
        private void ReplaceBookmarkText(Word.Document document, string bookmarkName, string newText)
        {
            // Xử lý trường hợp "BN_KetQuaDieuTri"
            if (bookmarkName == "BN_KetQuaDieuTri")
            {
                var resultBookmarkMapping = new Dictionary<string, string>
                {
                    { "Khỏi", "BN_Khoi" },
                    { "Đỡ", "BN_Do" },
                    { "Không thay đổi", "BN_KhongThayDoi" },
                    { "Tiên lượng nặng xin về", "BN_NangHonXinVe" },
                    { "Tử vong", "BN_TuVong" },
                    { "Chưa xác định", "BN_ChuaXacDinh" },
                    { "Nặng hơn", "BN_NangHon" }
                };

                foreach (var item in resultBookmarkMapping)
                {
                    foreach (Word.ContentControl control in document.ContentControls)
                    {
                        // Kiểm tra nếu Content Control là checkbox và có tag khớp
                        if (control.Type == Word.WdContentControlType.wdContentControlCheckBox &&
                            control.Tag == item.Value &&
                            item.Key == newText)
                        {
                            // Đánh dấu checkbox
                            control.Checked = true;
                        }
                    }
                }
                return;
            }

            // Thêm xử lý cho các trường checkbox khác nếu cần
            // Ví dụ: Giới tính
            if (bookmarkName == "KB_HuongDieuTri")
            {
                foreach (Word.ContentControl control in document.ContentControls)
                {
                    // Kiểm tra nếu Content Control là checkbox và có tag khớp
                    if (control.Type == Word.WdContentControlType.wdContentControlCheckBox &&
                        control.Tag == "PPDT_NoiKhoa" && !newText.IsNullOrEmpty())
                    {
                        // Đánh dấu checkbox
                        control.Checked = true;
                    }
                    else
                    {
                        if(control.Tag == "NoPPDT_NoiKhoa")
                        {
                            control.Checked = true;
                        }
                    }
                }

                return;

            }


        }

        /// <summary>
        /// Thêm phương thức để thay thế text với format nâng cao (dành cho text dài có xuống dòng)
        /// </summary>
        private void ReplaceBookmarkWithFormattedText(Word.Document doc, string bookmarkName, string text)
        {
            if (!doc.Bookmarks.Exists(bookmarkName)) return;

            Word.Bookmark bookmark = doc.Bookmarks[bookmarkName];
            Word.Range range = bookmark.Range;

            // Lưu tất cả formatting properties
            var formatting = new
            {
                FontName = range.Font.Name,
                FontSize = range.Font.Size,
                Bold = range.Font.Bold,
                Italic = range.Font.Italic,
                Underline = range.Font.Underline,
                Color = range.Font.Color,
                Alignment = range.ParagraphFormat.Alignment,
                LineSpacing = range.ParagraphFormat.LineSpacing,
                SpaceBefore = range.ParagraphFormat.SpaceBefore,
                SpaceAfter = range.ParagraphFormat.SpaceAfter,
                LeftIndent = range.ParagraphFormat.LeftIndent,
                RightIndent = range.ParagraphFormat.RightIndent
            };

            // Thay thế text (xử lý xuống dòng)
            range.Text = text?.Replace("\n", "\r") ?? "";

            // Áp dụng lại tất cả formatting
            range.Font.Name = formatting.FontName;
            range.Font.Size = formatting.FontSize;
            range.Font.Bold = formatting.Bold;
            range.Font.Italic = formatting.Italic;
            range.Font.Underline = formatting.Underline;
            range.Font.Color = formatting.Color;
            range.ParagraphFormat.Alignment = formatting.Alignment;
            range.ParagraphFormat.LineSpacing = formatting.LineSpacing;
            range.ParagraphFormat.SpaceBefore = formatting.SpaceBefore;
            range.ParagraphFormat.SpaceAfter = formatting.SpaceAfter;
            range.ParagraphFormat.LeftIndent = formatting.LeftIndent;
            range.ParagraphFormat.RightIndent = formatting.RightIndent;

            // Tạo lại bookmark
            doc.Bookmarks.Add(bookmarkName, range);
        }
    }
}
