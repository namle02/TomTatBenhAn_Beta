using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;


namespace TomTatBenhAn_WPF.ViewModel.ControlViewModel
{
    public partial class SideBarViewModel : ObservableObject, IRecipient<LoadingStatusMessage>
    {
        [ObservableProperty] private string soBenhAn = string.Empty;
        [ObservableProperty] private string maYTe = string.Empty;
        [ObservableProperty] private PatientAllData? patient = new PatientAllData();
        [ObservableProperty] private ObservableCollection<string>? soBenhAnList = new ObservableCollection<string>();
        [ObservableProperty] private string doctorName = "TsBs. Vũ Trung Kiên";
        [ObservableProperty] private string reportNumber = string.Empty;
        [ObservableProperty] private bool isMaYTeChecked;
        [ObservableProperty] private bool isSoBenhAnChecked;
        [ObservableProperty] private string selectedBenhAn = string.Empty;
        [ObservableProperty] private bool isSelectedEnable = false;
        [ObservableProperty] private bool isLoadingData = false;
        [ObservableProperty] private bool isProcessingAI = false;
        [ObservableProperty] private string userName = "Hà Thị Vui";
        [ObservableProperty] private string userDepartment = "Phòng Kế hoạch tổng hợp";
        [ObservableProperty] private string userUsageCount = "1952";

        private readonly IDataMapper _dataMapper;
     


        public SideBarViewModel(IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;
          
            // Đăng ký nhận message loading status
            WeakReferenceMessenger.Default.Register<LoadingStatusMessage>(this);
        }

        /// <summary>
        /// Nhận thông báo loading status từ ContentViewModel
        /// </summary>
        public void Receive(LoadingStatusMessage message)
        {
            IsProcessingAI = message.Value;
            System.Diagnostics.Debug.WriteLine($"SideBar nhận loading status: {message.Value}");
        }

        /// <summary>
        /// Kiểm tra có thể thực hiện lệnh fetch data không
        /// </summary>
        public bool CanFetchData => !IsLoadingData && !IsProcessingAI;

        /// <summary>
        /// Load danh sách số bệnh án theo mã y tế
        /// </summary>
        private async Task LoadBenhAnListByMaYTe()
        {
            try
            {
                IsLoadingData = true;
                SoBenhAnList = new ObservableCollection<string>();
                Patient = new PatientAllData();
                
                Patient!.DanhSachBenhAn = await _dataMapper.GetBenhAnList(MaYTe);
                foreach (var item in Patient.DanhSachBenhAn)
                {
                    SoBenhAnList!.Add(item.SoBenhAn);
                }
                if (SoBenhAnList.Count != 0)
                {
                    IsSelectedEnable = true;
                }
                else
                {
                    MessageBox.Show("Bệnh nhân không có số bệnh án phù hợp");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách bệnh án: {ex.Message}");
            }
            finally
            {
                IsLoadingData = false;
            }
        }

        [RelayCommand(CanExecute = nameof(CanFetchData))]
        private async Task FetchData()
        {
            try
            {
                IsLoadingData = true;
                SoBenhAnList = new ObservableCollection<string>();
                Patient = new PatientAllData();
                
                if (IsSoBenhAnChecked)
                {
                    Patient = await _dataMapper.GetAllPatientData(SoBenhAn);
                    Patient.ReportNumber = this.ReportNumber;
                    Patient.DoctorName = this.DoctorName;
                    WeakReferenceMessenger.Default.Send(new SendPatientDataMessage(Patient, "SideBarVM"));
                    SoBenhAnList.Clear();
                    IsSelectedEnable = false;
                }
                else if (IsMaYTeChecked)
                {
                    await LoadBenhAnListByMaYTe();
                }
                else
                {
                    MessageBox.Show("Hãy chọn số bệnh án hoặc mã y tế");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
            finally
            {
                IsLoadingData = false;
            }
        }

        /// <summary>
        /// In bản tóm tắt
        /// </summary>
        [RelayCommand]
        private void PrintReport()
        {
            WeakReferenceMessenger.Default.Send<string>("PrintReport");
        }
        
        /// <summary>
        /// Lưu tiến trình hiện tại
        /// </summary>
        [RelayCommand]
        private void SaveProgress()
        {
            WeakReferenceMessenger.Default.Send<string>("SaveProgress");
        }


        partial void OnIsMaYTeCheckedChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                IsSoBenhAnChecked = false;
                SoBenhAn = string.Empty;
            }
        }

        async partial void OnMaYTeChanged(string? oldValue, string newValue)
        {
            // Chỉ tự động load khi checkbox Mã Y Tế được chọn và mã y tế đủ 8 ký tự
            if (IsMaYTeChecked && !string.IsNullOrWhiteSpace(newValue) && newValue.Length == 8)
            {
                await LoadBenhAnListByMaYTe();
            }
            else if (IsMaYTeChecked && newValue.Length < 8)
            {
                // Reset danh sách khi mã y tế chưa đủ 8 ký tự
                SoBenhAnList = new ObservableCollection<string>();
                IsSelectedEnable = false;
            }
        }

        partial void OnIsSoBenhAnCheckedChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                IsMaYTeChecked = false;
                MaYTe = string.Empty;
            }
        }

        async partial void OnSelectedBenhAnChanged(string? oldValue, string newValue)
        {
            try
            {
                if (newValue != null)
                {
                    IsLoadingData = true;
                    Patient = await _dataMapper.GetAllPatientData(newValue);
                    WeakReferenceMessenger.Default.Send(new SendPatientDataMessage(Patient, "SideBarVM"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu bệnh án: {ex.Message}");
            }
            finally
            {
                IsLoadingData = false;
            }
        }

        /// <summary>
        /// Cập nhật CanExecute khi IsLoadingData thay đổi
        /// </summary>
        partial void OnIsLoadingDataChanged(bool value)
        {
            FetchDataCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Cập nhật CanExecute khi IsProcessingAI thay đổi
        /// </summary>
        partial void OnIsProcessingAIChanged(bool value)
        {
            FetchDataCommand.NotifyCanExecuteChanged();
        }
    }
}
