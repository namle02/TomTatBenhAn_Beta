using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Message
{
    public class SideBarStateMessage
    {
        public bool isOpen { get; set; }

        public SideBarStateMessage(bool isOpen)
        {
            this.isOpen = isOpen;
        }
    }
}
