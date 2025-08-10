using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;
using TomTatBenhAn_WPF.View.ControlView;


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

                // Hiển thị item đầu tiên nhưng không chọn
                if (SoBenhAnList.Count > 0)
                {
                    PreviewSoBenhAn = SoBenhAnList[0].soBenhAn;
                    SelectedSoBenhAn = null;
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
        [ObservableProperty]
        private string? previewSoBenhAn;
        // Đổ vào ComboBox
        public ObservableCollection<LoadData> SoBenhAnList { get; set; } = new();

        [ObservableProperty]
        private LoadData? selectedSoBenhAn;

        [ObservableProperty]
        private string soBenhAn = string.Empty;

        [ObservableProperty]
        private string maYTe = string.Empty;


        [ObservableProperty]
        private bool isSoBenhAnChecked;


        [ObservableProperty]
        private bool isMaYTeChecked;

        // Khi người dùng chọn bệnh án trong ComboBox
        partial void OnSelectedSoBenhAnChanged(LoadData? value)
        {
            if (value != null)
            {
                WeakReferenceMessenger.Default.Send(new LoadData(value.soBenhAn!, value.maYTe!));
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

        private readonly Regex _soBenhAnRegex = new Regex(@"^\d{2}\.\d{6}$");

        
        private readonly Regex _maYTeRegex = new Regex(@"^\d{8}$");
      
        [RelayCommand]
        private void GetData()
        {
            // Validate SoBenhAn if user entered something
            if (!string.IsNullOrWhiteSpace(SoBenhAn))
            {
                if (!_soBenhAnRegex.IsMatch(SoBenhAn))
                {
                    MessageBox.Show("Số bệnh án không đúng định dạng",
                                    "Lỗi định dạng",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                SoBenhAnList.Clear();
                SelectedSoBenhAn = null;

                WeakReferenceMessenger.Default.Send(new LoadData(SoBenhAn, MaYTe));
                return;
            }

            // Validate MaYTe if user entered something
            if (!string.IsNullOrWhiteSpace(MaYTe))
            {
                if (!_maYTeRegex.IsMatch(MaYTe))
                {
                    MessageBox.Show("Mã y tế không đúng định dạng ",
                                    "Lỗi định dạng",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                WeakReferenceMessenger.Default.Send(new LoadData(null, MaYTe));
            }
        }
       
    }
}
