using CommunityToolkit.Mvvm.Messaging.Messages;


namespace TomTatBenhAn_WPF.Message
{
    public class SoBenhAnListMessage : ValueChangedMessage<List<LoadData>>
    {
        public SoBenhAnListMessage(List<LoadData> list) : base(list) { }
    }
}