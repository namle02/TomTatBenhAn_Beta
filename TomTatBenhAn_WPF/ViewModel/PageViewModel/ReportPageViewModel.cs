using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.ViewModel.PageViewModel
{
    public partial class ReportPageViewModel : ObservableRecipient
    {
        private readonly IReportService _reportServices;
        
        [ObservableProperty] private PatientAllData? patient;
        [ObservableProperty] private string htmlContent = string.Empty;
        [ObservableProperty] private bool isLoading = false;

        public ReportPageViewModel(IReportService reportServices)
        {
            _reportServices = reportServices;
        }

        public async Task LoadDataAsync(PatientAllData patient)
        {
            try
            {
                IsLoading = true;
                Patient = patient;
                
                // Generate HTML content từ patient data
                HtmlContent = await _reportServices.GenerateHtmlReportAsync(patient);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo HTML report: {ex.Message}");
                MessageBox.Show($"Lỗi khi tạo báo cáo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
