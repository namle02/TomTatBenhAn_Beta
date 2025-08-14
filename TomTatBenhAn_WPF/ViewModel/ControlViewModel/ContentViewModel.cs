using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Services.Interface;
using TomTatBenhAn_WPF.View.PageView;
using TomTatBenhAn_WPF.ViewModel.PageViewModel;
using System.IO;

namespace TomTatBenhAn_WPF.ViewModel.ControlViewModel
{
    public partial class ContentViewModel : ObservableRecipient, IRecipient<SendPatientDataMessage>, IRecipient<string>
    {
        private readonly IAiService _aiServices;
        private readonly IReportService _reportService;
        private readonly IServiceProvider provider;

        [ObservableProperty] private PatientAllData patient = new PatientAllData();
        [ObservableProperty] private bool isLoading = false;
        [ObservableProperty] private string loadingText = "Đang tóm tắt bệnh án...";
        [ObservableProperty] private bool isHuongDieuTriContains;
        [ObservableProperty] private bool isReportReady = false;

        // Computed properties để lấy kết quả điều trị từ Patient
        public string? KetQuaDieuTri
        {
            get
            {
                try
                {
                    return Patient?.ThongTinHanhChinh?.FirstOrDefault()?.KetQuaDieuTri;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy KetQuaDieuTri: {ex.Message}");
                    return null;
                }
            }
            set
            {
                try
                {
                    if (Patient?.ThongTinHanhChinh?.FirstOrDefault() != null)
                    {
                        Patient.ThongTinHanhChinh[0].KetQuaDieuTri = value;
                        OnPropertyChanged();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi set KetQuaDieuTri: {ex.Message}");
                }
            }
        }

        public async void Receive(SendPatientDataMessage message)
        {
            try
            {
                if (message.CalledBy == "SideBarVM")
                {
                    if (message?.patient == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Nhận được message hoặc patient null");
                        return;
                    }

                    IsLoading = true;
                    LoadingText = "Đang tải dữ liệu bệnh nhân...";
                    WeakReferenceMessenger.Default.Send(new LoadingStatusMessage(true));

                    this.Patient = message.patient;
                    if (Patient.ThongTinKhamBenh![0].HuongDieuTri.Length != 0)
                    {
                        IsHuongDieuTriContains = true;
                    }
                    OnPropertyChanged(nameof(KetQuaDieuTri));

                    LoadingText = "Đang tóm tắt bệnh án với AI...";
                    await _aiServices.TomTatBenhAn(message.patient);

                    System.Diagnostics.Debug.WriteLine("Đã load và xử lý dữ liệu bệnh nhân thành công");

                    IsReportReady = true;
                    WeakReferenceMessenger.Default.Send(new NavigationMessage("OpenReport", "ContentVM"));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi xử lý dữ liệu bệnh nhân: {ex.Message}");
                MessageBox.Show(
                    $"Có lỗi xảy ra khi xử lý dữ liệu bệnh nhân:\n{ex.Message}\n\nVui lòng thử lại hoặc liên hệ hỗ trợ kỹ thuật.",
                    "Lỗi xử lý dữ liệu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                if (message?.patient != null)
                {
                    this.Patient = message.patient;
                    if (Patient.ThongTinKhamBenh![0].HuongDieuTri.Length != 0)
                    {
                        IsHuongDieuTriContains = true;
                    }
                    OnPropertyChanged(nameof(KetQuaDieuTri));
                }
            }
            finally
            {
                IsLoading = false;
                WeakReferenceMessenger.Default.Send(new LoadingStatusMessage(false));
            }
        }

        public ContentViewModel(IAiService aiServices, IReportService reportService, IServiceProvider provider)
        {
            try
            {
                IsActive = true;
                _aiServices = aiServices ?? throw new ArgumentNullException(nameof(aiServices));
                _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
                this.provider = provider;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi khởi tạo ContentViewModel: {ex.Message}");
                throw;
            }
        }

        partial void OnPatientChanged(PatientAllData? oldValue, PatientAllData newValue)
        {
            try
            {
                OnPropertyChanged(nameof(KetQuaDieuTri));
                System.Diagnostics.Debug.WriteLine("Đã cập nhật KetQuaDieuTri sau khi Patient thay đổi");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi cập nhật KetQuaDieuTri: {ex.Message}");
            }
        }

        public async void Receive(string message)
        {
            if (message == "PrintReport")
            {
                try
                {
                    // Đường dẫn tới file template Word (khớp tên thật)
                    string templatePath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Templates",
                        "TemplateTomTat.docx"
                    );

                    if (!File.Exists(templatePath))
                    {
                        MessageBox.Show(
                            $"Không tìm thấy file template:\n{templatePath}",
                            "Lỗi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                        return;
                    }

                    // Gọi hàm xuất file Word
                    string filePath = await _reportService.ExportDocxFromTemplateAsync(Patient, templatePath);

                    MessageBox.Show(
                        $"Xuất file Word thành công:\n{filePath}",
                        "Thành công",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    // Mở file sau khi tạo
                    if (File.Exists(filePath))
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = filePath,
                            UseShellExecute = true
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Lỗi khi xuất Word:\n{ex.Message}",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

    }
}
