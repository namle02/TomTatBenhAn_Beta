using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TomTatBenhAn_WPF.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? hello;

        public MainViewModel()
        {
            Hello = "xin chào";
        }

        [RelayCommand]
        private void ShowMessage()
        {
            MessageBox.Show(Hello + " b?n");
        }
    }
}
