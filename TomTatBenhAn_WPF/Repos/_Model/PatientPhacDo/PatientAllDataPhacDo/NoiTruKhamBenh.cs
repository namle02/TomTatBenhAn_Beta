using CommunityToolkit.Mvvm.ComponentModel;

namespace TomTatBenhAn_WPF.Repos._Model.PatientPhacDo.PatientAllDataPhacDo
{
    public partial class NoiTruKhamBenh : ObservableObject
    {
        [ObservableProperty] private DateTime? thoiGianThucHien;
        [ObservableProperty] private string? dinhBenh;
        [ObservableProperty] private string? dienBien;
        [ObservableProperty] private string? loiDan;
        [ObservableProperty] private string? cheDoAn;
        [ObservableProperty] private string? cheDoChamSoc;
    }
}
