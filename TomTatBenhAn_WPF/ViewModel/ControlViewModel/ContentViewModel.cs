using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Services.Interface;
using TomTatBenhAn_WPF.View.PageView;
using TomTatBenhAn_WPF.ViewModel.PageViewModel;

namespace TomTatBenhAn_WPF.ViewModel.ControlViewModel
{
    public partial class ContentViewModel : ObservableRecipient, IRecipient<SendPatientDataMessage>, IRecipient<string>
    {
        private readonly IAiService _aiServices;
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
                    // Có thể hiển thị thông báo cho user nếu cần
                    // MessageBox.Show($"Không thể cập nhật kết quả điều trị: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public async void Receive(SendPatientDataMessage message)
        {
            try
            {
                if (message.CalledBy == "SideBarVM")
                {
                    // Validate input
                    if (message?.patient == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Nhận được message hoặc patient null");
                        return;
                    }

                    // Hiển thị loading và thông báo cho SideBar
                    IsLoading = true;
                    LoadingText = "Đang tải dữ liệu bệnh nhân...";
                    WeakReferenceMessenger.Default.Send(new LoadingStatusMessage(true));

                    // Cập nhật patient data trước
                    this.Patient = message.patient;
                    if (Patient.ThongTinKhamBenh![0].HuongDieuTri.Length != 0)
                    {
                        IsHuongDieuTriContains = true;
                    }
                    OnPropertyChanged(nameof(KetQuaDieuTri));

                    // Cập nhật loading text cho AI processing
                    LoadingText = "Đang tóm tắt bệnh án với AI...";

                    // Gọi AI service để tóm tắt bệnh án
                    await _aiServices.TomTatBenhAn(message.patient);

                    System.Diagnostics.Debug.WriteLine("Đã load và xử lý dữ liệu bệnh nhân thành công");

                    // Đánh dấu báo cáo đã sẵn sàng
                    IsReportReady = true;

                    // Gửi message để mở báo cáo sau khi AI hoàn thành
                    WeakReferenceMessenger.Default.Send(new NavigationMessage("OpenReport", "ContentVM"));
                }
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi xử lý dữ liệu bệnh nhân: {ex.Message}");
                
                // Hiển thị thông báo lỗi cho user
                MessageBox.Show(
                    $"Có lỗi xảy ra khi xử lý dữ liệu bệnh nhân:\n{ex.Message}\n\nVui lòng thử lại hoặc liên hệ hỗ trợ kỹ thuật.", 
                    "Lỗi xử lý dữ liệu", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                
                // Vẫn cập nhật patient data nếu có (có thể AI service lỗi nhưng data gốc vẫn ok)
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
                // Ẩn loading trong mọi trường hợp và thông báo cho SideBar
                IsLoading = false;
                WeakReferenceMessenger.Default.Send(new LoadingStatusMessage(false));
            }
        }

        public ContentViewModel(IAiService aiServices, IServiceProvider provider)
        {
            try
            {
                IsActive = true;
                _aiServices = aiServices ?? throw new ArgumentNullException(nameof(aiServices));
                this.provider=provider;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi khởi tạo ContentViewModel: {ex.Message}");
                throw; // Re-throw để DI container biết có lỗi
            }

            this.provider=provider;
        }

        // Override OnPropertyChanged để đảm bảo KetQuaDieuTri được cập nhật khi Patient thay đổi
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
                // Không throw exception ở đây để tránh crash UI
            }
        }

        public async void Receive(string message)
        {
            if(message == "PrintReport")
            {
                var reportPage = provider.GetRequiredService<ReportPage>();
                if(reportPage.DataContext is ReportPageViewModel vm)
                {
                    await vm.LoadDataAsync(Patient);
                }
                reportPage.Show();
            }
        }
    }
}
