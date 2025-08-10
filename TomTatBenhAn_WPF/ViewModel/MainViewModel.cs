using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TomTatBenhAn_WPF.ViewModel.ControlViewModel;

namespace TomTatBenhAn_WPF.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private SideBarViewModel sideBarVM;

        [ObservableProperty]
        private ContentViewModel contentVM;


        public MainViewModel(SideBarViewModel _sideBarViewModel, ContentViewModel _contentVM)
        {
            sideBarVM = _sideBarViewModel;
            contentVM = _contentVM;
        }
    }
}
