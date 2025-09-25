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
        [ObservableProperty] private bool pPDT_NoiKhoa = false;
        [ObservableProperty] private bool notPPDT_NoiKhoa = true;
        [ObservableProperty] private bool pPDT_PTTT = false;
        [ObservableProperty] private bool notPPDT_PTTT = true;
        [ObservableProperty] private string huongDieuTri_PTTT = string.Empty;


        partial void OnHuongDieuTriChanged(string? oldValue, string newValue)
        {
            if(newValue != string.Empty)
            {
                PPDT_NoiKhoa = true;
                NotPPDT_NoiKhoa = !PPDT_NoiKhoa;
            }
        }

        partial void OnHuongDieuTri_PTTTChanged(string? oldValue, string newValue)
        {
            if (newValue != string.Empty)
            {
                PPDT_PTTT = true;
                NotPPDT_PTTT = !PPDT_PTTT;
            }
        }

        partial void OnNotPPDT_NoiKhoaChanged(bool oldValue, bool newValue)
        {
            if(newValue == true)
            {
                PPDT_NoiKhoa = false;
                HuongDieuTri = string.Empty;
            }
        }

        partial void OnNotPPDT_PTTTChanged(bool oldValue, bool newValue)
        {
            if (newValue == true)
            {
                PPDT_PTTT = false;
                HuongDieuTri_PTTT = string.Empty;
            }
        }
    }
}
