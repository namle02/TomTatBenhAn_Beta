using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Repos._Model.PatientData
{
    public partial class ThongTinKhamBenhModel : ObservableObject
    {
        [ObservableProperty] private string lyDoVaoVien = string.Empty;
        [ObservableProperty] private string quaTrinhBenhLy = string.Empty;
        [ObservableProperty] private string tienSuBenh = string.Empty;
        [ObservableProperty] private string huongDieuTri = string.Empty;
    }
}
