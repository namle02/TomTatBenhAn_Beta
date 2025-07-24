using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Message
{
    public class SoBenhAnListMessage : ValueChangedMessage<List<LoadData>>
    {
        public SoBenhAnListMessage(List<LoadData> list) : base(list) { }
    }
}