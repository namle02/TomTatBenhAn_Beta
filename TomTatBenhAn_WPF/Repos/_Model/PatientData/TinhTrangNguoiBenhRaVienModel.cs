using CommunityToolkit.Mvvm.ComponentModel;


namespace TomTatBenhAn_WPF.Repos._Model.PatientData
{
    public partial class TinhTrangNguoiBenhRaVienModel : ObservableObject
    {
        [ObservableProperty]
        private string? dienBien;

        [ObservableProperty]
        private string? loiDanThayThuoc;
        [ObservableProperty]
        private string? ppdt;
    }
}
