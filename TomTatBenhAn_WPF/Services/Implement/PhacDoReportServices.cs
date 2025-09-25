using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Services.Interface;
using TomTatBenhAn_WPF.Repos._Model.PatientPhacDo;
using Word = Microsoft.Office.Interop.Word;

namespace TomTatBenhAn_WPF.Services.Implement
{
    /// <summary>
    /// Class lưu trữ thống kê bảng kiểm
    /// </summary>
    public class BangKiemStatistics
    {
        public int TongTieuChi { get; set; }
        public int TieuChiCoDauSao { get; set; }
        public int TieuChiThuong { get; set; }
        public int TieuChiCoDauSaoDat { get; set; }
        public int TieuChiCoDauSaoDuocDanhGia { get; set; }
        public int TieuChiThuongDat { get; set; }
        public int TieuChiThuongDuocDanhGia { get; set; }
        public double PhanTramTieuChiThuongDat { get; set; }
        public bool IsDat { get; set; }
    }

    public class PhacDoReportServices : IPhacDoReportServices
    {
        #region Tạo bảng kiểm từ file word
        public BangKiemRequestDTO ExtractTableBangDanhGiaFromWord(string filePath, string phacDoId, string tenBangKiem)
        {
            var app = new Word.Application();
            Word.Document? doc = null;
            try
            {
                doc = app.Documents.Open(filePath, ReadOnly: true, Visible: false);
                var parsed = ParseFirstTableAsMatrix(doc!);
                var dto = MapMatrixToBangKiem(parsed, phacDoId, tenBangKiem);
                return dto;
            }
            finally
            {
                if (doc != null)
                {
                    doc.Close(false);
                }
                app.Quit(false);
            }
        }

        private static List<List<List<string>>> ParseFirstTableAsMatrix(Word.Document doc)
        {
            var result = new List<List<List<string>>>();
            if (doc.Tables.Count < 1) return result;
            var table = doc.Tables[1];

            // Duyệt qua toàn bộ ô để tránh lỗi khi có merge dọc
            int totalCols = table.Columns.Count;
            var cells = table.Range.Cells;
            int maxRow = 0;
            var map = new Dictionary<(int r, int c), string>();

            foreach (Word.Cell cell in cells)
            {
                int r = cell.RowIndex;
                int c = cell.ColumnIndex;
                if (r > maxRow) maxRow = r;
                string text = cell.Range.Text;
                text = text?.Replace("\r", string.Empty).Replace("\a", string.Empty).Trim() ?? string.Empty;
                map[(r, c)] = text;
            }

            for (int r = 1; r <= maxRow; r++)
            {
                var rowCells = new List<List<string>>();
                for (int c = 1; c <= totalCols; c++)
                {
                    map.TryGetValue((r, c), out var text);
                    rowCells.Add(new List<string> { text ?? string.Empty });
                }
                result.Add(rowCells);
            }

            return result;
        }

