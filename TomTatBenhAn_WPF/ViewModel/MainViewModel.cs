using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.ViewModel.ControlViewModel;
using TomTatBenhAn_WPF.ViewModel.PageViewModel;
using TomTatBenhAn_WPF.View.ControlView;

namespace TomTatBenhAn_WPF.ViewModel
{
    public partial class MainViewModel : ObservableObject, IRecipient<NavigationMessage>
    {
        private readonly IServiceProvider serviceProvider;
       

        [ObservableProperty] private object currentPage;

        public MainViewModel(IServiceProvider serviceProvider, SideBarViewModel sideBarViewModel, ContentViewModel contentViewModel)
        {
            this.serviceProvider = serviceProvider;
            CurrentPage = serviceProvider.GetRequiredService<TomTatBenhAnVM>();
            WeakReferenceMessenger.Default.RegisterAll(this);
            

        }

        // Cơ chế chuyển trang
        public void Receive(NavigationMessage message)
        {
            switch (message.PageName)
            {
                case "PhacDoPage":
                    CurrentPage = serviceProvider.GetRequiredService<PhacDoVM>();
                    break;
                case "TomTatBenhAnPage":
                    CurrentPage = serviceProvider.GetRequiredService<TomTatBenhAnVM>();
                    break;
                default:
                    CurrentPage = serviceProvider.GetRequiredService<TomTatBenhAnVM>();
                    break;
            }
        }

        

    }
}
