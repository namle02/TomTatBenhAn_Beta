using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTatBenhAn_WPF.Repos.Model;
using TomTatBenhAn_WPF.ViewModel.ControlViewModel;

namespace TomTatBenhAn_WPF.Message
{
    public class ReportDataMessage : ValueChangedMessage<ThongTinBenhNhan>
    {
        public ReportDataMessage(ThongTinBenhNhan value) : base(value) { }
    }
}
