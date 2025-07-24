using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Repos.Model
{
    public partial class KetQuaXetNghiemCLSModel:ObservableObject
    {
        public string? TenNhomDichVu {  get; set; }
        public string? NoiDungChiTiet {  get; set; }
        public string?TenPhongBan {  get; set; }
        public string? TenDichvu {  get; set; }
        public string? KetQua { get; set; }
        public string? MucBinhThuong {  get; set; }
        public string? MucBinhThuongMin {  get; set; }
        public string? MucBinhThuongMax {  get; set; }
        public string? BatThuong { get; set; }

        public String? ThoiGianThucHIen {  get; set; }
        public string? KetLuan {  get; set; }
        public string? MoTa {  get; set; }

    }
}