        private static BangKiemRequestDTO MapMatrixToBangKiem(List<List<List<string>>> matrix, string phacDoId, string tenBangKiem)
        {
            var dto = new BangKiemRequestDTO
            {
                PhacDoId = phacDoId,
                TenBangKiem = tenBangKiem
            };

            // Input matrix định dạng như output.json: mỗi hàng là mảng các cột (string)
            // Bỏ 2 hàng header đầu
            var dataRows = matrix.Skip(2).ToList();

            // Sẽ duyệt và gom theo Hạng mục (col 2), Nội dung (col 3)
            HangMucKiemTra? currentHangMuc = null;
            NoiDungKiemTra? currentNoiDung = null;

            foreach (var row in dataRows)
            {
                var cols = row.Select(c => (c.FirstOrDefault() ?? string.Empty).Trim()).ToList();
                // Đảm bảo đủ 9 cột theo ví dụ
                while (cols.Count < 9) cols.Add(string.Empty);

                var sttHangMuc = cols[0];
                var tenHangMuc = cols[1];
                var tenNoiDung = cols[2];
                var sttTieuChi = cols[3];
                var yeuCau = cols[4];
                var ketQuaDat = cols[5]; // "Đạt/ Có"
                var ketQuaKhongDat = cols[6]; // "Không đạt/ Không có"
                var khongAd = cols[7];
                var lyDo = cols[8];

                if (!string.IsNullOrWhiteSpace(sttHangMuc) && !string.IsNullOrWhiteSpace(tenHangMuc))
                {
                    // Bắt đầu hạng mục mới
                    currentHangMuc = new HangMucKiemTra
                    {
                        Stt = sttHangMuc.Trim(),
                        TenHangMucKiemTra = tenHangMuc.Trim()
                    };

                    // Gắn vào dto theo Stt 1..4
                    switch (currentHangMuc.Stt)
                    {
                        case "1": dto.DanhGiaVaChanDoan = currentHangMuc; break;
                        case "2": dto.DieuTri = currentHangMuc; break;
                        case "3": dto.ChamSoc = currentHangMuc; break;
                        case "4": dto.RaVien = currentHangMuc; break;
                    }
                    currentNoiDung = null;
                }

                if (!string.IsNullOrWhiteSpace(tenNoiDung))
                {
                    // Bắt đầu nội dung mới bên trong currentHangMuc
                    if (currentHangMuc == null)
                    {
                        // Nếu chưa có hạng mục, bỏ qua
                        continue;
                    }
                    currentNoiDung = new NoiDungKiemTra
                    {
                        TenNoiDungKiemTra = tenNoiDung.Trim()
                    };
                    currentHangMuc.DanhSachNoiDung.Add(currentNoiDung);
                }

                if (!string.IsNullOrWhiteSpace(sttTieuChi) || !string.IsNullOrWhiteSpace(yeuCau))
                {
                    if (currentNoiDung == null)
                    {
                        // Nếu thiếu nội dung, tạo nội dung rỗng tạm
                        currentNoiDung = new NoiDungKiemTra { TenNoiDungKiemTra = string.Empty };
                        currentHangMuc?.DanhSachNoiDung.Add(currentNoiDung);
                    }
                    var tieuChi = new TieuChiKiemTra
                    {
                        Stt = sttTieuChi.Trim(),
                        YeuCauDatDuoc = yeuCau.Trim(),
                        Dat = !string.IsNullOrWhiteSpace(ketQuaDat),
                        KhongDat = !string.IsNullOrWhiteSpace(ketQuaKhongDat),
                        KhongApDung = !string.IsNullOrWhiteSpace(khongAd),
                        LyDoKhongDat = lyDo?.Trim() ?? string.Empty,
                        IsImportant = sttTieuChi.Contains("*")
                    };
                    // Chuẩn hóa STT bỏ dấu * nếu có
                    tieuChi.Stt = tieuChi.Stt.Replace("*", string.Empty).Trim();
                    currentNoiDung.DanhSachTieuChi.Add(tieuChi);
                }
            }

            return dto;
        }

        #endregion

        #region Xuất bảng kiểm ra file word
        public async Task<string?> CreateOutputFileWithDataAsync(string originalFilePath, string outputFilePath, BangKiemResponseDTO bangKiemData, PatientPhacDoAllData? patientData = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(originalFilePath) || !File.Exists(originalFilePath))
                    throw new FileNotFoundException("Không tìm thấy file Word gốc", originalFilePath);

                // Copy file gốc sang output
                var outputDir = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrWhiteSpace(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                File.Copy(originalFilePath, outputFilePath, true);

                // Mở file output và cập nhật dữ liệu
                using var wordDocument = WordprocessingDocument.Open(outputFilePath, true);
                var body = wordDocument.MainDocumentPart?.Document?.Body;
                if (body == null) return null;

                // Cập nhật dữ liệu bảng
                await UpdateTableDataInWord(body, bangKiemData);

                // Cập nhật thông tin bệnh nhân và thống kê nếu có dữ liệu bệnh nhân
                if (patientData != null)
                {
                    await UpdatePatientInfoAndStatisticsInWord(body, bangKiemData, patientData);
                }

                wordDocument.Save();
                return outputFilePath;
            }
            catch
            {
                return null;
            }
        }

