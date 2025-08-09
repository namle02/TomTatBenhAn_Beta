using Microsoft.Web.WebView2.Wpf;
using System;
using System.IO;
using System.Threading.Tasks;
using TomTatBenhAn_WPF.Repos.Model;

namespace TomTatBenhAn_WPF.ViewModel
{
    public class ReportPageModel
    {
        private readonly WebView2 _webView;
        private readonly string _templateHtml;

        public ReportPageModel(
            WebView2 webView,
            ThongTinBenhNhan patient,
            string aiTomTatQuaTrinhBenhLy,
            HanhChinhModel hanhchinh,
            ChuanDoanModel chuandoan,
            BenhAnChiTietModel benhAnChiTiet,
            string aiDauHieuLamSang,
            string aiKQXN,
            string aiDienBien,
            CheckBoxModel checkBox,
            string tinhTrangRaVien,
            string huongDieuTriTiepTheo)
        {
            _webView = webView;

            string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ReportTemplate.html");
            _templateHtml = File.Exists(htmlPath) ? File.ReadAllText(htmlPath) : "<h2>Không tìm thấy file template</h2>";

            _ = ShowReport(patient, aiTomTatQuaTrinhBenhLy, hanhchinh, chuandoan, benhAnChiTiet, aiDauHieuLamSang, aiKQXN, aiDienBien, checkBox,tinhTrangRaVien,huongDieuTriTiepTheo);
        }

        private string CheckedIfTrue(bool value) => value ? "checked" : "";

        private async Task ShowReport(
            ThongTinBenhNhan patient,
            string aiTomTatQuaTrinhBenhLy,
            HanhChinhModel hanhchinh,
            ChuanDoanModel chuandoan,
            BenhAnChiTietModel benhAnChiTiet,
            string aiDauHieuLamSang,
            string aiKQXN,
            string aiDienBien,
            CheckBoxModel checkBox,
            string tinhTrangRaVien ,
            string huongDieuTriTiepTheo)
        {
            await _webView.EnsureCoreWebView2Async();

            string html = _templateHtml
                  .Replace("{{TenBenhNhan}}", patient?.TenBenhNhan ?? "")
                .Replace("{{NgaySinh}}", patient?.NgaySinh ?? "")
                .Replace("{{GioiTinh}}", patient?.GioiTinh ?? "")
                .Replace("{{Tuoi}}", patient?.Tuoi?.ToString() ?? "")
                .Replace("{{DiaChi}}", patient?.DiaChi ?? "")
                .Replace("{{DanToc}}", patient?.DanToc ?? "")
                .Replace("{{BHYT}}", patient?.BHYT ?? "")
                .Replace("{{CCCD}}", patient?.CCCD ?? "")
                .Replace("{{SoBenhAn}}", patient?.SoBenhAn ?? "")
                .Replace("{{MaYTe}}", patient?.MaYTe ?? "")
                .Replace("{{AiQuaTrinh}}", aiTomTatQuaTrinhBenhLy ?? "")
                .Replace("{{VaoVien}}", hanhchinh?.ThoiGianVaoVien ?? "")
                .Replace("{{RaVien}}", hanhchinh?.ThoiGianRaVien ?? "")
                .Replace("{{BenhChinhVaoVien}}", chuandoan?.benhChinhVaoVien ?? "")
                .Replace("{{IcdBenhChinhVaoVien}}", chuandoan?.icdVaoKhoaChinh ?? "")
                .Replace("{{BenhPhuVaoVien}}", chuandoan?.benhPhuVaoVien ?? "")
                .Replace("{{IcdBenhPhuVaoVien}}", chuandoan?.icdVaoKhoaPhu ?? "")
                .Replace("{{BenhChinhRaVien}}", chuandoan?.benhChinhRaVien ?? "")
                .Replace("{{IcdBenhChinhRaVien}}", chuandoan?.icdRaVienChinh ?? "")
                .Replace("{{BenhPhuRaVien}}", chuandoan?.benhPhuRaVien ?? "")
                .Replace("{{IcdBenhPhuRaVien}}", chuandoan?.icdRaVienPhu ?? "")
                .Replace("{{LiDoVaoVien}}", benhAnChiTiet?.LyDoVaoVien ?? "")
                .Replace("{{TienSuBenh}}", benhAnChiTiet?.TienSuBenh ?? "")
                .Replace("{{DauHieuLamSang}}", $"<div style='white-space: pre-line'>{aiDauHieuLamSang ?? ""}</div>")
                .Replace("{{PhuongPhapDieuTri}}", benhAnChiTiet?.HuongDieuTri ?? "")
                .Replace("{{KetQuaXetNghiemCLS}}", $"<div style='white-space: pre-line'>{aiKQXN ?? ""}</div>")
                .Replace("{{HuongDieuTri}}", aiDienBien ?? "")
                .Replace("{{TinhTrangRaVien}}", tinhTrangRaVien ?? "")
                .Replace("{{HuongDieuTriTiepTheo}}", huongDieuTriTiepTheo ?? "")
                .Replace("{{IsCheckedKhoi}}", CheckedIfTrue(checkBox?.IsCheckedKhoi ?? false))
                .Replace("{{IsCheckedDo}}", CheckedIfTrue(checkBox?.IsCheckedDo ?? false))
                .Replace("{{IsCheckedKhongThayDoi}}", CheckedIfTrue(checkBox?.IsCheckedKhongThayDoi ?? false))
                .Replace("{{IsCheckedNangHon}}", CheckedIfTrue(checkBox?.IsCheckedNangHon ?? false))
                .Replace("{{IsCheckedTuVong}}", CheckedIfTrue(checkBox?.IsCheckedTuVong ?? false))
                .Replace("{{IsCheckedTienLuongNangXinVe}}", CheckedIfTrue(checkBox?.IsCheckedTienLuongNangXinVe ?? false))
                .Replace("{{IsCheckedChuaXacDinh}}", CheckedIfTrue(checkBox?.IsCheckedChuaXacDinh ?? false))
                 .Replace("{{CheckBoxNoiKhoaFalse}}", CheckedIfTrue(checkBox?.checkBoxNoiKhoaFalse ?? false))
                  .Replace("{{CheckBoxNoiKhoaTrue}}", CheckedIfTrue(checkBox?.checkBoxNoiKhoaTrue ?? false))
                   .Replace("{{CheckBoxPTTTFalse}}", CheckedIfTrue(checkBox?.checkBoxPTTTFalse ?? false))
                 .Replace("{{CheckBoxPTTTTrue}}", CheckedIfTrue(checkBox?.checkBoxPTTTTrue ?? false))
            .Replace("{{LydoNoiKhoaTrue}}", benhAnChiTiet?.LydoNoiKhoaTrue ?? "")
            .Replace("{{LydoPTTTTrue}}", benhAnChiTiet?.LydoPTTTTrue ?? "");

            _webView.NavigateToString(html);
        }
    }
}
