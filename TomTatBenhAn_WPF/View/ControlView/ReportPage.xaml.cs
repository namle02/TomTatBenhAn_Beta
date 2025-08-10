using Microsoft.Web.WebView2.Wpf;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using TomTatBenhAn_WPF.Repos.Model;
using TomTatBenhAn_WPF.ViewModel;

namespace TomTatBenhAn_WPF.View.ControlView
{
    public partial class ReportPage : Window
    {
        public ReportPage(
            ThongTinBenhNhan patient,
            string aiTomTatQuaTrinhBenhLy,
            string aiDauHieuLamSang,
            HanhChinhModel hanhchinh,
            BenhAnChiTietModel benhAnChiTiet,
            string aiKQXN,
            string aiDienBien,
            CheckBoxModel checkBox,
            ChuanDoanModel chuandoan,
            string tinhTrangRaVien,
            string HuongDieuTriTiepTheo
            )
        {
            InitializeComponent();

            // Gán ViewModel (render HTML)
            DataContext = new ReportPageModel(
                ReportView,
                patient,
                aiTomTatQuaTrinhBenhLy,
                hanhchinh,
                chuandoan,
                benhAnChiTiet,
                aiDauHieuLamSang,
                aiKQXN,
                aiDienBien,
                checkBox,
                tinhTrangRaVien,
                HuongDieuTriTiepTheo
            );

            Loaded += ReportPage_Loaded;
        }

        private async void ReportPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ReportView.EnsureCoreWebView2Async();

            ReportView.CoreWebView2.WebMessageReceived -= CoreWebView2_WebMessageReceived;
            ReportView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
        }

        private async void CoreWebView2_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            

            try
            {
                using var doc = JsonDocument.Parse(e.WebMessageAsJson);
                if (doc.RootElement.TryGetProperty("action", out var actionProp))
                {
                    var action = actionProp.GetString();
                    if (action == "export-pdf")
                    {
                        await ExportPdfAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nhận message từ WebView2:\n" + ex.Message);
            }
        }

        private async Task ExportPdfAsync()
        {
            try
            {
                string folderName = "BaoCaoBenhAn"; // Tên folder tùy chỉnh
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string folderPath = Path.Combine(desktopPath, folderName);

                // Nếu folder chưa có thì tạo mới
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string outputPath = Path.Combine(folderPath, fileName);

                bool success = await ReportView.CoreWebView2.PrintToPdfAsync(outputPath);

                if (success)
                {
                    MessageBox.Show($"✅ Đã xuất PDF ra: {folderPath}\n{fileName}", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    System.Diagnostics.Process.Start("explorer", folderPath);
                }
                else
                {
                    MessageBox.Show("❌ Xuất PDF thất bại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất PDF:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
