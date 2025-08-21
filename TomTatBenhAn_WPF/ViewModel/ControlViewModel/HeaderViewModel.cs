using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.ViewModel.ControlViewModel
{
    public partial class HeaderViewModel : ObservableObject
    {
        private readonly IBenhNhanService _benhNhanService;

        [ObservableProperty]
        private string soBenhAn = string.Empty;

        [ObservableProperty]
        private bool isSearching = false;

        public HeaderViewModel(IBenhNhanService benhNhanService)
        {
            _benhNhanService = benhNhanService ?? throw new ArgumentNullException(nameof(benhNhanService));
        }

        [RelayCommand]
        private async Task SearchPatientAsync()
        {
            await SearchPatient();
        }

        [RelayCommand]
        private async Task OnEnterKeyAsync()
        {
            await SearchPatient();
        }

        private async Task SearchPatient()
        {
            var soBenhAnTrimmed = SoBenhAn?.Trim();
            
            if (string.IsNullOrEmpty(soBenhAnTrimmed))
            {
                MessageBox.Show("Vui lòng nhập số bệnh án!", "Thông báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                IsSearching = true;
                
                // Hiển thị loading
                WeakReferenceMessenger.Default.Send(new LoadingStatusMessage(true));

                // Gọi API tìm kiếm
                var result = await _benhNhanService.GetBenhNhanBySoBenhAnAsync(soBenhAnTrimmed);

                if (result.Success && result.Data != null)
                {
                    // Gửi dữ liệu bệnh nhân về ContentViewModel
                    WeakReferenceMessenger.Default.Send(new SendPatientDataMessage(result.Data));
                    
                    MessageBox.Show("✅ Tìm thấy thông tin bệnh nhân!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Xóa text trong ô tìm kiếm
                    SoBenhAn = string.Empty;
                }
                else
                {
                    MessageBox.Show($"❌ Không tìm thấy bệnh nhân với số bệnh án: {soBenhAnTrimmed}", "Không tìm thấy", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"🛑 Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsSearching = false;
                // Ẩn loading
                WeakReferenceMessenger.Default.Send(new LoadingStatusMessage(false));
            }
        }
    }
}
