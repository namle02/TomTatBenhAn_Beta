using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.ViewModel.PageViewModel
{
    public partial class DashBoardVM : ObservableObject
    {
        [ObservableProperty] private string greeting = "Trang dashboard";
    }
}
