using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Repos.Model
{
    public partial class BenhAnTypeModel : ObservableObject
    {

        public string? LoaiBenhAn { get; set; }

        public string? BenhAnTongQuatId { get; set; }

        public string? TiepNhanId {  get; set; }
       

    }
}
