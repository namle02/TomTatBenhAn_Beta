using CommunityToolkit.Mvvm.ComponentModel;


namespace TomTatBenhAn_WPF.Repos._Model.PatientData
{
    public partial class ThongTinHanhChinhModel : ObservableObject
    {
        [ObservableProperty]
        private string? soBenhAn;

        [ObservableProperty]
        private string? bacSiDieuTri;

        [ObservableProperty]
        private string? soVaoVien;

        [ObservableProperty]
        private string? cccd;

        [ObservableProperty]
        private string? tenBN;

        [ObservableProperty]
        private string? ngaySinh;

        [ObservableProperty]
        private int? tuoi;

        [ObservableProperty]
        private string? gioiTinh;

        [ObservableProperty]
        private string? diaChi;

        [ObservableProperty]
        private string? soBHYT;

        [ObservableProperty]
        private DateTime? ngayVaoVien;

        [ObservableProperty]
        private DateTime? ngayRaVien;

        [ObservableProperty]
        private string? danToc;

        [ObservableProperty]
        private string? maYTe;

        [ObservableProperty]
        private string? thoiGianVaoVien;

        [ObservableProperty]
        private string? thoiGianRaVien;

        [ObservableProperty]
        private string? ketQuaDieuTri;
    }
}
