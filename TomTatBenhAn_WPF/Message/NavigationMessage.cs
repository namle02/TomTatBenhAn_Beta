

namespace TomTatBenhAn_WPF.Message
{
    
    public class NavigationMessage
    {
        public string PageName = string.Empty;
        public NavigationMessage(string _pageName)
        {
            PageName = _pageName;
        }
        
    }
}
