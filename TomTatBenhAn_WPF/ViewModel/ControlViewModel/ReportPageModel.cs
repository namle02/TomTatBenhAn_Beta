using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Web.WebView2.Wpf;
using System.IO;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos.Model;
using System.Threading.Tasks;

using System;
using System.Threading.Tasks;


namespace TomTatBenhAn_WPF.ViewModel
{
    public class ReportPageModel
    {
        private readonly WebView2 _webView;
        private readonly string _templateHtml;

        public ReportPageModel(WebView2 webView, ThongTinBenhNhan patient, string aiTomTatQuaTrinhBenhLy,HanhChinhModel hanhchinh, ChuanDoanModel chuandoan,BenhAnChiTietModel benhAnChiTiet,string aiDauHieuLamSang)
        {
            _webView = webView;

            // Đọc template HTML
            string htmlPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ReportTemplate.html");
            _templateHtml = File.Exists(htmlPath) ? File.ReadAllText(htmlPath) : "<h2>Không tìm thấy file template</h2>";

            // Hiển thị báo cáo
            _ = ShowReport(patient, aiTomTatQuaTrinhBenhLy,hanhchinh,chuandoan, benhAnChiTiet, aiDauHieuLamSang);
        }

        private async Task ShowReport(ThongTinBenhNhan patient, string aiTomTatQuaTrinhBenhLy,HanhChinhModel hanhchinh,ChuanDoanModel chuandoan, BenhAnChiTietModel benhAnChiTiet,string aiDauHieuLamSang)
        {
            await _webView.EnsureCoreWebView2Async(null);

            // Replace dữ liệu trong template
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
                  .Replace("{{IcdBenhPhuRaVien}}", chuandoan?.icdRaVienPhu ?? "")
                  .Replace("{{LiDoVaoVien}}", benhAnChiTiet?.LyDoVaoVien ?? "")
                  .Replace("{{TienSuBenh}}", benhAnChiTiet?.TienSuBenh ?? "")
                  .Replace("{{DauHieuLamSang}}", aiDauHieuLamSang ?? "")
                  .Replace("{{PhuongPhapDieuTri}}", benhAnChiTiet.HuongDieuTri ?? "")

                ;

            _webView.NavigateToString(html);
        }
    }

}
