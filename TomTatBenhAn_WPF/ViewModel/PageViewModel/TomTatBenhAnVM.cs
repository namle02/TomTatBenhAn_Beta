using CommunityToolkit.Mvvm.ComponentModel;
using TomTatBenhAn_WPF.ViewModel.ControlViewModel;

namespace TomTatBenhAn_WPF.ViewModel.PageViewModel
{
    public partial class TomTatBenhAnVM : ObservableObject
    {
        [ObservableProperty] private SideBarViewModel sideBarVM;
        [ObservableProperty] private ContentViewModel contentVM;
        public TomTatBenhAnVM(SideBarViewModel sideBarVM, ContentViewModel contentVM)
        {
            this.sideBarVM = sideBarVM;
            this.contentVM = contentVM;
        }

    }
}
