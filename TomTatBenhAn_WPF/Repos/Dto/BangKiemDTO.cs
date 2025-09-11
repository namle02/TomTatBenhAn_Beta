using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace TomTatBenhAn_WPF.Repos.Dto
{

    public partial class HangMucKiemTra : ObservableObject
    {
        [ObservableProperty] private string stt = string.Empty;
        [ObservableProperty] private string tenHangMucKiemTra = string.Empty;
        [ObservableProperty] ObservableCollection<NoiDungKiemTra> danhSachNoiDung = new();

    }

    public partial class NoiDungKiemTra : ObservableObject
    {
        [ObservableProperty] private string tenNoiDungKiemTra = string.Empty;
        [ObservableProperty] ObservableCollection<TieuChiKiemTra> danhSachTieuChi = new();
    }

    public partial class TieuChiKiemTra : ObservableObject
    {
        [ObservableProperty] private string stt = string.Empty;
        [ObservableProperty] private string yeuCauDatDuoc = string.Empty;
        [ObservableProperty] private bool dat;
        [ObservableProperty] private bool khongDat;
        [ObservableProperty] private string lyDoKhongDat = string.Empty;
        [ObservableProperty] private bool khongApDung;
        [ObservableProperty] private bool isImportant;
    }

    public class TieuChiHienThi
    {
        public string Stt { get; set; } = string.Empty;
        public string TenNoiDungKiemTra { get; set; } = string.Empty;
        public string YeuCauDatDuoc { get; set; } = string.Empty;
        public bool Dat { get; set; }
        public bool KhongDat { get; set; }
        public string LyDoKhongDat { get; set; } = string.Empty;
        public bool KhongApDung { get; set; }
        public bool IsImportant { get; set; }
    }


}