        private async Task UpdateTableDataInWord(Body body, BangKiemResponseDTO bangKiemData)
        {
            var tables = body.Elements<Table>().ToList();
            foreach (var table in tables)
            {
                await UpdateTableRows(table, bangKiemData);
            }
        }

        private async Task UpdateTableRows(Table table, BangKiemResponseDTO bangKiemData)
        {
            var rows = table.Elements<TableRow>().ToList();
            // Bỏ qua 2 hàng đầu (header). Hàng dữ liệu bắt đầu từ hàng thứ 3
            for (int i = 2; i < rows.Count; i++)
            {
                var row = rows[i];
                var cells = row.Elements<TableCell>().ToList();
                if (cells.Count < 6) continue;
                var firstCellText = GetCellText(cells[4]);

                await UpdateRowData(cells, firstCellText, bangKiemData);
            }
        }


        private async Task UpdateRowData(List<TableCell> cells, string firstCellText, BangKiemResponseDTO bangKiemData)
        {
            await System.Threading.Tasks.Task.Delay(1);
            var tieuChi = FindTieuChiKiemTra(firstCellText, bangKiemData);
            if (tieuChi == null) return;

            if (cells.Count >= 8)
            {
                UpdateCellWithMark(cells[5], tieuChi.Dat);
                UpdateCellWithMark(cells[6], tieuChi.KhongDat);
                UpdateCellWithMark(cells[7], tieuChi.KhongApDung);
                if (cells.Count >= 9)
                {
                    UpdateCellWithText(cells[8], tieuChi.LyDoKhongDat);
                }
            }
        }

        private static string GetCellText(TableCell cell)
        {
            try
            {
                var paragraphs = cell.Elements<Paragraph>();
                var texts = paragraphs.Select(p => p.InnerText).Where(t => !string.IsNullOrWhiteSpace(t));
                return string.Join(" ", texts).Trim();
            }
            catch { return string.Empty; }
        }

        private static void UpdateCellWithMark(TableCell cell, bool isMarked)
        {
            EnsureCellCentered(cell);
            var paragraph = cell.Elements<Paragraph>().FirstOrDefault();
            if (paragraph == null)
            {
                paragraph = new Paragraph();
                cell.Append(paragraph);
            }
            paragraph.RemoveAllChildren();
            CenterParagraph(paragraph);
            if (isMarked)
            {
                paragraph.AppendChild(new Run(new Text("X")));

            }
            else
            {
                paragraph.AppendChild(new Run(new Text("")));
            }
        }

        private static void UpdateCellWithText(TableCell cell, string text)
        {
            EnsureCellCentered(cell);
            var paragraph = cell.Elements<Paragraph>().FirstOrDefault();
            if (paragraph == null)
            {
                paragraph = new Paragraph();
                cell.Append(paragraph);
            }
            paragraph.RemoveAllChildren();
            CenterParagraph(paragraph);
            paragraph.AppendChild(new Run(new Text(text ?? string.Empty)));
        }

