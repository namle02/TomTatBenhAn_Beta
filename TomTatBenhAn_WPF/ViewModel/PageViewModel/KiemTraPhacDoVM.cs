using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Drawing;
using System.Collections.ObjectModel;
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

        public KiemTraPhacDoVM(IDataMapper dataMapper, IBangKiemServices bangKiemServices, IPhacDoServices phacDoServices, IKiemTraPhacDoServices kiemTraPhacDoServices)
        {
            _dataMapper = dataMapper;
            _bangKiemServices = bangKiemServices;
            _phacDoServices = phacDoServices;
            _kiemTraPhacDoServices = kiemTraPhacDoServices;

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
            foreach(var item in DanhSachBangKiemDaDanhGia)
            {
                if(item.BangKiemId == BangKiemId)
                {
                    BangKiemDaDanhGia = item;
                }
            }
        }
        [RelayCommand]
        private void ClosePreviewBangKiem()
        {
            IsViewBangKiem = false;
        }
        

        partial void OnDanhSachBangKiemDaDanhGiaChanged(ObservableCollection<BangKiemResponseDTO>? oldValue, ObservableCollection<BangKiemResponseDTO> newValue)
        {
            if(newValue.Count != 0)
            {
                HasNoBangKiem = false;
            }
            else
            {
                HasNoBangKiem = true;
            }
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
