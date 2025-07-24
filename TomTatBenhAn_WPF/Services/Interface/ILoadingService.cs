using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface ILoadingService : INotifyPropertyChanged
    {
        Visibility IsLoading { get; }
        void Show();
        void Hide();
    }
}
