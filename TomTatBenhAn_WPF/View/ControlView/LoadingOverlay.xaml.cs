using System.Windows;
using System.Windows.Controls;

namespace TomTatBenhAn_WPF.View.ControlView
{
    /// <summary>
    /// Interaction logic for LoadingOverlay.xaml
    /// </summary>
    public partial class LoadingOverlay : UserControl
    {
        public static readonly DependencyProperty LoadingTextProperty =
            DependencyProperty.Register("LoadingText", typeof(string), typeof(LoadingOverlay), 
                new PropertyMetadata("Đang tóm tắt bệnh án..."));

        public string LoadingText
        {
            get { return (string)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }

        public LoadingOverlay()
        {
            InitializeComponent();
        }
    }
}
