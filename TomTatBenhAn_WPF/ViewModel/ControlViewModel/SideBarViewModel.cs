using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;

namespace TomTatBenhAn_WPF.ViewModel.ControlViewModel
{
     public partial class SideBarViewModel : ObservableObject
    {
        private readonly IDataMapper _dataMapper;
        public SideBarViewModel(IDataMapper dataMapper) 
        {
            _dataMapper = dataMapper;
        }

        [RelayCommand]
        private void GetData()
        {
            WeakReferenceMessenger.Default.Send(new LoadData("24.009732", null));
        }
    }
}
