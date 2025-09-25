using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TomTatBenhAn_WPF.Message;

namespace TomTatBenhAn_WPF.ViewModel.ControlViewModel
{
    public partial class SideBarNavigationViewModel : ObservableObject
    {
        [ObservableProperty] private string greeting = "Xin chào bác sĩ Nguyễn Văn A";

        [RelayCommand]
        private void Navigation(string Page)
        {
            WeakReferenceMessenger.Default.Send(new NavigationMessage(Page));
        }
    }
}
