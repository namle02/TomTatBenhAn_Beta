using CommunityToolkit.Mvvm.ComponentModel;

namespace TomTatBenhAn_WPF.Repos.Model
{
    public partial class ThongTinBenhNhan : ObservableObject  
    {
        
        public string? TenBenhNhan {  get; set; }
       
        public string? NgaySinh { get; set; }

        public string? GioiTinh { get; set; }

        public int? Tuoi {  get; set; }
        
        public string? DiaChi { get; set; }

        public string? DanToc { get; set; }

        public string? BHYT { get; set; }

        public string? CCCD { get; set; }

        public string? SoBenhAn { get; set; }

        public string? MaYTe { get; set; }

        
    }
}   
