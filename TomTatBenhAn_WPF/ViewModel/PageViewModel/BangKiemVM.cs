using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Services.Interface;
using System.Threading.Tasks;
using System.Linq;

namespace TomTatBenhAn_WPF.ViewModel.PageViewModel
{
    public partial class BangKiemVM : ObservableObject
    {
        private readonly IBangKiemServices _bangKiemServices;
        private readonly IPhacDoServices _phacDoServices;
        [ObservableProperty] private ObservableCollection<BangKiemResponseDTO> danhSachBangKiem = new();
        [ObservableProperty] private ObservableCollection<PhacDoItemDTO> danhSachPhacDo = new();
        [ObservableProperty] private BangKiemRequestDTO? bangKiemMoi = new();
        [ObservableProperty]
        private ObservableCollection<string> noiDungKiemTra = new ObservableCollection<string>
        {
            "Tiền sử","Bệnh sử","Khám", "Cận lâm sàng", "Chẩn đoán", "Xử trí cấp cứu", "Phẫu thuật", "Bảo tồn", "Thuốc", "Theo dõi", "Phân cấp chăm sóc", "Chăm sóc hàng ngày", "Giáo dục sức khỏe", "Tiêu chuẩn ra viện", "Hướng dẫn ra viện"
        };
        [ObservableProperty] private string selectedPhacDo = string.Empty;
        [ObservableProperty] private bool isAddBangKiem = false;
        [ObservableProperty] private bool isEmptyDataGrid = true;
        [ObservableProperty] private bool isLoading = false;
        [ObservableProperty] private bool isViewBangKiem = false;
        [ObservableProperty] private BangKiemResponseDTO? selectedBangKiem;
        [ObservableProperty] private ObservableCollection<TieuChiHienThi> danhSachTieuChiHienThi = new();
        public BangKiemVM(IBangKiemServices bangKiemServices, IPhacDoServices phacDoServices)
        {
            _bangKiemServices = bangKiemServices;
            _phacDoServices = phacDoServices;

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

            IsLoading = true;
            try
            {
                BangKiemMoi.PhacDoId = SelectedPhacDo;
                var result = await _bangKiemServices.CreateAsync(BangKiemMoi);
                if (result.Data is not null && result.Success)
                {
                    // Cập nhật danh sách và đóng modal
                    if (!DanhSachBangKiem.Any(x => x.BangKiemId == result.Data.BangKiemId))
                    {
                        DanhSachBangKiem.Insert(0, result.Data);
                    }
                    IsEmptyDataGrid = DanhSachBangKiem.Count == 0;
                    MessageBox.Show("Tạo bảng kiểm thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseModalAddBangKiemr();
                }
                else
                {
                    MessageBox.Show(result.GetErrorMessage(), "Không thành công", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private void ThemHangMuc(string NoiDungKiemTra)
        {
            if (BangKiemMoi is not null)
            {
                int nextStt = GetNextTieuChiStt();
                switch (NoiDungKiemTra)
                {
                    case "1":
                        BangKiemMoi.DanhGiaVaChanDoan.DanhSachNoiDung.Add(new NoiDungKiemTra
                        {
                            DanhSachTieuChi = new ObservableCollection<TieuChiKiemTra>
                            {
                                new TieuChiKiemTra { Stt = nextStt.ToString() }
                            }
                        });
                        break;
                    case "2":
                        BangKiemMoi.DieuTri.DanhSachNoiDung.Add(new NoiDungKiemTra
                        {
                            DanhSachTieuChi = new ObservableCollection<TieuChiKiemTra>
                            {
                                new TieuChiKiemTra { Stt = nextStt.ToString() }
                            }
                        });
                        break;
                    case "3":
                        BangKiemMoi.ChamSoc.DanhSachNoiDung.Add(new NoiDungKiemTra
                        {
                            DanhSachTieuChi = new ObservableCollection<TieuChiKiemTra>
                            {
                                new TieuChiKiemTra { Stt = nextStt.ToString() }
                            }
                        });
                        break;
                    case "4":
                        BangKiemMoi.RaVien.DanhSachNoiDung.Add(new NoiDungKiemTra
                        {
                            DanhSachTieuChi = new ObservableCollection<TieuChiKiemTra>
                            {
                                new TieuChiKiemTra { Stt = nextStt.ToString() }
                            }
                        });
                        break;
                }
            }

        }

        private int GetNextTieuChiStt()
        {
            if (BangKiemMoi is null) return 1;
            int total = 0;
            total += BangKiemMoi.DanhGiaVaChanDoan?.DanhSachNoiDung?.Sum(nd => nd.DanhSachTieuChi?.Count ?? 0) ?? 0;
            total += BangKiemMoi.DieuTri?.DanhSachNoiDung?.Sum(nd => nd.DanhSachTieuChi?.Count ?? 0) ?? 0;
            total += BangKiemMoi.ChamSoc?.DanhSachNoiDung?.Sum(nd => nd.DanhSachTieuChi?.Count ?? 0) ?? 0;
            total += BangKiemMoi.RaVien?.DanhSachNoiDung?.Sum(nd => nd.DanhSachTieuChi?.Count ?? 0) ?? 0;
            return total + 1;
        }

        [RelayCommand]
        private void XoaNoiDungDanhGia(NoiDungKiemTra item)
        {
            if (BangKiemMoi is null || item is null) return;
            BangKiemMoi.DanhGiaVaChanDoan.DanhSachNoiDung.Remove(item);
        }

        [RelayCommand]
        private void XoaNoiDungDieuTri(NoiDungKiemTra item)
        {
            if (BangKiemMoi is null || item is null) return;
            BangKiemMoi.DieuTri.DanhSachNoiDung.Remove(item);
        }

        [RelayCommand]
        private void XoaNoiDungChamSoc(NoiDungKiemTra item)
        {
            if (BangKiemMoi is null || item is null) return;
            BangKiemMoi.ChamSoc.DanhSachNoiDung.Remove(item);
        }

        [RelayCommand]
        private void XoaNoiDungRaVien(NoiDungKiemTra item)
        {
            if (BangKiemMoi is null || item is null) return;
            BangKiemMoi.RaVien.DanhSachNoiDung.Remove(item);
        }

        [RelayCommand]
        private void XemNoiDungBangKiem(BangKiemResponseDTO? item)
        {
            if (item is not null)
            {
                SelectedBangKiem = item;
                BuildDanhSachTieuChiHienThi(item);
                IsViewBangKiem = true;
            }
            else
            {
                // toggle/close when no item is provided (e.g., close button)
                IsViewBangKiem = false;
                DanhSachTieuChiHienThi.Clear();
                SelectedBangKiem = null;
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

        private void BuildDanhSachTieuChiHienThi(BangKiemResponseDTO bk)
        {
            DanhSachTieuChiHienThi.Clear();

            void addRows(HangMucKiemTra hm)
            {
                if (hm?.DanhSachNoiDung is null) return;
                foreach (var nd in hm.DanhSachNoiDung)
                {
                    if (nd?.DanhSachTieuChi is null) continue;
                    foreach (var tc in nd.DanhSachTieuChi)
                    {
                        DanhSachTieuChiHienThi.Add(new TieuChiHienThi
                        {
                            Stt = tc.Stt,
                            TenNoiDungKiemTra = nd.TenNoiDungKiemTra,
                            YeuCauDatDuoc = tc.YeuCauDatDuoc,
                            Dat = tc.Dat,
                            KhongDat = tc.KhongDat,
                            LyDoKhongDat = tc.LyDoKhongDat,
                            KhongApDung = tc.KhongApDung,
                            IsImportant = tc.IsImportant
                        });
                    }
                }
            }

            addRows(bk.DanhGiaVaChanDoan);
            addRows(bk.DieuTri);
            addRows(bk.ChamSoc);
            addRows(bk.RaVien);
        }
    }
}
