using CommunityToolkit.Mvvm.ComponentModel;
using TomTatBenhAn_WPF.Repos._Model.PatientData;

namespace TomTatBenhAn_WPF.Repos._Model
{
    public partial class PatientAllData : ObservableObject
    {

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
