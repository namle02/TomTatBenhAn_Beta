using CommunityToolkit.Mvvm.ComponentModel;

namespace TomTatBenhAn_WPF.Repos._Model.PatientData
{
    public partial class KetQuaXetNghiemModel : ObservableObject
    {
        [ObservableProperty]
        private string? tenNhomDichVu;

        [ObservableProperty]
        private string? noiDungChiTiet;

        [ObservableProperty]
        private string? tenPhongBan;

        [ObservableProperty]
        private string? tenDichVu;

        [ObservableProperty]
        private string? ketQua;

        [ObservableProperty]
        private string? mucBinhThuong;

        [ObservableProperty]
        private string? mucBinhThuongMin;

        [ObservableProperty]
        private string? mucBinhThuongMax;

        [ObservableProperty]
        private string? batThuong;

        [ObservableProperty]
        private DateTime? thoiGianThucHien;

        [ObservableProperty]
        private string? ketLuan;

        [ObservableProperty]
        private string? moTa_Text;
    }
}
