using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTatBenhAn_WPF.Repos.Model;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IAiService
    {
        
        Task<Dictionary<string, string>> TomTatBenhLyAsync(string quaTrinhBenhLy);
        Task<Dictionary<string, string>> HuongDieuTriAsync(string DienBien, string LoiDanThayThuoc);
        Task<string> TomTatKetQuaXetNghiemCSLAsync(string chuanDoanChinh, List<KetQuaXetNghiemCLSModel> danhSachKQXN);
    }
}
