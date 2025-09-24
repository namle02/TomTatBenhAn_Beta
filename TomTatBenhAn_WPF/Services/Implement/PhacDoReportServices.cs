using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Services.Interface;
using Word = Microsoft.Office.Interop.Word;



namespace TomTatBenhAn_WPF.Services.Implement
{
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
        public async Task<string?> CreateOutputFileWithDataAsync(string originalFilePath, string outputFilePath, BangKiemResponseDTO bangKiemData)
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

                await UpdateTableDataInWord(body, bangKiemData);

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
            var paragraph = cell.Elements<Paragraph>().FirstOrDefault();
            if (paragraph == null) return;
            paragraph.RemoveAllChildren();
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
            var paragraph = cell.Elements<Paragraph>().FirstOrDefault();
            if (paragraph == null) return;
            paragraph.RemoveAllChildren();
            paragraph.AppendChild(new Run(new Text(text ?? string.Empty)));
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
    }
}
