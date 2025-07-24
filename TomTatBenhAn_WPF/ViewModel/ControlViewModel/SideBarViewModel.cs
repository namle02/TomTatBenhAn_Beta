using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;
using TomTatBenhAn_WPF.Repos.Model;

namespace TomTatBenhAn_WPF.ViewModel.ControlViewModel
{
    public partial class SideBarViewModel : ObservableObject
    {
        private readonly IDataMapper _dataMapper;

        public SideBarViewModel(IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;

            // Nhận danh sách số bệnh án từ ContentViewModel
            WeakReferenceMessenger.Default.Register<SoBenhAnListMessage>(this, (r, m) =>
            {
                SoBenhAnList.Clear();
                foreach (var item in m.Value)
                {
                    SoBenhAnList.Add(item);
                }
            });

            // Nhận lại mục đã chọn từ ContentViewModel để hiển thị
            WeakReferenceMessenger.Default.Register<LoadDataMessage>(this, (r, m) =>
            {
                SelectedSoBenhAn = SoBenhAnList.FirstOrDefault(x =>
                    x.soBenhAn?.Trim().Equals(m.Value.soBenhAn?.Trim(), StringComparison.OrdinalIgnoreCase) == true
                );
            });
        }

        // Đổ vào ComboBox
        public ObservableCollection<LoadData> SoBenhAnList { get; set; } = new();

        [ObservableProperty]
        private LoadData? selectedSoBenhAn;

        [ObservableProperty]
        private string soBenhAn;

        [ObservableProperty]
        private string maYTe;

       
        [ObservableProperty]
        private bool isSoBenhAnChecked;

    
        [ObservableProperty]
        private bool isMaYTeChecked;

        // Khi người dùng chọn bệnh án trong ComboBox
        partial void OnSelectedSoBenhAnChanged(LoadData? value)
        {
            if (value != null)
            {
                WeakReferenceMessenger.Default.Send(new LoadData(value.soBenhAn, value.maYTe));
            }
        }

        partial void OnIsSoBenhAnCheckedChanged(bool value)
        {
            if (value)
            {
               
                IsMaYTeChecked = false;  
            }
            else
            {
                
                SoBenhAn = string.Empty;
            }
        }

        partial void OnIsMaYTeCheckedChanged(bool value)
        {
            if (value)
            {
                
                IsSoBenhAnChecked = false; 
            }
            else
            {
                
                MaYTe = string.Empty;
            }
        }


        // Nhấn nút "Lấy dữ liệu"
        [RelayCommand]
        private void GetData()
        {
            if (!string.IsNullOrWhiteSpace(SoBenhAn))
            {
                SoBenhAnList.Clear();
                SelectedSoBenhAn = null;

                WeakReferenceMessenger.Default.Send(new LoadData(SoBenhAn, MaYTe));
                return;
            }

            if (!string.IsNullOrWhiteSpace(MaYTe))
            {
                WeakReferenceMessenger.Default.Send(new LoadData(null, MaYTe));
            }
        }
    }
}
