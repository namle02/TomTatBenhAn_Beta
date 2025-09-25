using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.ViewModel.ControlViewModel;
using TomTatBenhAn_WPF.ViewModel.PageViewModel;


namespace TomTatBenhAn_WPF.ViewModel
{
    public partial class MainViewModel : ObservableRecipient, IRecipient<NavigationMessage>, IRecipient<SideBarStateMessage>
    {
        private readonly IServiceProvider serviceProvider;
        [ObservableProperty] private HeaderViewModel _headerViewModel;
        [ObservableProperty] private SideBarNavigationViewModel _sideBaerNavigation;

        [ObservableProperty] private object currentPage;

        [ObservableProperty] private bool isSideBarOpen;

        public MainViewModel(IServiceProvider serviceProvider, HeaderViewModel headerViewModel, SideBarNavigationViewModel sideBaerNavigation)
        {
            this.serviceProvider = serviceProvider;
            CurrentPage = serviceProvider.GetRequiredService<TomTatBenhAnVM>();
            _headerViewModel = headerViewModel;
            WeakReferenceMessenger.Default.RegisterAll(this);
            _sideBaerNavigation = sideBaerNavigation;
        }

        // Cơ chế chuyển trang
        public void Receive(NavigationMessage message)
        {
            switch (message.PageName)
            {
                case "DashboardPage":
                    CurrentPage = serviceProvider.GetRequiredService<DashBoardVM>();
                    IsSideBarOpen = false;
                    break;
                case "TomTatBenhAnPage":
                    CurrentPage = serviceProvider.GetRequiredService<TomTatBenhAnVM>();
                    IsSideBarOpen = false;
                    break;
                case "KiemTraPhacDoPage":
                    CurrentPage = serviceProvider.GetRequiredService<KiemTraPhacDoVM>();
                    IsSideBarOpen = false;
                    break;
                case "PhacDoPage":
                    CurrentPage = serviceProvider.GetRequiredService<PhacDoVM>();
                    IsSideBarOpen = false;
                    break;
                case "BangKiemPage":
                    CurrentPage = serviceProvider.GetRequiredService<BangKiemVM>();
                    IsSideBarOpen = false;
                    break;
                default:
                    CurrentPage = serviceProvider.GetRequiredService<TomTatBenhAnVM>();
                    IsSideBarOpen = false;
                    break;
            }
        }

        public void Receive(SideBarStateMessage message)
        {
            IsSideBarOpen = message.isOpen;
        }

        partial void OnIsSideBarOpenChanged(bool oldValue, bool newValue)
        {
            WeakReferenceMessenger.Default.Send<SideBarStateMessage>(new SideBarStateMessage(newValue));
        }
    }
}
