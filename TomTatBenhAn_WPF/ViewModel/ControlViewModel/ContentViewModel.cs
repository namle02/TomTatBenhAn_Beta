using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;
using TomTatBenhAn_WPF.Repos.Model;

namespace TomTatBenhAn_WPF.ViewModel.ControlViewModel
{
    public partial class ContentViewModel : ObservableObject, IRecipient<LoadData>
    {
        private readonly IDataMapper _dataMapper;

        [ObservableProperty]
        private HanhChinhModel patientInfo = new();

        public ContentViewModel(IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;

            WeakReferenceMessenger.Default.Register<LoadData>(this);
        }

        public async void Receive(LoadData message)
        {
            patientInfo = await _dataMapper.GetHanhChinhData(message.soBenhAn);
        }


    }
}
