using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.Services.Implement
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    public class LoadingService : ILoadingService
    {
        private Visibility _isLoading = Visibility.Collapsed;
        public Visibility IsLoading
        {
            get => _isLoading;
            private set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public void Show() => IsLoading = Visibility.Visible;

        public void Hide() => IsLoading = Visibility.Collapsed;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
