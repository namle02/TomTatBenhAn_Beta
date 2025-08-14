

namespace TomTatBenhAn_WPF.Message
{
    
    public class NavigationMessage
    {
        public string PageName = string.Empty;
        public string CalledBy = string.Empty;

        public NavigationMessage(string _pageName, string _calledBy)
        {
            PageName = _pageName;
            CalledBy = _calledBy;
        }
        
    }
}
