// OpenXML
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Services.Interface;
using W2010 = DocumentFormat.OpenXml.Office2010.Word;

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

                result = ProcessTreatmentMethodCheckboxes(result, khamBenh, tinhTrangRaVien);
                result = result.Replace("{{PhuongPhapDieuTri}}", khamBenh?.HuongDieuTri ?? "");

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

            var hasNoiKhoa = !string.IsNullOrWhiteSpace(khamBenh?.HuongDieuTri) &&
                             khamBenh.HuongDieuTri.ToLower().Contains("nội khoa");

            result = result.Replace("{{CheckBoxNoiKhoaTrue}}", hasNoiKhoa ? "checked" : "");
            result = result.Replace("{{CheckBoxNoiKhoaFalse}}", !hasNoiKhoa ? "checked" : "");
            result = result.Replace("{{LydoNoiKhoaTrue}}", hasNoiKhoa ? (khamBenh?.HuongDieuTri ?? "") : "");

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

            result = result.Replace("{{IsCheckedKhoi}}", "");
            result = result.Replace("{{IsCheckedDo}}", "");
            result = result.Replace("{{IsCheckedKhongThayDoi}}", "");
            result = result.Replace("{{IsCheckedNangHon}}", "");
            result = result.Replace("{{IsCheckedTuVong}}", "");
            result = result.Replace("{{IsCheckedTienLuongNangXinVe}}", "");
            result = result.Replace("{{IsCheckedChuaXacDinh}}", "");

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

        public async Task<string> ExportDocxFromTemplateAsync(PatientAllData patient, string templatePath, string? outputPath = null)
        {
            await Task.Yield();

            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Không thấy template: {templatePath}");

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                var fileName = $"TomTatBenhAn_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                outputPath = Path.Combine(desktop, fileName);
            }

            File.Copy(templatePath, outputPath!, overwrite: true);

            using (var doc = WordprocessingDocument.Open(outputPath!, true))
            {
                var main = doc.MainDocumentPart!;
                var hanhChinh = patient.ThongTinHanhChinh?.FirstOrDefault();
                var khamBenh = patient.ThongTinKhamBenh?.FirstOrDefault();
                var chanDoan = patient.ChanDoanIcd?.FirstOrDefault();
                var tomTat = patient.ThongTinTomTat?.FirstOrDefault();
                var tinhTrang = patient.TinhTrangNguoiBenhRaVien?.FirstOrDefault();

                SetBookmarkText(main, "BN_Name", hanhChinh?.TenBN);
                SetBookmarkText(main, "BN_Bd", FormatDate(hanhChinh?.NgaySinh));
                SetBookmarkText(main, "BN_Age", hanhChinh?.Tuoi?.ToString());
                SetBookmarkText(main, "BN_Sex", hanhChinh?.GioiTinh);
                SetBookmarkText(main, "BN_DanToc", hanhChinh?.DanToc);
                SetBookmarkText(main, "BN_Addr", hanhChinh?.DiaChi);
                SetBookmarkText(main, "BN_Bhyt", hanhChinh?.SoBHYT);
                SetBookmarkText(main, "BN_CCCD", hanhChinh?.Cccd);
                SetBookmarkText(main, "BN_SoBenhAn", hanhChinh?.SoBenhAn);
                SetBookmarkText(main, "BN_MaYTe", hanhChinh?.MaYTe);
                SetBookmarkText(main, "BN_NgayVaoVien", FormatDateTime(hanhChinh?.NgayVaoVien));
                SetBookmarkText(main, "BN_NgayRaVien", FormatDateTime(hanhChinh?.NgayRaVien));

                SetBookmarkText(main, "BN_ChanDoanBenhChinhVao", chanDoan?.BenhChinhVaoVien);
                SetBookmarkText(main, "BN_ICDBenhChinhVao", chanDoan?.MaICDChinhVaoVien);
                SetBookmarkText(main, "BN_ChanDoanBenhPhuVao", chanDoan?.BenhPhuVaoVien);
                SetBookmarkText(main, "BN_ICDBenhPhuVao", chanDoan?.MaICDPhuVaoVien);
                SetBookmarkText(main, "BN_ChanDoanBenhChinhRa", chanDoan?.BenhChinhRaVien);
                SetBookmarkText(main, "BN_ICDBenhChinhRa", chanDoan?.MaICDChinhRaVien);
                SetBookmarkText(main, "BN_ChanDoanBenhPhuRa", chanDoan?.BenhKemTheoRaVien);
                SetBookmarkText(main, "BN_ICDBenhPhuRa", chanDoan?.MaICDKemTheoRaVien);

                SetBookmarkText(main, "BN_LyDoVaoVien", khamBenh?.LyDoVaoVien);
                SetBookmarkText(main, "BN_TomTatQuaTrinhBenhLy", tomTat?.TomTatQuaTrinhBenhLy);
                SetBookmarkText(main, "BN_TienSuBenh", khamBenh?.TienSuBenh);
                SetBookmarkRawWithBreaks(main, "BN_DauHieuLamSang", tomTat?.TomTatDauHieuLamSang);
                SetBookmarkRawWithBreaks(main, "BN_KetQuaXetNghiem", tomTat?.TomTatKetQuaXN);

                bool hasNoiKhoa = !string.IsNullOrWhiteSpace(khamBenh?.HuongDieuTri) &&
                                  khamBenh!.HuongDieuTri!.ToLower().Contains("nội khoa");
                bool hasPTTT = !string.IsNullOrWhiteSpace(tinhTrang?.Ppdt) ||
                               (!string.IsNullOrWhiteSpace(khamBenh?.HuongDieuTri) &&
                                (khamBenh!.HuongDieuTri!.ToLower().Contains("phẫu thuật") ||
                                 khamBenh!.HuongDieuTri!.ToLower().Contains("thủ thuật")));

                SetCheckbox(main, "CheckBoxNoiKhoaFalse", !hasNoiKhoa);
                SetCheckbox(main, "CheckBoxNoiKhoaTrue", hasNoiKhoa);
                SetBookmarkText(main, "LyDoNoiKhoa", hasNoiKhoa ? khamBenh?.HuongDieuTri : "");

                SetCheckbox(main, "CheckPTTT", hasPTTT);
                SetBookmarkText(main, "LyDoPTTT", hasPTTT ? (tinhTrang?.Ppdt ?? khamBenh?.HuongDieuTri) : "");

                SetBookmarkText(main, "BN_PPDT", tinhTrang?.Ppdt);

                var kq = (hanhChinh?.KetQuaDieuTri ?? "").ToLower();
                SetCheckbox(main, "CK_Khoi", kq.Contains("khỏi"));
                SetCheckbox(main, "CK_Do", kq.Contains("đỡ"));
                SetCheckbox(main, "CK_KhongThayDoi", kq.Contains("không thay đổi"));
                SetCheckbox(main, "CK_NangHon", kq.Contains("nặng hơn"));
                SetCheckbox(main, "CK_TuVong", kq.Contains("tử vong"));
                SetCheckbox(main, "CK_TienLuongNang", kq.Contains("tiên lượng nặng"));

                SetBookmarkText(main, "BN_TTNguoiBenhRaVien", tomTat?.TomTatTinhTrangNguoiBenhRaVien);
                SetBookmarkText(main, "BN_HuongDieuTri", tomTat?.TomTatHuongDieuTriTiepTheo);

                main.Document.Save();
            }

            return outputPath!;
        }

        private static void SetBookmarkText(MainDocumentPart main, string bookmarkName, string? text)
        {
            var starts = main.Document.Body
                .Descendants<BookmarkStart>()
                .Where(b => b.Name == bookmarkName)
                .ToList();

            foreach (var start in starts)
            {
                var end = main.Document.Body.Descendants<BookmarkEnd>()
                            .FirstOrDefault(e => e.Id.Value == start.Id.Value);
                if (end == null) continue;

                var node = start.NextSibling();
                while (node != null && node != end)
                {
                    var next = node.NextSibling();
                    node.Remove();
                    node = next;
                }

                var run = new Run(new Text(text ?? string.Empty) { Space = SpaceProcessingModeValues.Preserve });
                start.Parent!.InsertAfter(run, start);
            }
        }

        private static void SetBookmarkRawWithBreaks(MainDocumentPart main, string bookmarkName, string? raw)
        {
            var start = main.Document.Body.Descendants<BookmarkStart>()
                         .FirstOrDefault(b => b.Name == bookmarkName);
            if (start == null) return;

            var text = (raw ?? string.Empty).Replace("\r\n", "\n");
            var lines = text.Split('\n');

            if (start.Parent is Run run)
            {
                OpenXmlElement last = start;
                for (int i = 0; i < lines.Length; i++)
                {
                    var t = new Text(lines[i]) { Space = SpaceProcessingModeValues.Preserve };
                    run.InsertAfter(t, last);
                    last = t;

                    if (i < lines.Length - 1)
                    {
                        var br = new Break();
                        run.InsertAfter(br, last);
                        last = br;
                    }
                }
                return;
            }

            var para = start.Ancestors<Paragraph>().FirstOrDefault();
            if (para == null) return;

            var baseRun = para.Elements<Run>().FirstOrDefault();
            var newRun = new Run();
            if (baseRun?.RunProperties != null)
                newRun.RunProperties = (RunProperties)baseRun.RunProperties.CloneNode(true);

            for (int i = 0; i < lines.Length; i++)
            {
                newRun.Append(new Text(lines[i]) { Space = SpaceProcessingModeValues.Preserve });
                if (i < lines.Length - 1)
                    newRun.Append(new Break());
            }

            para.InsertAfter(newRun, para.Elements<Run>().LastOrDefault() ?? (OpenXmlElement)start);
        }

        private static void SetCheckbox(MainDocumentPart main, string bookmarkName, bool isChecked)
        {
            var starts = main.Document.Body.Descendants<BookmarkStart>()
                .Where(b => b.Name == bookmarkName)
                .ToList();

            foreach (var start in starts)
            {
                var end = main.Document.Body.Descendants<BookmarkEnd>()
                            .FirstOrDefault(e => e.Id.Value == start.Id.Value);
                if (end == null) continue;

                var node = start.NextSibling();
                while (node != null && node != end)
                {
                    var next = node.NextSibling();
                    node.Remove();
                    node = next;
                }

                // Chỉ set trạng thái check, không custom symbol
                var cb = new SdtContentCheckBox(
                    new W2010.Checked { Val = isChecked ? OnOffValues.One : OnOffValues.Zero }
                );

                var sdt = new SdtRun(
                    new SdtProperties(new Tag { Val = bookmarkName }, cb),
                    new SdtContentRun(new Run(new Text("")))
                );

                if (start.Parent is Run r)
                    r.InsertAfter(sdt, start);
                else
                    start.Parent!.InsertAfter(new Run(sdt), start);
            }
        }
    }
}