        // Helpers: center horizontally and vertically
        private static void EnsureCellCentered(TableCell cell)
        {
            var tcp = cell.GetFirstChild<TableCellProperties>();
            if (tcp == null)
            {
                tcp = new TableCellProperties();
                cell.PrependChild(tcp);
            }
            var vAlign = tcp.GetFirstChild<TableCellVerticalAlignment>();
            if (vAlign == null)
            {
                vAlign = new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center };
                tcp.Append(vAlign);
            }
            else
            {
                vAlign.Val = TableVerticalAlignmentValues.Center;
            }
        }

        private static void CenterParagraph(Paragraph paragraph)
        {
            var pp = paragraph.GetFirstChild<ParagraphProperties>();
            if (pp == null)
            {
                pp = new ParagraphProperties();
                paragraph.PrependChild(pp);
            }
            var just = pp.GetFirstChild<Justification>();
            if (just == null)
            {
                just = new Justification { Val = JustificationValues.Center };
                pp.Append(just);
            }
            else
            {
                just.Val = JustificationValues.Center;
            }
        }

        private static TieuChiKiemTra? FindTieuChiKiemTra(string cellText, BangKiemResponseDTO bangKiemData)
        {
            var cleanCellText = CleanTextForComparison(cellText);
            var allTieuChi = new List<TieuChiKiemTra>();
            allTieuChi.AddRange(bangKiemData.DanhGiaVaChanDoan.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            allTieuChi.AddRange(bangKiemData.DieuTri.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            allTieuChi.AddRange(bangKiemData.ChamSoc.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            allTieuChi.AddRange(bangKiemData.RaVien.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));

            foreach (var tc in allTieuChi)
            {
                var cleanTieuChiText = CleanTextForComparison(tc.YeuCauDatDuoc);
                if (cleanCellText.Equals(cleanTieuChiText, StringComparison.OrdinalIgnoreCase)) return tc;
                if (cleanCellText.Contains(cleanTieuChiText, StringComparison.OrdinalIgnoreCase) ||
                    cleanTieuChiText.Contains(cleanCellText, StringComparison.OrdinalIgnoreCase)) return tc;
            }
            return null;
        }

        private static string CleanTextForComparison(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            return System.Text.RegularExpressions.Regex.Replace(text.Trim(), "\\s+", " ")
                .Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");
        }

        public int CountUpdatedCriteria(BangKiemResponseDTO bangKiemData)
        {
            var allTieuChi = new List<TieuChiKiemTra>();
            allTieuChi.AddRange(bangKiemData.DanhGiaVaChanDoan.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            allTieuChi.AddRange(bangKiemData.DieuTri.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            allTieuChi.AddRange(bangKiemData.ChamSoc.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            allTieuChi.AddRange(bangKiemData.RaVien.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            return allTieuChi.Count(tc => tc.Dat || tc.KhongDat || tc.KhongApDung);
        }
        #endregion

        #region Thêm thông tin phụ cho bảng kiểm 

        /// <summary>
        /// Tính toán thống kê tiêu chí từ bảng kiểm đã đánh giá
        /// </summary>
        public BangKiemStatistics CalculateBangKiemStatistics(BangKiemResponseDTO bangKiemData)
        {
            var allTieuChi = GetAllTieuChiFromBangKiem(bangKiemData);
            
            var statistics = new BangKiemStatistics
            {
                TongTieuChi = allTieuChi.Count,
                TieuChiCoDauSao = allTieuChi.Count(tc => tc.IsImportant),
                TieuChiThuong = allTieuChi.Count(tc => !tc.IsImportant),
                TieuChiCoDauSaoDat = allTieuChi.Count(tc => tc.IsImportant && tc.Dat),
                TieuChiCoDauSaoDuocDanhGia = allTieuChi.Count(tc => tc.IsImportant && (tc.Dat || tc.KhongDat || tc.KhongApDung)),
                TieuChiThuongDat = allTieuChi.Count(tc => !tc.IsImportant && tc.Dat),
                TieuChiThuongDuocDanhGia = allTieuChi.Count(tc => !tc.IsImportant && (tc.Dat || tc.KhongDat || tc.KhongApDung))
            };

            // Tính phần trăm tiêu chí thường đạt
            if (statistics.TieuChiThuongDuocDanhGia > 0)
            {
                statistics.PhanTramTieuChiThuongDat = (double)statistics.TieuChiThuongDat / statistics.TieuChiThuongDuocDanhGia * 100;
            }

            // Xác định kết luận đạt/không đạt
            statistics.IsDat = statistics.TieuChiCoDauSaoDat == statistics.TieuChiCoDauSaoDuocDanhGia && 
                              statistics.TieuChiCoDauSaoDuocDanhGia > 0 &&
                              statistics.PhanTramTieuChiThuongDat > 90;

            return statistics;
        }

        /// <summary>
        /// Lấy tất cả tiêu chí từ bảng kiểm
        /// </summary>
        private List<TieuChiKiemTra> GetAllTieuChiFromBangKiem(BangKiemResponseDTO bangKiemData)
        {
            var allTieuChi = new List<TieuChiKiemTra>();
            allTieuChi.AddRange(bangKiemData.DanhGiaVaChanDoan.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            allTieuChi.AddRange(bangKiemData.DieuTri.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            allTieuChi.AddRange(bangKiemData.ChamSoc.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            allTieuChi.AddRange(bangKiemData.RaVien.DanhSachNoiDung.SelectMany(n => n.DanhSachTieuChi));
            return allTieuChi;
        }

        /// <summary>
        /// Cập nhật thông tin bệnh nhân và thống kê vào file Word
        /// </summary>
        public async Task UpdatePatientInfoAndStatisticsInWord(Body body, BangKiemResponseDTO bangKiemData, PatientPhacDoAllData patientData)
        {
            var statistics = CalculateBangKiemStatistics(bangKiemData);
            
            // Cập nhật các trường thông tin bệnh nhân
            await UpdatePatientFields(body, patientData);
            
            // Cập nhật thống kê và kết quả đánh giá
            await UpdateStatisticsFields(body, statistics);
            
            // Cập nhật kết luận
            await UpdateConclusionFields(body, statistics);
        }

        /// <summary>
        /// Cập nhật các trường thông tin bệnh nhân
        /// </summary>
        private async Task UpdatePatientFields(Body body, PatientPhacDoAllData patientData)
        {
            await System.Threading.Tasks.Task.Delay(1);
            
            var paragraphs = body.Elements<Paragraph>().ToList();
            
            foreach (var paragraph in paragraphs)
            {
                var text = GetParagraphText(paragraph);
                
                // Cập nhật họ tên người bệnh và số HSBA (cùng một dòng)
                if (text.Contains("Họ tên người bệnh:") && text.Contains("Số HSBA:"))
                {
                    UpdatePatientNameAndHSBA(paragraph, patientData);
                }
                
                // Cập nhật ngày vào viện, ngày ra viện, BS điều trị và ngày đánh giá (cùng một dòng)
                else if (text.Contains("Ngày vào viện:") && text.Contains("Ngày ra") && text.Contains("BS điều trị:") && text.Contains("Ngày đánh giá:"))
                {
                    UpdateDatesAndDoctor(paragraph, patientData);
                }
            }
        }

        /// <summary>
        /// Cập nhật họ tên người bệnh và số HSBA
        /// </summary>
        private void UpdatePatientNameAndHSBA(Paragraph paragraph, PatientPhacDoAllData patientData)
        {
            var fullText = GetParagraphText(paragraph);
            
            // Debug: Log thông tin để kiểm tra
            System.Diagnostics.Debug.WriteLine($"Original patient text: {fullText}");
            
            // Cập nhật họ tên người bệnh - chỉ thay thế phần sau dấu hai chấm và trước "Số HSBA:"
            var hoTenValue = patientData.ThongTinHanhChinh?.TenBN ?? "";
            var regexHoTen = new System.Text.RegularExpressions.Regex(@"Họ tên người bệnh:\s+");
            fullText = regexHoTen.Replace(fullText, $"Họ tên người bệnh: {hoTenValue} \t");
            
            // Cập nhật số HSBA - chỉ thay thế phần sau "Số HSBA:"
            var soHSBAValue = patientData.ThongTinHanhChinh?.SoBenhAn ?? "";
            var regexHSBA = new System.Text.RegularExpressions.Regex(@"Số HSBA:\s*$");
            fullText = regexHSBA.Replace(fullText, $"\tSố HSBA: {soHSBAValue}");
            
            System.Diagnostics.Debug.WriteLine($"Updated patient text: {fullText}");
            
            // Cập nhật paragraph với text mới
            paragraph.RemoveAllChildren();
            paragraph.AppendChild(new Run(new Text(fullText)));
        }

        /// <summary>
        /// Cập nhật ngày vào viện, ngày ra viện, BS điều trị và ngày đánh giá
        /// </summary>
        private void UpdateDatesAndDoctor(Paragraph paragraph, PatientPhacDoAllData patientData)
        {
            var fullText = GetParagraphText(paragraph);
            
            // Debug: Log thông tin để kiểm tra
            System.Diagnostics.Debug.WriteLine($"Original text: {fullText}");
            System.Diagnostics.Debug.WriteLine($"NgayVaoVien: {patientData.ThongTinHanhChinh?.NgayVaoVien}");
            System.Diagnostics.Debug.WriteLine($"NgayRaVien: {patientData.ThongTinHanhChinh?.NgayRaVien}");
            
            // Cập nhật ngày vào viện
            var ngayVaoVien = patientData.ThongTinHanhChinh?.NgayVaoVien?.ToString("dd/MM/yyyy") ?? "";
            if (!string.IsNullOrEmpty(ngayVaoVien))
            {
                // Debug: Kiểm tra regex pattern - pattern thực tế là "Ngày vào viện:  / /202"
                var regexVaoVien = new System.Text.RegularExpressions.Regex(@"Ngày vào viện:\s+/\s+/\s+202");
                var isMatch = regexVaoVien.IsMatch(fullText);
                System.Diagnostics.Debug.WriteLine($"Regex pattern match for NgayVaoVien: {isMatch}");
                System.Diagnostics.Debug.WriteLine($"Looking for pattern: 'Ngày vào viện:\\s+/\\s+/\\s+202'");
                System.Diagnostics.Debug.WriteLine($"In text: '{fullText}'");
                
                if (isMatch)
                {
                    fullText = regexVaoVien.Replace(fullText, $"Ngày vào viện: {ngayVaoVien}");
                    System.Diagnostics.Debug.WriteLine($"After updating NgayVaoVien: {fullText}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Pattern not found, trying alternative approach");
                    // Thử pattern khác: có thể có khoảng trắng khác nhau
                    var alternativePattern = new System.Text.RegularExpressions.Regex(@"Ngày vào viện:\s*/\s*/\s*202");
                    if (alternativePattern.IsMatch(fullText))
                    {
                        fullText = alternativePattern.Replace(fullText, $"Ngày vào viện: {ngayVaoVien}");
                        System.Diagnostics.Debug.WriteLine($"After alternative update NgayVaoVien: {fullText}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Still not found, trying exact match");
                        // Thử thay thế trực tiếp phần "  / /202"
                        fullText = fullText.Replace("  / /202", $" {ngayVaoVien}");
                        System.Diagnostics.Debug.WriteLine($"After direct replace NgayVaoVien: {fullText}");
                    }
                }
            }
            
            // Cập nhật ngày ra viện
            var ngayRaVien = patientData.ThongTinHanhChinh?.NgayRaVien?.ToString("dd/MM/yyyy") ?? "";
            if (!string.IsNullOrEmpty(ngayRaVien))
            {
                // Debug: Kiểm tra regex pattern - pattern thực tế là "Ngày ra   / /202"
                var regexRaVien = new System.Text.RegularExpressions.Regex(@"Ngày ra\s+/\s+/\s+202");
                var isMatch = regexRaVien.IsMatch(fullText);
                System.Diagnostics.Debug.WriteLine($"Regex pattern match for NgayRaVien: {isMatch}");
                System.Diagnostics.Debug.WriteLine($"Looking for pattern: 'Ngày ra\\s+/\\s+/\\s+202'");
                
                if (isMatch)
                {
                    fullText = regexRaVien.Replace(fullText, $"Ngày ra {ngayRaVien}");
                    System.Diagnostics.Debug.WriteLine($"After updating NgayRaVien: {fullText}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Pattern not found for NgayRaVien, trying alternative approach");
                    // Thử pattern khác: có thể có khoảng trắng khác nhau
                    var alternativePattern = new System.Text.RegularExpressions.Regex(@"Ngày ra\s*/\s*/\s*202");
                    if (alternativePattern.IsMatch(fullText))
                    {
                        fullText = alternativePattern.Replace(fullText, $"Ngày ra {ngayRaVien}");
                        System.Diagnostics.Debug.WriteLine($"After alternative update NgayRaVien: {fullText}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Still not found for NgayRaVien, trying exact match");
                        // Thử thay thế trực tiếp phần "   / /202"
                        fullText = fullText.Replace("   / /202", $" {ngayRaVien}");
                        System.Diagnostics.Debug.WriteLine($"After direct replace NgayRaVien: {fullText}");
                    }
                }
            }
            
            // Cập nhật BS điều trị (tạm thời để trống)
            var regexBS = new System.Text.RegularExpressions.Regex(@"BS điều trị:\s*;");
            var bacSiDieuTri = patientData.ThongTinHanhChinh?.BacSiDieuTri ?? "";

            fullText = regexBS.Replace(fullText, $"BS điều trị: {bacSiDieuTri};");
            
            // Cập nhật ngày đánh giá
            var ngayDanhGia = DateTime.Now.ToString("dd/MM/yyyy");
            var regexDanhGia = new System.Text.RegularExpressions.Regex(@"Ngày đánh giá:\s*$");
            fullText = regexDanhGia.Replace(fullText, $"Ngày đánh giá: {ngayDanhGia}");
            
            System.Diagnostics.Debug.WriteLine($"Final text: {fullText}");
            
            // Cập nhật paragraph với text mới
            paragraph.RemoveAllChildren();
            paragraph.AppendChild(new Run(new Text(fullText)));
        }



        /// <summary>
        /// Cập nhật các trường thống kê và kết quả đánh giá
        /// </summary>
        private async Task UpdateStatisticsFields(Body body, BangKiemStatistics statistics)
        {
            await System.Threading.Tasks.Task.Delay(1);
            
            var paragraphs = body.Elements<Paragraph>().ToList();
            
            foreach (var paragraph in paragraphs)
            {
                var text = GetParagraphText(paragraph);
                
                // Cập nhật tổng tiêu chí và phân loại (cùng một dòng)
                if (text.Contains("Tổng tiêu chí") && text.Contains("Số tiêu chí dấu* là") && text.Contains("Số tiêu chí thường áp dụng"))
                {
                    UpdateTotalCriteriaAndBreakdown(paragraph, statistics);
                }
                
                // Cập nhật kết quả đánh giá (cùng một dòng)
                else if (text.Contains("Kết quả:") && text.Contains("Số lượng tiêu chí có dấu * đạt") && text.Contains("số tiêu chí thường đạt"))
                {
                    UpdateEvaluationResults(paragraph, statistics);
                }
            }
        }

        /// <summary>
        /// Cập nhật tổng tiêu chí và phân loại
        /// </summary>
        private void UpdateTotalCriteriaAndBreakdown(Paragraph paragraph, BangKiemStatistics statistics)
        {
            var fullText = GetParagraphText(paragraph);
            
            // Cập nhật tổng tiêu chí
            var regexTotal = new System.Text.RegularExpressions.Regex(@"Tổng tiêu chí \d+");
            var newText = regexTotal.Replace(fullText, $"Tổng tiêu chí {statistics.TongTieuChi}");
            
            // Cập nhật số tiêu chí có dấu *
            var regexDauSao = new System.Text.RegularExpressions.Regex(@"Số tiêu chí dấu\* là \d+");
            newText = regexDauSao.Replace(newText, $"Số tiêu chí dấu* là {statistics.TieuChiCoDauSao}");
            
            // Cập nhật số tiêu chí thường áp dụng
            var regexThuong = new System.Text.RegularExpressions.Regex(@"Số tiêu chí thường áp dụng\d+");
            newText = regexThuong.Replace(newText, $"Số tiêu chí thường áp dụng {statistics.TieuChiThuong}");
            
            // Cập nhật paragraph với text mới
            paragraph.RemoveAllChildren();
            paragraph.AppendChild(new Run(new Text(newText)));
        }

        /// <summary>
        /// Cập nhật kết quả đánh giá
        /// </summary>
        private void UpdateEvaluationResults(Paragraph paragraph, BangKiemStatistics statistics)
        {
            var fullText = GetParagraphText(paragraph);
            
            // Cập nhật kết quả tiêu chí có dấu *
            var ketQuaDauSao = $"{statistics.TieuChiCoDauSaoDat}/{statistics.TieuChiCoDauSaoDuocDanhGia}";
            var regexDauSao = new System.Text.RegularExpressions.Regex(@"Số lượng tiêu chí có dấu \* đạt/tiêu chí \* được đánh giá:\s*;");
            var newText = regexDauSao.Replace(fullText, $"Số lượng tiêu chí có dấu * đạt/tiêu chí * được đánh giá: {ketQuaDauSao};\n");
            
            // Cập nhật kết quả tiêu chí thường
            var ketQuaThuong = $"{statistics.TieuChiThuongDat}/{statistics.TieuChiThuongDuocDanhGia}";
            var regexThuong = new System.Text.RegularExpressions.Regex(@"số tiêu chí thường đạt/tiêu chí được đánh giá:");
            newText = regexThuong.Replace(newText, $"Số tiêu chí thường đạt/tiêu chí được đánh giá: {ketQuaThuong}");
            
            // Cập nhật paragraph với text mới
            paragraph.RemoveAllChildren();
            paragraph.AppendChild(new Run(new Text(newText)));
        }

        /// <summary>
        /// Cập nhật kết luận đạt/không đạt
        /// </summary>
        private async Task UpdateConclusionFields(Body body, BangKiemStatistics statistics)
        {
            await System.Threading.Tasks.Task.Delay(1);
            
            var paragraphs = body.Elements<Paragraph>().ToList();
            
            foreach (var paragraph in paragraphs)
            {
                var text = GetParagraphText(paragraph);
                
                if (text.Contains("Kết luận") && text.Contains("khoanh tròn") && text.Contains("1) Đạt") && text.Contains("2) Không đạt"))
                {
                    UpdateConclusion(paragraph, statistics);
                }
            }
        }

        /// <summary>
        /// Cập nhật kết luận
        /// </summary>
        private void UpdateConclusion(Paragraph paragraph, BangKiemStatistics statistics)
        {
            var fullText = GetParagraphText(paragraph);
            
            // Xác định kết luận đúng
            var ketLuanDung = statistics.IsDat ? "1) Đạt" : "2) Không đạt";
            var ketLuanSai = statistics.IsDat ? "2) Không đạt" : "1) Đạt";
            
            // Thay thế kết luận đúng bằng kết luận có dấu ✓
            var newText = fullText.Replace(ketLuanDung, ketLuanDung + " ✓");
            
            // Cập nhật paragraph với text mới
            paragraph.RemoveAllChildren();
            paragraph.AppendChild(new Run(new Text(newText)));
        }

        /// <summary>
        /// Lấy text từ paragraph
        /// </summary>
        private string GetParagraphText(Paragraph paragraph)
        {
            try
            {
                var runs = paragraph.Elements<Run>();
                var texts = runs.Select(r => r.InnerText).Where(t => !string.IsNullOrWhiteSpace(t));
                return string.Join("", texts).Trim();
            }
            catch { return string.Empty; }
        }

        #endregion
    }
}
