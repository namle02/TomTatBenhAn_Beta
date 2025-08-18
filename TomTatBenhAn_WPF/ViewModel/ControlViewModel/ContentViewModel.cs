using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Services.Interface;
using System.IO;

namespace TomTatBenhAn_WPF.ViewModel.ControlViewModel
{
    public partial class ContentViewModel : ObservableRecipient, IRecipient<SendPatientDataMessage>, IRecipient<string>
    {
        private readonly IAiService _aiServices;
        private readonly IReportService _reportService;


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
                if (message?.patient == null)
                {
                    System.Diagnostics.Debug.WriteLine("Nhận được message hoặc patient null");
                    return;
                }

                IsLoading = true;
                LoadingText = "Đang tải dữ liệu bệnh nhân...";
                WeakReferenceMessenger.Default.Send(new LoadingStatusMessage(true));

                this.Patient = message.patient;
                
                // Kiểm tra an toàn trước khi truy cập ThongTinKhamBenh
                if (Patient.ThongTinKhamBenh != null && 
                    Patient.ThongTinKhamBenh.Count > 0 && 
                    !string.IsNullOrEmpty(Patient.ThongTinKhamBenh[0].HuongDieuTri))
                {
                    IsHuongDieuTriContains = true;
                }
                OnPropertyChanged(nameof(KetQuaDieuTri));

                // Chỉ chạy AI nếu được gọi từ SideBarVM (dữ liệu mới từ database)
                // Nếu từ search thì dữ liệu đã được xử lý AI rồi
                if (message.CalledBy == "SideBarVM")
                {
                    LoadingText = "Đang tóm tắt bệnh án với AI...";
                    await _aiServices.TomTatBenhAn(message.patient);
                }
                else
                {
                    // Dữ liệu từ search đã có tóm tắt AI rồi
                    LoadingText = "Đã tải dữ liệu bệnh nhân...";
                }

                System.Diagnostics.Debug.WriteLine("Đã load và xử lý dữ liệu bệnh nhân thành công");

                IsReportReady = true;
                WeakReferenceMessenger.Default.Send(new NavigationMessage("OpenReport", "ContentVM"));
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

        public ContentViewModel(IAiService aiServices, IReportService reportService)
        {
            try
            {
                IsActive = true;
                _aiServices = aiServices ?? throw new ArgumentNullException(nameof(aiServices));
                _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
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


        //Hàm in báo cáo
        public void Receive(string message)
        {
            if (message == "PrintReport")
            {
                try
                {
                    // Kiểm tra dữ liệu trước khi xuất
                    if (Patient == null)
                    {
                        MessageBox.Show(
                            "Không có dữ liệu bệnh nhân để xuất báo cáo.",
                            "Thông báo",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
                    }

                    if (Patient.ThongTinHanhChinh == null || !Patient.ThongTinHanhChinh.Any())
                    {
                        MessageBox.Show(
                            "Thiếu thông tin hành chính của bệnh nhân.",
                            "Thông báo",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
                    }

                    // Đường dẫn file template (cần điều chỉnh theo đường dẫn thực tế)
                    string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "TemplateTomTat.docx");
                    
                    // Kiểm tra file template có tồn tại không
                    if (!File.Exists(templatePath))
                    {
                        MessageBox.Show(
                            $"Không tìm thấy file mẫu tại:\n{templatePath}\n\nVui lòng kiểm tra lại đường dẫn file template.",
                            "Lỗi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                        return;
                    }

                    // Hiển thị loading
                    IsLoading = true;
                    LoadingText = "Đang xuất báo cáo Word...";
                    WeakReferenceMessenger.Default.Send(new LoadingStatusMessage(true));

                    // Gọi service để in báo cáo
                    _ = Task.Run(() => _reportService.PrintFileWord(templatePath, Patient));

                    // Thông báo thành công
                    string month = DateTime.Now.Month.ToString();
                    string year = DateTime.Now.Year.ToString();
                    string reportNumber = Patient.ReportNumber ?? "RPT";
                    string soBenhAn = Patient.ThongTinHanhChinh[0]?.SoBenhAn ?? "Unknown";
                    string fileName = $"{reportNumber}_{soBenhAn}.docx";
                    string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                                  "HoSoTomTat",$"Nam_{year}", $"Thang_{month}", fileName);

                    MessageBox.Show(
                        $"Xuất báo cáo thành công!\n\nFile đã được lưu tại:\n{savePath}\n\nFile Word sẽ được mở để bạn có thể xem và in.",
                        "Thành công",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Lỗi khi xuất Word:\n{ex.Message}\n\nVui lòng thử lại hoặc liên hệ hỗ trợ kỹ thuật.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    
                    // Log chi tiết lỗi để debug
                    System.Diagnostics.Debug.WriteLine($"Chi tiết lỗi xuất báo cáo: {ex}");
                }
                finally
                {
                    IsLoading = false;
                    WeakReferenceMessenger.Default.Send(new LoadingStatusMessage(false));
                }
            }
        }

    }
}
