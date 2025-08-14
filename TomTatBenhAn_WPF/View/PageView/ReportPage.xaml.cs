using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using TomTatBenhAn_WPF.ViewModel.PageViewModel;

namespace TomTatBenhAn_WPF.View.PageView
{
    public partial class ReportPage : Window
    {
        public ReportPage(ReportPageViewModel reportPageViewModel)
        {
            InitializeComponent();
            DataContext = reportPageViewModel;
            
            // Subscribe to WebView events
            ReportView.NavigationCompleted += ReportView_NavigationCompleted;
        }

        private void ReportView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            // Add WebMessage listener for PDF export
            ReportView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
        }

        private async void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var message = e.TryGetWebMessageAsString();
                
                // Parse JSON message
                if (message.Contains("export-pdf"))
                {
                    // Show save dialog
                    var saveDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                        DefaultExt = "pdf",
                        FileName = $"BaoTao_BenhAn_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                    };

                    if (saveDialog.ShowDialog() == true)
                    {
                        await ExportToPdf(saveDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý message: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExportToPdf(string filePath)
        {
            try
            {
                // Export to PDF using WebView2
                await ReportView.CoreWebView2.PrintToPdfAsync(filePath);
                
                MessageBox.Show($"Đã xuất báo cáo thành công!\nFile được lưu tại: {filePath}", 
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

