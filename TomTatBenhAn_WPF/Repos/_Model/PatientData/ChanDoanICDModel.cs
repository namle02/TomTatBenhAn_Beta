using CommunityToolkit.Mvvm.ComponentModel;

namespace TomTatBenhAn_WPF.Repos._Model
{
    public partial class ChanDoanICDModel : ObservableObject
    {
        [ObservableProperty] private string? benhChinhVaoVien;
        [ObservableProperty] private string? maICDChinhVaoVien;
        [ObservableProperty] private string? benhPhuVaoVien;
        [ObservableProperty] private string? maICDPhuVaoVien;
        [ObservableProperty] private string? benhChinhRaVien;
        [ObservableProperty] private string? maICDChinhRaVien;
        [ObservableProperty] private string? benhKemTheoRaVien;
        [ObservableProperty] private string? maICDKemTheoRaVien;
    }
}
