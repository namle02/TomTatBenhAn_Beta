using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Repos.Dto
{
    public partial class BangKiemItemDTO : ObservableObject
    {
        [ObservableProperty] private bool isChecked;
        [ObservableProperty] private string phacDo = string.Empty;
    }
}
