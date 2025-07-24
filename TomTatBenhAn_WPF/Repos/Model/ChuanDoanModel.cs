using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Repos.Model
{
    public partial class ChuanDoanModel : ObservableObject
    {
       
        public string? benhChinhVaoVien {  get; set; }
        
        public string? benhPhuVaoVien { get; set; }
       
        public string? icdVaoKhoaChinh {  get; set; }
        
        public string? icdVaoKhoaPhu { get; set; }

        public string? benhChinhRaVien { get; set; }

        public string? benhPhuRaVien { get; set; }

        public string? icdRaVienChinh { get; set; }

        public string? icdRaVienPhu { get; set; }

    }
}
