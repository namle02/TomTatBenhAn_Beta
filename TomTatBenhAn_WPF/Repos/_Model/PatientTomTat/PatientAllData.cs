using CommunityToolkit.Mvvm.ComponentModel;
using TomTatBenhAn_WPF.Repos._Model.PatientData;
using TomTatBenhAn_WPF.Repos._Model.PatientTomTat.PatientData;

namespace TomTatBenhAn_WPF.Repos._Model
{
    public partial class PatientAllData : ObservableObject
    {
        [ObservableProperty] private string? reportNumber;
        [ObservableProperty] private string? doctorName;
        [ObservableProperty] private LoaiBenhAnModel? loaiBenhAn;
        [ObservableProperty] private List<ThongTinKhamBenhModel>? thongTinKhamBenh;
        [ObservableProperty] private List<BenhAnIdModel>? danhSachBenhAn;
        [ObservableProperty] private List<ThongTinHanhChinhModel>? thongTinHanhChinh;
        [ObservableProperty] private List<ChanDoanICDModel>? chanDoanIcd;
        [ObservableProperty] private List<TinhTrangNguoiBenhRaVienModel>? tinhTrangNguoiBenhRaVien;
        [ObservableProperty] private List<KetQuaXetNghiemModel>? ketQuaXetNghien;
        [ObservableProperty] private List<DataTomTat>? thongTinTomTat;
    }
}
