using CommunityToolkit.Mvvm.ComponentModel;

namespace TomTatBenhAn_WPF.Repos._Model.PatientPhacDo.PatientAllDataPhacDo
{
    public partial class NoiTruToaThuoc : ObservableObject
    {
        [ObservableProperty] private string? duocId;
        [ObservableProperty] private string? tenDuocDayDu;
        [ObservableProperty] private string? soLuong;
        [ObservableProperty] private string? soLanTrenNgay;
        [ObservableProperty] private DateTime? ngayTao;
    }
}
