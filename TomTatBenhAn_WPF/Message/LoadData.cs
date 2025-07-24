using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Message
{
    public class LoadData :ObservableObject
    {
        public string? soBenhAn { get; set; }  
        public string? maYTe { get; }
        public LoadData(string SoBenhAn, string MaYTe)
        {
            this.soBenhAn = SoBenhAn;
            this.maYTe = MaYTe;
        }
        public override string ToString()
        {
            return soBenhAn; 
        }

    }
}
