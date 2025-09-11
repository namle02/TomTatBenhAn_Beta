using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Windows;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.ViewModel.PageViewModel
{
    public partial class PhacDoVM : ObservableObject
    {
        private readonly IPhacDoServices _phacDoServices;
        [ObservableProperty] ObservableCollection<PhacDoItemDTO> danhSachPhacDo = new ObservableCollection<PhacDoItemDTO>();
        [ObservableProperty] private string phacDoContent = string.Empty;
        [ObservableProperty] private PhacDoItemDTO rawPhacDo = new PhacDoItemDTO();
        [ObservableProperty] private bool hasNoPhacDo = true;
        [ObservableProperty] private bool isAddPhacDo = false;
        [ObservableProperty] private bool isEditPhacDo = false;
        [ObservableProperty] private bool isViewPhacDo = false;
        [ObservableProperty] private bool isWaiting = false;

        public PhacDoVM(IPhacDoServices phacDoServices)
        {
            _phacDoServices = phacDoServices;
            _ = GetAllPhacDo();
        }


        [RelayCommand]
        private void ChangeModalState()
        {
            IsAddPhacDo = !IsAddPhacDo;
            if (IsAddPhacDo)
            {
                // Reset form khi mở modal thêm mới
                PhacDoContent = string.Empty;
                IsEditPhacDo = false;
            }
        }

        [RelayCommand]
        private void ChangeEditModalState()
        {
            IsEditPhacDo = !IsEditPhacDo;
            if (!IsEditPhacDo)
            {
                // Reset form khi đóng modal edit
                PhacDoContent = string.Empty;
            }
        }

        [RelayCommand]
        private async Task GetAllPhacDo()
        {
            try
            {
                IsWaiting = true;
                DanhSachPhacDo.Clear();
                var apiResponse = await _phacDoServices.GetAllPhacDoAsync();
                
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    foreach(var item in apiResponse.Data)
                    {
                        DanhSachPhacDo.Add(item);
                    }
                }
                else
                {
                    MessageBox.Show($"Lỗi khi tải danh sách phác đồ: {apiResponse.Message}");
                }
                
                HasNoPhacDo = DanhSachPhacDo.Count == 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách phác đồ: {ex.Message}");
                HasNoPhacDo = true;
            }
            finally
            {
                IsWaiting = false;
            }
        }

        [RelayCommand]
        private async Task AddPhacDo()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PhacDoContent))
                {
                    MessageBox.Show("Nội dung phác đồ không được để trống");
                    return;
                }

                IsWaiting = true;
                var result = await _phacDoServices.AddPhacDoWithForceAsync(PhacDoContent, true);
                
                if (result.Success)
                {
                    MessageBox.Show("Thêm phác đồ thành công!");
                    PhacDoContent = string.Empty;
                    IsAddPhacDo = false;
                    await GetAllPhacDo(); // Refresh danh sách
                }
                else
                {
                    MessageBox.Show($"Lỗi khi thêm phác đồ: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm phác đồ mới: {ex.Message}");
            }
            finally
            {
                IsWaiting = false;
            }
        }

        [RelayCommand]
        private async Task DeletePhacDo(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("ID phác đồ không hợp lệ");
                    return;
                }

                // Hiển thị dialog xác nhận
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa phác đồ này?\nThao tác này không thể hoàn tác.", 
                    "Xác nhận xóa", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Warning);
                
                if (result != MessageBoxResult.Yes)
                    return;

                IsWaiting = true;
                var deleteResult = await _phacDoServices.DeletePhacDoAsync(id);
                
                if (deleteResult.Success)
                {
                    MessageBox.Show("Xóa phác đồ thành công!");
                    await GetAllPhacDo(); // Refresh danh sách
                }
                else
                {
                    MessageBox.Show($"Lỗi khi xóa phác đồ: {deleteResult.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa phác đồ: {ex.Message}");
            }
            finally
            {
                IsWaiting = false;
            }
        }

        [RelayCommand]
        private void ViewPhacDo(string id)
        {
            if(IsViewPhacDo == true && id.IsNullOrEmpty())
            {
                IsViewPhacDo = false;
                RawPhacDo = new();
            }
            else
            {
                IsViewPhacDo = true;
                RawPhacDo = new();
                foreach (var item in DanhSachPhacDo)
                {
                    if (item._id == id)
                    {
                        RawPhacDo = item;
                    }
                }

            }
            
           
        }

        
    }
}
