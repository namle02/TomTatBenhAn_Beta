using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TomTatBenhAn_WPF.Message
{
    public class LoadDataMessage : ValueChangedMessage<LoadData>
    {
        public LoadDataMessage(LoadData value) : base(value) { }
    }
}
