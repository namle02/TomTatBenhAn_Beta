using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Services.Interface;
using Word = Microsoft.Office.Interop.Word;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
using Microsoft.Win32;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public class ReportService : IReportService
    {
        private readonly IBenhNhanService _benhNhanService;

        public ReportService(IBenhNhanService benhNhanService)
        {
            _benhNhanService = benhNhanService;
        }

        /// <summary>
        /// Kiểm tra xem Microsoft Office (Word) có được cài đặt trên máy không
        /// </summary>
        private bool IsOfficeInstalled()
        {
            try
            {
                // Kiểm tra registry để xem Office có được cài đặt không
                string[] officeKeys = new string[]
                {
                    @"SOFTWARE\Microsoft\Office\16.0\Word",  // Office 2016/2019/365
                    @"SOFTWARE\Microsoft\Office\15.0\Word",  // Office 2013
                    @"SOFTWARE\Microsoft\Office\14.0\Word",  // Office 2010
                    @"SOFTWARE\WOW6432Node\Microsoft\Office\16.0\Word",
                    @"SOFTWARE\WOW6432Node\Microsoft\Office\15.0\Word",
                    @"SOFTWARE\WOW6432Node\Microsoft\Office\14.0\Word"
                };

                foreach (string keyPath in officeKeys)
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
                    {
                        if (key != null)
                        {
                            return true;
                        }
                    }
                }

                // Thử tạo instance Word để kiểm tra
                try
                {
                    var testApp = new Word.Application();
                    testApp.Quit(false);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        #region In bản tóm tắt ra file word
        public async void PrintFileWord(string templateFilePath, PatientAllData patient)
        {
            // Kiểm tra Office có được cài đặt không
            if (!IsOfficeInstalled())
            {
                MessageBox.Show(
                    "Microsoft Office (Word) chưa được cài đặt trên máy tính này.\n\n" +
                    "Vui lòng cài đặt Microsoft Office để sử dụng tính năng xuất file Word.",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            Word.Application app = null;
            Word.Document doc = null;
            string outputFilePath = "";

            try
            {
                // Thử tạo Word Application với xử lý lỗi cụ thể
                try
                {
                    app = new Word.Application();
                    app.Visible = false; // Ẩn Word application
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    // Kiểm tra xem có phải lỗi Office không
                    string errorMsg = ex.Message.ToLower();
                    if (errorMsg.Contains("office") || errorMsg.Contains("microsoft.office") || 
                        ex.FileName?.ToLower().Contains("office") == true)
                    {
                        MessageBox.Show(
                            "Không tìm thấy Microsoft Office (Word) trên máy tính này.\n\n" +
                            "Vui lòng cài đặt Microsoft Office để sử dụng tính năng xuất file Word.\n\n" +
                            $"Chi tiết lỗi: {ex.Message}",
                            "Lỗi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                        return;
                    }
                    throw; // Re-throw nếu không phải lỗi Office
                }
                catch (System.Runtime.InteropServices.COMException ex)
                {
                    MessageBox.Show(
                        "Không thể khởi tạo Microsoft Office (Word).\n\n" +
                        "Vui lòng đảm bảo Microsoft Office đã được cài đặt và đang hoạt động.\n\n" +
                        $"Chi tiết: {ex.Message}",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }
                catch (System.Reflection.ReflectionTypeLoadException ex)
                {
                    // Lỗi khi không load được type từ assembly
                    string officeError = ex.LoaderExceptions?
                        .FirstOrDefault(e => e?.Message?.ToLower().Contains("office") == true)?.Message ?? "";
                    
                    MessageBox.Show(
                        "Không thể tải Microsoft Office Interop assemblies.\n\n" +
                        "Vui lòng cài đặt Microsoft Office để sử dụng tính năng xuất file Word.\n\n" +
                        $"Chi tiết: {ex.Message}\n{(string.IsNullOrEmpty(officeError) ? "" : $"\n{officeError}")}",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }
                catch (Exception ex)
                {
                    // Kiểm tra inner exception
                    Exception? innerEx = ex.InnerException;
                    while (innerEx != null)
                    {
                        if (innerEx.Message.ToLower().Contains("office") || 
                            innerEx is System.IO.FileNotFoundException)
                        {
                            MessageBox.Show(
                                "Không tìm thấy Microsoft Office (Word) trên máy tính này.\n\n" +
                                "Vui lòng cài đặt Microsoft Office để sử dụng tính năng xuất file Word.\n\n" +
                                $"Chi tiết: {innerEx.Message}",
                                "Lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error
                            );
                            return;
                        }
                        innerEx = innerEx.InnerException;
                    }

                    MessageBox.Show(
                        $"Không thể khởi tạo Microsoft Office (Word).\n\n" +
                        $"Lỗi: {ex.Message}\n\n" +
                        "Vui lòng kiểm tra Microsoft Office đã được cài đặt đúng cách.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }

                if (app == null)
                {
                    MessageBox.Show(
                        "Không thể khởi tạo Microsoft Office (Word).\n\n" +
                        "Vui lòng đảm bảo Microsoft Office đã được cài đặt.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }

                // Tạo đường dẫn thư mục lưu file theo tháng
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string currentMonth = DateTime.Now.Month.ToString();
                string currentYear = DateTime.Now.Year.ToString();
                string baseDirectory = Path.Combine(desktopPath, "HoSoTomTat", $"Nam_{currentYear}", $"Thang_{currentMonth}");

                // Tạo thư mục nếu chưa tồn tại
                Directory.CreateDirectory(baseDirectory);

                // Tạo tên file với ReportNumber và SoBenhAn
                string reportNumber = patient.ReportNumber ?? "RPT";
                string soBenhAn = patient.ThongTinHanhChinh?[0]?.SoBenhAn ?? "Unknown";
                string fileName = $"{reportNumber}_{soBenhAn}_{patient.ThongTinHanhChinh![0].TenBN}.docx";

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
                    // 1) Hai khóa cần thụt toàn bộ block vào "1-2 ô"
                    if (kvp.Key == "TT_TomTatDauHieuLamSang" || kvp.Key == "TT_TomTatKetQuaXN")
                    {
                        SetBookmarkTextWithIndentChars(doc, kvp.Key, kvp.Value ?? "", charIndent: 2);
                        continue; // đã xử lý xong → bỏ qua luồng mặc định
                    }

                    // 2) Xử lý checkbox trước (nếu có logic checkbox trùng key)
                    ReplaceBookmarkText(doc, kvp.Key, kvp.Value ?? "");

                    // 3) Sau đó xử lý bookmark text thông thường (giữ nguyên format)
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

                        // Thay thế text (nếu text có xuống dòng, bạn có thể dùng ReplaceBookmarkWithFormattedText thay thế)
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

                //// Lưu thông tin bệnh nhân vào database sau khi xuất file thành công
                await SavePatientToDatabase(patient);
            }
            catch (System.IO.FileNotFoundException ex) when (ex.Message.Contains("office") || ex.Message.Contains("Office"))
            {
                // Xử lý lỗi khi Office không được tìm thấy
                MessageBox.Show(
                    "Không tìm thấy Microsoft Office (Word) trên máy tính này.\n\n" +
                    "Vui lòng cài đặt Microsoft Office để sử dụng tính năng xuất file Word.\n\n" +
                    $"Chi tiết lỗi: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                // Xử lý lỗi COM khi Office không khả dụng
                MessageBox.Show(
                    "Không thể kết nối với Microsoft Office (Word).\n\n" +
                    "Vui lòng đảm bảo Microsoft Office đã được cài đặt và đang hoạt động.\n\n" +
                    $"Chi tiết lỗi: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khác
                MessageBox.Show(
                    $"Lỗi khi xuất file Word: {ex.Message}\n\n" +
                    $"Loại lỗi: {ex.GetType().Name}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
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
                data.Add("KB_HuongDieuTriNoiKhoa", khamBenh.HuongDieuTri ?? "");
                data.Add("KB_HuongDieuTriPTTT", khamBenh.HuongDieuTri_PTTT);

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
            data.Add("NgayInBaoCao", $"Ngày {DateTime.Now.Day} Tháng {DateTime.Now.Month} Năm {DateTime.Now.Year}");
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

            // Ví dụ: xử lý huớng điều trị -> tick/no-tick theo dữ liệu
            if (bookmarkName == "KB_HuongDieuTriNoiKhoa")
            {
                foreach (Word.ContentControl control in document.ContentControls)
                {
                    // Kiểm tra nếu Content Control là checkbox và có tag khớp
                    if (control.Type == Word.WdContentControlType.wdContentControlCheckBox &&
                        control.Tag == "PPDT_NoiKhoa" && !newText.IsNullOrEmpty())
                    {
                        // Đánh dấu checkbox
                        control.Checked = true;
                        return;
                    }
                    else if (control.Type == Word.WdContentControlType.wdContentControlCheckBox &&
                        control.Tag == "NotPPDT_NoiKhoa" && newText.IsNullOrEmpty())
                    {
                        // Đánh dấu checkbox
                        control.Checked = true;
                        return;
                    }
                }

               
            }

            if (bookmarkName ==  "KB_HuongDieuTriPTTT")
            {
                foreach (Word.ContentControl control in document.ContentControls)
                {

                    // Kiểm tra nếu Content Control là checkbox và có tag khớp
                    if (control.Type == Word.WdContentControlType.wdContentControlCheckBox &&
                        control.Tag == "PPDT_PTTT" && !newText.IsNullOrEmpty())
                    {
                        // Đánh dấu checkbox
                        control.Checked = true;
                        return;
                    }
                    else if (control.Type == Word.WdContentControlType.wdContentControlCheckBox &&
                        control.Tag == "NotPPDT_PTTT" && newText.IsNullOrEmpty())
                    {
                        // Đánh dấu checkbox
                        control.Checked = true;
                        return;
                    }
                }
            }


        }


        // ***** Helper mới: set text + giữ format + thụt cả block theo "số ô" (ký tự) *****
        private void SetBookmarkTextWithIndentChars(Word.Document doc, string bookmarkName, string text, int charIndent = 2)
        {
            if (!doc.Bookmarks.Exists(bookmarkName)) return;

            Word.Bookmark bookmark = doc.Bookmarks[bookmarkName];
            Word.Range range = bookmark.Range;

            // Lưu format hiện tại
            var keep = new
            {
                Name = range.Font.Name,
                Size = range.Font.Size,
                Bold = range.Font.Bold,
                Italic = range.Font.Italic,
                Underline = range.Font.Underline,
                Color = range.Font.Color,
                Align = range.ParagraphFormat.Alignment,
                LineSpacing = range.ParagraphFormat.LineSpacing,
                SpaceBefore = range.ParagraphFormat.SpaceBefore,
                SpaceAfter = range.ParagraphFormat.SpaceAfter
            };

            // Gán text (chuyển \n -> \r để Word xuống dòng đúng)
            range.Text = (text ?? string.Empty).Replace("\n", "\r");

            // Thụt toàn bộ block vào N "ô"
            range.ParagraphFormat.CharacterUnitLeftIndent = charIndent;
            range.ParagraphFormat.CharacterUnitFirstLineIndent = 0;

            // Khôi phục các thuộc tính định dạng khác (giữ indent mới)
            range.Font.Name = keep.Name;
            range.Font.Size = keep.Size;
            range.Font.Bold = keep.Bold;
            range.Font.Italic = keep.Italic;
            range.Font.Underline = keep.Underline;
            range.Font.Color = keep.Color;
            range.ParagraphFormat.Alignment = keep.Align;
            range.ParagraphFormat.LineSpacing = keep.LineSpacing;
            range.ParagraphFormat.SpaceBefore = keep.SpaceBefore;
            range.ParagraphFormat.SpaceAfter = keep.SpaceAfter;

            // Re-add bookmark
            doc.Bookmarks.Add(bookmarkName, range);
        }

        #endregion

        #region Lưu bản tóm tắt vào cơ sở dữ liệu

        /// <summary>
        /// Lưu thông tin bệnh nhân vào database MongoDB
        /// </summary>
        /// <param name="patient">Thông tin bệnh nhân</param>
        public async Task SavePatientToDatabase(PatientAllData patient)
        {
            try
            {
                var result = await _benhNhanService.SaveBenhNhanAsync(patient);

                if (result.Success)
                {
                    MessageBox.Show("✅ Lưu thông tin bệnh nhân thành công!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"⚠️ Không thể lưu thông tin bệnh nhân: {result.Message}", "Cảnh báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"🛑 Lỗi khi lưu thông tin bệnh nhân: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
