using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Repos.Model
{
     public partial class DienBienModel:ObservableObject
    {
        public string? DienBien {  get; set; }
        public string? LoiDanThayThuoc {  get; set; }
    }
}
