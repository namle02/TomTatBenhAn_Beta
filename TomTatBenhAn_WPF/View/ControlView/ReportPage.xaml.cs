using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TomTatBenhAn_WPF.Repos.Model;
using TomTatBenhAn_WPF.ViewModel;
using TomTatBenhAn_WPF.ViewModel.ControlViewModel;


namespace TomTatBenhAn_WPF.View.ControlView
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Window
    {

        public ReportPage(ThongTinBenhNhan patient, 
            string aiTomTatQuaTrinhBenhLy,
            string aiDauHieuLamSang,
            HanhChinhModel hanhchinh, 
            BenhAnChiTietModel benhAnChiTiet,
             
            ChuanDoanModel chuandoan)
        {
            InitializeComponent();
            DataContext = new ReportPageModel(ReportView, patient, aiTomTatQuaTrinhBenhLy,hanhchinh,chuandoan,benhAnChiTiet, aiDauHieuLamSang);
        }


    }
}
