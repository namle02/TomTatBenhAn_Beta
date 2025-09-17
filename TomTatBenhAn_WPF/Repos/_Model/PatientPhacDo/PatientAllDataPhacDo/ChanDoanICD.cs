using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Repos._Model.PatientPhacDo.PatientAllDataPhacDo
{
    public partial class ChanDoanICD : ObservableObject
    {
        [ObservableProperty] private string? benhChinhVaoVien;
        [ObservableProperty] private string? maICDChinhVaoVien;
        [ObservableProperty] private string? benhPhuVaoVien;
        [ObservableProperty] private string? maICDPhuVaoVien;
        [ObservableProperty] private string? benhChinhRaVien;
        [ObservableProperty] private string? maICDChinhRaVien;
        [ObservableProperty] private string? benhKemTheoRaVien;
        [ObservableProperty] private string? maICDKemTheoRaVien;
    }
}
