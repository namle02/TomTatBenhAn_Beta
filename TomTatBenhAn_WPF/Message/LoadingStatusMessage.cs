using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TomTatBenhAn_WPF.Message
{
    /// <summary>
    /// Message để thông báo trạng thái loading giữa các ViewModel
    /// </summary>
    public class LoadingStatusMessage : ValueChangedMessage<bool>
    {
        public LoadingStatusMessage(bool isLoading) : base(isLoading)
        {
        }
    }
}
