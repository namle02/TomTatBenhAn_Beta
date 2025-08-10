using CommunityToolkit.Mvvm.ComponentModel;

public class BenhAnChiTietModel : ObservableObject
{
    public string? LyDoVaoVien { get; set; }
    public string? QuaTrinhBenhLy { get; set; }
    public string? TienSuBenh { get; set; }
    public string? HuongDieuTri { get; set; }
    
    public string? LydoNoiKhoaTrue { get; set; }
    public string? LydoPTTTTrue { get; set; }
}
