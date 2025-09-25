using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using TomTatBenhAn_WPF.Repos._Model.PatientPhacDo;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.ViewModel.PageViewModel
{
    public partial class KiemTraPhacDoVM : ObservableObject
    {
        private readonly IDataMapper _dataMapper;
        private readonly IPhacDoServices _phacDoServices;
        private readonly IBangKiemServices _bangKiemServices;
        private readonly IKiemTraPhacDoServices _kiemTraPhacDoServices;
        private readonly IPhacDoReportServices _phacDoReportServices;

        [ObservableProperty] private PatientPhacDoAllData patientData = new PatientPhacDoAllData();

        [ObservableProperty] private ObservableCollection<PhacDoItemDTO> danhSachPhacDo = new ObservableCollection<PhacDoItemDTO>();
        [ObservableProperty] private ObservableCollection<BangKiemResponseDTO> danhSachBangKiem = new ObservableCollection<BangKiemResponseDTO>();
        [ObservableProperty] private ObservableCollection<PhacDoItemDTO> danhSachPhacDoPhuHop = new ObservableCollection<PhacDoItemDTO>();
        [ObservableProperty] private ObservableCollection<BangKiemResponseDTO> danhSachBangKiemDaDanhGia = new ObservableCollection<BangKiemResponseDTO>();

        [ObservableProperty] private BangKiemResponseDTO bangKiemDaDanhGia = new BangKiemResponseDTO();
        [ObservableProperty] private PhacDoItemDTO? selectedPhacDo;

        [ObservableProperty] private bool isLoading = false;
        [ObservableProperty] private bool isAddMorePhacDo = false;
        [ObservableProperty] private bool isViewBangKiem = false;
        [ObservableProperty] private bool hasNoBangKiem = true;

        public KiemTraPhacDoVM(IDataMapper dataMapper, IBangKiemServices bangKiemServices, IPhacDoServices phacDoServices, IKiemTraPhacDoServices kiemTraPhacDoServices, IPhacDoReportServices phacDoReportServices)
        {
            _dataMapper = dataMapper;
            _bangKiemServices = bangKiemServices;
            _phacDoServices = phacDoServices;
            _kiemTraPhacDoServices = kiemTraPhacDoServices;
            _phacDoReportServices = phacDoReportServices;

            _ = Task.Run(async () =>
            {
                try { await GetPhacDoAndBangKiem(); }
                catch (Exception ex) { MessageBox.Show($"Lỗi: {ex.Message}"); }
            });
        }

        private async Task GetPhacDoAndBangKiem()
        {
            DanhSachPhacDo = new ObservableCollection<PhacDoItemDTO>((await _phacDoServices.GetAllPhacDoAsync()).Data!);
            DanhSachBangKiem = new ObservableCollection<BangKiemResponseDTO>((await _bangKiemServices.GetAllAsync()).Data!);
        }

        [RelayCommand]
        private async Task KiemTraBenhNhan(string soBenhAn)
        {
            try
            {
                IsLoading = true;
                DanhSachBangKiemDaDanhGia.Clear();

                PatientData = await _dataMapper.GetAllPatientPhacDoData(soBenhAn);

                var list = _kiemTraPhacDoServices.TimPhacDoPhuHop(PatientData, DanhSachPhacDo);
                // Chọn 1 trong 2:
                DanhSachPhacDoPhuHop.ResetWith(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task KiemTraTuanThuPhacDo(string PhacDoId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PhacDoId)) return;
                var phacDo = DanhSachPhacDoPhuHop.FirstOrDefault(x => x._id == PhacDoId) ?? DanhSachPhacDo.FirstOrDefault(x => x._id == PhacDoId);
                if (phacDo is null) { MessageBox.Show("Không tìm thấy phác đồ."); return; }

                var bangKiem = DanhSachBangKiem.FirstOrDefault(b => b.PhacDoId == PhacDoId);
                if (bangKiem is null) { MessageBox.Show("Chưa có bảng kiểm cho phác đồ này."); return; }

                IsLoading = true;
                var result = await _kiemTraPhacDoServices.DanhGiaTuanThuPhacDoAsync(PatientData, phacDo, bangKiem);
                if (result.Success && result.Data is not null)
                {
                    DanhSachBangKiemDaDanhGia.Add(result.Data);
                    MessageBox.Show("Đánh giá tuân thủ phác đồ thành công.");
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void AddPhacDo()
        {
            if (SelectedPhacDo is null) return;
            if (DanhSachPhacDoPhuHop.Any(x => x._id == SelectedPhacDo._id)) return; // tránh trùng
            DanhSachPhacDoPhuHop.Add(SelectedPhacDo);
        }

        [RelayCommand]
        private void RemovePhacDo(string PhacDoId)
        {
            if (string.IsNullOrWhiteSpace(PhacDoId)) return;

            for (int i = DanhSachPhacDoPhuHop.Count - 1; i >= 0; i--)
                if (DanhSachPhacDoPhuHop[i]?._id == PhacDoId)
                    DanhSachPhacDoPhuHop.RemoveAt(i);
        }

        [RelayCommand]
        private void PreviewBangKiem(string BangKiemId)
        {
            IsViewBangKiem = true;
            foreach (var item in DanhSachBangKiemDaDanhGia)
            {
                if (item.BangKiemId == BangKiemId)
                {
                    BangKiemDaDanhGia = item;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu bảng kiểm đã đánh giá ra file Word
        /// </summary>
        [RelayCommand]
        private async Task XuatBangKiem(string bangKiemId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(bangKiemId))
                {
                    MessageBox.Show("Không tìm thấy ID bảng kiểm.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Tìm bảng kiểm đã đánh giá
                var bangKiemDaDanhGia = DanhSachBangKiemDaDanhGia.FirstOrDefault(x => x.BangKiemId == bangKiemId);
                if (bangKiemDaDanhGia == null)
                {
                    MessageBox.Show("Không tìm thấy bảng kiểm đã đánh giá.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                IsLoading = true;

                // Tạo thư mục temp để lưu file tạm
                var tempDir = Path.Combine(Path.GetTempPath(), "BangKiemExport");
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                // Download file Word gốc từ server
                var tempFilePath = Path.Combine(tempDir, $"temp_{bangKiemId}.docx");
                var downloadResult = await _bangKiemServices.DownloadOriginalFileAsync(bangKiemId, tempFilePath);

                if (!downloadResult.Success)
                {
                    MessageBox.Show($"Không thể tải file Word gốc: {downloadResult.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Tạo file output với dữ liệu đã đánh giá
                var outputFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{PatientData.ThongTinHanhChinh!.SoBenhAn}.docx";
                var outputPath = await CreateOutputFileWithData(tempFilePath, bangKiemDaDanhGia, outputFileName);

                if (!string.IsNullOrWhiteSpace(outputPath))
                {
                    MessageBox.Show($"Xuất bảng kiểm thành công!\nFile đã lưu tại: {outputPath}", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi xuất bảng kiểm.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Dọn dẹp file tạm
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất bảng kiểm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Tạo file Word output với dữ liệu đã đánh giá
        /// </summary>
        private async Task<string?> CreateOutputFileWithData(string originalFilePath, BangKiemResponseDTO bangKiemData, string outputFileName)
        {
            try
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string thisYear = DateTime.Now.ToString("yyyy");
                string thisMonth = DateTime.Now.ToString("MM");
                string folderPath = Path.Combine(desktop, "Bảng kiểm tuân thủ phác đô", thisYear, thisMonth, outputFileName);

                var outputPath = folderPath;
                var resultPath = await _phacDoReportServices.CreateOutputFileWithDataAsync(originalFilePath, outputPath, bangKiemData, PatientData);
                if (string.IsNullOrWhiteSpace(resultPath))
                {
                    MessageBox.Show("Không thể tạo file Word output.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                return resultPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo file đánh giá: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        [RelayCommand]
        private void ClosePreviewBangKiem()
        {
            IsViewBangKiem = false;
        }

        partial void OnDanhSachBangKiemChanged(ObservableCollection<BangKiemResponseDTO> value)
        {
            HasNoBangKiem = value.Count == 0;
        }

    }
    public static class OcExtensions
    {
        public static void ResetWith<T>(this ObservableCollection<T> oc, IEnumerable<T> items)
        {
            oc.Clear();
            foreach (var i in items) oc.Add(i);
        }
    }
}
