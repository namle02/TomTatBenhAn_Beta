using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Repos._Model.PatientData
{
    public partial class DataTomTat : ObservableObject
    {
        [ObservableProperty] private string tomTatQuaTrinhBenhLy = string.Empty;
        [ObservableProperty] private string tomTatDauHieuLamSang = string.Empty;
        [ObservableProperty] private string tomTatKetQuaXN = string.Empty;
        [ObservableProperty] private string tomTatTinhTrangNguoiBenhRaVien = string.Empty;
        [ObservableProperty] private string tomTatHuongDieuTriTiepTheo = string.Empty;
    }
}
