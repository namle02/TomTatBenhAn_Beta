using ControlzEx.Standard;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TomTatBenhAn_WPF.Repos.Model;
using TomTatBenhAn_WPF.ViewModel;
using static MaterialDesignThemes.Wpf.Theme;

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

    private void Sidebar_Loaded(object sender, RoutedEventArgs e)
    {

    }
    
}
