using System.Windows;
using TomTatBenhAn_WPF.ViewModel;

namespace TomTatBenhAn_WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel vm)
    {
        
        InitializeComponent();
        DataContext = vm;

    }


    
}
