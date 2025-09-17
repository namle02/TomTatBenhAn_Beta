using CommunityToolkit.Mvvm.ComponentModel;
using TomTatBenhAn_WPF.Repos._Model.PatientPhacDo.PatientAllDataPhacDo;


namespace TomTatBenhAn_WPF.Repos._Model.PatientPhacDo
{
    public partial class PatientPhacDoAllData : ObservableObject
    {
        [ObservableProperty] private string soBenhAn = string.Empty;
        [ObservableProperty] private ChanDoanICD? chanDoanICD;
        [ObservableProperty] private List<NoiTruKhamBenh>? noiTruKhamBenh;
        [ObservableProperty] private List<NoiTruCls>? noiTruCLS;
        [ObservableProperty] private List<NoiTruToaThuoc>? noiTruToaThuoc;
        [ObservableProperty] private List<NoiTruChamSoc>? noiTruChamSoc;
    }
}
