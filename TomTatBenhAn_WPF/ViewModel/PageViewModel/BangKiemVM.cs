using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.ViewModel.PageViewModel
{
    public partial class BangKiemVM : ObservableObject
    {
        private readonly IBangKiemServices _bangKiemServices;
        private readonly IPhacDoServices _phacDoServices;
        private readonly IPhacDoReportServices _phacDoReportServices;
        [ObservableProperty] private ObservableCollection<BangKiemResponseDTO> danhSachBangKiem = new();
        [ObservableProperty] private ObservableCollection<PhacDoItemDTO> danhSachPhacDo = new();
        [ObservableProperty] private BangKiemResponseDTO? selectedBangKiem;
        [ObservableProperty] private BangKiemRequestDTO? bangKiemMoi = new();

        [ObservableProperty] private string selectedPhacDo = string.Empty;
        [ObservableProperty] private string bangKiemPath = string.Empty;

        [ObservableProperty] private bool isAddBangKiem = false;
        [ObservableProperty] private bool isEmptyDataGrid = true;
        [ObservableProperty] private bool isLoading = false;
        [ObservableProperty] private bool isViewBangKiem = false;
      
        public BangKiemVM(IBangKiemServices bangKiemServices, IPhacDoServices phacDoServices, IPhacDoReportServices phacDoReportServices)
        {
            _bangKiemServices = bangKiemServices;
            _phacDoServices = phacDoServices;
            _phacDoReportServices = phacDoReportServices;

            _ = GetDanhSachBangKiem();
            _ = GetDanhSachPhacDo();
        }

        [RelayCommand]
        private async Task GetDanhSachBangKiem()
        {
            IsLoading = true;
            DanhSachBangKiem.Clear();
            var response = await _bangKiemServices.GetAllAsync();
            if (response.Data is not null)
            {
                foreach (var item in response.Data)
                {
                    if (!DanhSachBangKiem.Any(x => x.BangKiemId == item.BangKiemId))
                    {
                        DanhSachBangKiem.Add(item);
                    }
                }
                if (DanhSachBangKiem.Count() > 0)
                {
                    IsEmptyDataGrid = false;
                }
            }
            IsLoading = false;
        }

        [RelayCommand]
        private void OpenModalAddBangKiemr()
        {
            IsAddBangKiem = true;
            BangKiemMoi = new BangKiemRequestDTO();
        }

        [RelayCommand]
        private void CloseModalAddBangKiemr()
        {
            IsAddBangKiem = false;
            BangKiemMoi = null;
            BangKiemPath = string.Empty;
        }

        [RelayCommand]
        private async Task SaveBangKiem()
        {
            if (BangKiemMoi is null)
            {
                MessageBox.Show("Dữ liệu không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate
            if (string.IsNullOrWhiteSpace(BangKiemMoi.TenBangKiem))
            {
                MessageBox.Show("Vui lòng nhập Tên bảng kiểm.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedPhacDo))
            {
                MessageBox.Show("Vui lòng chọn Phác đồ liên kết.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(BangKiemPath))
            {
                MessageBox.Show("Vui lòng chọn file Word bảng kiểm.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;
            try
            {
                // Extract table từ file Word để lấy dữ liệu bảng kiểm
                var requestDto = _phacDoReportServices.ExtractTableBangDanhGiaFromWord(BangKiemPath, SelectedPhacDo, BangKiemMoi.TenBangKiem);

                // Gọi API tạo bảng kiểm với file Word gốc
                var result = await _bangKiemServices.CreateWithFileAsync(requestDto, BangKiemPath);
                
                if (result.Success && result.Data is not null)
                {
                    DanhSachBangKiem.Insert(0, result.Data);
                    IsEmptyDataGrid = DanhSachBangKiem.Count == 0;
                    MessageBox.Show("Import bảng kiểm từ Word thành công.");
                    
                    // Reset form sau khi thành công
                    CloseModalAddBangKiemr();
                }
                else
                {
                    MessageBox.Show(result.GetErrorMessage());
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task XoaBangKiem(string BangKiemId)
        {
            IsLoading = true;
            try
            {
                var result = await _bangKiemServices.DeleteAsync(BangKiemId);
                if (result.Data is not null)
                {
                    MessageBox.Show("Xóa bảng kiểm thành công");
                }
                await GetDanhSachBangKiem();
                IsLoading = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa bảng kiểm: {ex.Message}");
            }
        }

        [RelayCommand]
        private void XemNoiDungBangKiem(string BangKiemId)
        {
            
            foreach(var item in DanhSachBangKiem)
            {
                if (item.BangKiemId == BangKiemId)
                {
                    SelectedBangKiem = item;
                    break;
                }
            }
            IsViewBangKiem = true;
        }

        [RelayCommand]
        private void ClosePreviewBangKiem()
        {
            IsViewBangKiem = false;
            SelectedBangKiem = null;
        }

        [RelayCommand]
        private void DragFile(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    var filePath = files[0];
                    var fileExt = System.IO.Path.GetExtension(filePath).ToLower();
                    
                    // Kiểm tra định dạng file
                    if (fileExt != ".docx" && fileExt != ".doc")
                    {
                        MessageBox.Show("Vui lòng thả file Word (.docx hoặc .doc).", "Định dạng file không hợp lệ", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    
                    // Kiểm tra kích thước file (giới hạn 10MB)
                    var fileInfo = new System.IO.FileInfo(filePath);
                    if (fileInfo.Length > 10 * 1024 * 1024)
                    {
                        MessageBox.Show("File quá lớn. Vui lòng chọn file nhỏ hơn 10MB.", "File quá lớn", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    
                    BangKiemPath = filePath;
                    MessageBox.Show($"Đã chọn file: {System.IO.Path.GetFileName(filePath)}", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        [RelayCommand]
        private void OpenFile()
        {
            var dlg = new OpenFileDialog
            {
                Title = "Chọn file Word bảng kiểm",
                Filter = "Word Documents (*.docx;*.doc)|*.docx;*.doc|All files (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                var filePath = dlg.FileName;
                var fileInfo = new System.IO.FileInfo(filePath);
                
                // Kiểm tra kích thước file (giới hạn 10MB)
                if (fileInfo.Length > 10 * 1024 * 1024)
                {
                    MessageBox.Show("File quá lớn. Vui lòng chọn file nhỏ hơn 10MB.", "File quá lớn", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                BangKiemPath = filePath;
                MessageBox.Show($"Đã chọn file: {System.IO.Path.GetFileName(filePath)}", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        [RelayCommand]
        private void CancelFile()
        {
            BangKiemPath = string.Empty;
        }

        private async Task GetDanhSachPhacDo()
        {
            var response = await _phacDoServices.GetAllPhacDoAsync();
            {
                if (response.Data is not null)
                {
                    foreach (var item in response.Data)
                    {
                        if (!DanhSachPhacDo.Any(x => x._id == item._id))
                        {
                            DanhSachPhacDo.Add(item);
                        }
                    }
                }
            }
        }

    }
}
