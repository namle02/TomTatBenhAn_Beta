using CommunityToolkit.Mvvm.ComponentModel;

namespace TomTatBenhAn_WPF.Repos.Model
{
    public partial class HanhChinhModel : ObservableObject
    {
        [ObservableProperty]
        private string? tenBenhNhan;
        [ObservableProperty]
        private string? ngaySinh ;
        [ObservableProperty]
        private string? gioiTinh ;
        [ObservableProperty]
        private int? tuoi ;
        [ObservableProperty]
        private string? diaChi ;
        [ObservableProperty]
        private string? danToc ;
        [ObservableProperty]
        private string? bHYT ;
        [ObservableProperty]
        private string? cCCD ;
        [ObservableProperty]
        private string? soBenhAn ;
        [ObservableProperty]
        private string? maYTe ;
        [ObservableProperty]
        private DateTime? vaoVien ;
        [ObservableProperty]
        private DateTime? raVien ;
    }
}   
