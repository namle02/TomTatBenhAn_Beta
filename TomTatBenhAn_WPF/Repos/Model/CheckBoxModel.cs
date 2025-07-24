using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Repos.Model
{
    public partial class CheckBoxModel : ObservableObject
    {
        [ObservableProperty]
        private bool isCheckedKhoi;
        [ObservableProperty]
        private bool isCheckedDo;
        [ObservableProperty]
        private bool isCheckedKhongThayDoi;
        [ObservableProperty]
        private bool isCheckedNangHon;
        [ObservableProperty]
        private bool isCheckedTuVong;
        [ObservableProperty]
        private bool isCheckedTienLuongNangXinVe;
        [ObservableProperty]
        private bool isCheckedChuaXacDinh;
        [ObservableProperty]
        public bool checkBox1;
        [ObservableProperty]
        public bool checkBox2;
    }
}
