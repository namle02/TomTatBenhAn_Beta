using CommunityToolkit.Mvvm.ComponentModel;

namespace TomTatBenhAn_WPF.Repos._Model.PatientPhacDo.PatientAllDataPhacDo
{
    public partial class NoiTruCls : ObservableObject
    {
        [ObservableProperty] private string tenNhomDichVu = string.Empty;
        [ObservableProperty] private string noiDungChiTiet = string.Empty;
        [ObservableProperty] private string tenPhongBan = string.Empty;
        [ObservableProperty] private string? tenDichVu;
        [ObservableProperty] private string? ketQua;
        [ObservableProperty] private string? mucBinhThuong;
        [ObservableProperty] private string? mucBinhThuongMin;
        [ObservableProperty] private string? mucBinhThuongMax;
        [ObservableProperty] private string? batThuong;
        [ObservableProperty] private DateTime? thoiGianThucHien;
        [ObservableProperty] private string? ketLuan;
        [ObservableProperty] private string? moTa_Text;
    }
}
