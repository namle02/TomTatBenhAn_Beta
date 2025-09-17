using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TomTatBenhAn_WPF.Message
{
    public class SendPhacDoMessage
    {
        public string Name { get; set; } = string.Empty;
        public StringBuilder PhacDo { get; set; } = new StringBuilder();

        public SendPhacDoMessage(string Name, StringBuilder PhacDo) 
        {
            this.Name = Name;
            this.PhacDo = PhacDo;
        }
    }
}
