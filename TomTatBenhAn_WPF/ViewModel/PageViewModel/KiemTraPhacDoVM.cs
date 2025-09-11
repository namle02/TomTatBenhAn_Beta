using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;

namespace TomTatBenhAn_WPF.ViewModel.PageViewModel
{
    public partial class KiemTraPhacDoVM : ObservableObject
    {
        private readonly IDataMapper _dataMapper;
        [ObservableProperty] private PatientAllData patientData = new PatientAllData();
        [ObservableProperty] private bool isLoading = false;

        public KiemTraPhacDoVM(IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;
        }

        [RelayCommand]
        private async Task KiemTraBenhNhan(string SoBenhAn)
        {
            PatientData = await _dataMapper.GetAllPatientData(SoBenhAn);
        }

        
    }
}
