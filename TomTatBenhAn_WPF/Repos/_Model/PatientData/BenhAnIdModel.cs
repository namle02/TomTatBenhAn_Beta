using CommunityToolkit.Mvvm.ComponentModel;


namespace TomTatBenhAn_WPF.Repos._Model.PatientData
{
    public partial class BenhAnIdModel : ObservableObject
    {
        [ObservableProperty] private string soBenhAn = string.Empty;
        [ObservableProperty] private string benhAnId = string.Empty;
        [ObservableProperty] private string benhAnTongQuatId = string.Empty;
    }
}
