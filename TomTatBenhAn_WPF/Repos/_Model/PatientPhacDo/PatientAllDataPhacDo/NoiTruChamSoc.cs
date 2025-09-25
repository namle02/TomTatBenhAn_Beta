using CommunityToolkit.Mvvm.ComponentModel;

namespace TomTatBenhAn_WPF.Repos._Model.PatientPhacDo.PatientAllDataPhacDo
{
    public partial class NoiTruChamSoc : ObservableObject
    {
        [ObservableProperty] private string dienBenh = string.Empty;
        [ObservableProperty] private string yLenhChamSoc = string.Empty;
        [ObservableProperty] private DateTime? ngayThucHien;
    }
}
