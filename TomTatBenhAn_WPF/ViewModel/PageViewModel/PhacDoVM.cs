using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.ViewModel.PageViewModel
{
    public partial class PhacDoVM : ObservableObject
    {
        private readonly IPhacDoServices _phacDoServices;

        [ObservableProperty] private ObservableCollection<BangKiemItemDTO>? bangKiemList;
        [ObservableProperty] private ObservableCollection<PhacDoItemDTO>? phacDoList;
        [ObservableProperty] private ObservableCollection<PhacDoItemDTO>? compatiblePhacDoList;
        [ObservableProperty] private PhacDoItemDTO? selectedPhacDo;
        [ObservableProperty] private string searchText = string.Empty;
        [ObservableProperty] private bool isLoading = false;
        [ObservableProperty] private string statusMessage = string.Empty;
        [ObservableProperty] private string newProtocolText = string.Empty;
        [ObservableProperty] private bool forceOverwrite = false;

        [RelayCommand]
        private void ChangePage()
        {
            WeakReferenceMessenger.Default.Send(new NavigationMessage("TomTatBenhAnPage", "PhacDoPage"));
        }

        [RelayCommand]
        private async Task LoadAllPhacDoAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Đang tải danh sách phác đồ...";

                var result = await _phacDoServices.GetAllPhacDoAsync();
                
                if (result.Success && result.Data != null)
                {
                    PhacDoList = new ObservableCollection<PhacDoItemDTO>(result.Data);
                    StatusMessage = $"Đã tải {result.Data.Count} phác đồ";
                }
                else
                {
                    StatusMessage = $"Lỗi: {result.Message}";
                    MessageBox.Show($"Không thể tải danh sách phác đồ: {result.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi: {ex.Message}";
                MessageBox.Show($"Lỗi khi tải danh sách phác đồ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SearchPhacDoAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadAllPhacDoAsync();
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = $"Đang tìm kiếm '{SearchText}'...";

                var result = await _phacDoServices.SearchPhacDoAsync(SearchText);
                
                if (result.Success && result.Data != null)
                {
                    PhacDoList = new ObservableCollection<PhacDoItemDTO>(result.Data);
                    StatusMessage = $"Tìm thấy {result.Data.Count} phác đồ";
                }
                else
                {
                    StatusMessage = $"Lỗi: {result.Message}";
                    MessageBox.Show($"Không thể tìm kiếm phác đồ: {result.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi: {ex.Message}";
                MessageBox.Show($"Lỗi khi tìm kiếm phác đồ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task DeletePhacDoAsync(string phacDoId)
        {
            if (string.IsNullOrEmpty(phacDoId))
                return;

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa phác đồ này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                IsLoading = true;
                StatusMessage = "Đang xóa phác đồ...";

                var deleteResult = await _phacDoServices.DeletePhacDoAsync(phacDoId);
                
                if (deleteResult.Success)
                {
                    StatusMessage = "Đã xóa phác đồ thành công";
                    await LoadAllPhacDoAsync(); // Reload danh sách
                }
                else
                {
                    StatusMessage = $"Lỗi: {deleteResult.Message}";
                    MessageBox.Show($"Không thể xóa phác đồ: {deleteResult.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi: {ex.Message}";
                MessageBox.Show($"Lỗi khi xóa phác đồ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddNewProtocolAsync()
        {
            if (string.IsNullOrWhiteSpace(NewProtocolText))
            {
                MessageBox.Show("Vui lòng nhập nội dung phác đồ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = "Đang phân tích và thêm phác đồ...";

                var result = await _phacDoServices.AddPhacDoWithForceAsync(NewProtocolText, ForceOverwrite);
                
                if (result.Success)
                {
                    StatusMessage = "Đã thêm phác đồ thành công";
                    NewProtocolText = string.Empty; // Clear text after success
                    await LoadAllPhacDoAsync(); // Reload list
                    MessageBox.Show("Thêm phác đồ thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    StatusMessage = $"Lỗi: {result.Message}";
                    var msgResult = MessageBox.Show(
                        $"Không thể thêm phác đồ: {result.Message}\n\nBạn có muốn thử lại với tùy chọn ghi đè không?", 
                        "Lỗi", 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Question);
                    
                    if (msgResult == MessageBoxResult.Yes)
                    {
                        ForceOverwrite = true;
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi: {ex.Message}";
                MessageBox.Show($"Lỗi khi thêm phác đồ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void ClearNewProtocol()
        {
            NewProtocolText = string.Empty;
            ForceOverwrite = false;
            StatusMessage = "Đã xóa nội dung nhập";
        }

        public PhacDoVM(IPhacDoServices phacDoServices)
        {
            _phacDoServices = phacDoServices;
            
            // Tự động tải danh sách phác đồ khi khởi tạo
            _ = Task.Run(LoadAllPhacDoAsync);
        }
    }
}
