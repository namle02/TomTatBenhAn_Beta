using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Message
{
    public class LoadDataMessage : ValueChangedMessage<LoadData>
    {
        public LoadDataMessage(LoadData value) : base(value) { }
    }
}
