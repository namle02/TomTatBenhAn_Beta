using Microsoft.Web.WebView2.Wpf;
using System.Windows;

namespace TomTatBenhAn_WPF.Behaviors
{
    public static class WebViewHtmlBehavior
    {
        public static readonly DependencyProperty HtmlContentProperty =
            DependencyProperty.RegisterAttached(
                "HtmlContent",
                typeof(string),
                typeof(WebViewHtmlBehavior),
                new PropertyMetadata(null, OnHtmlContentChanged));

        public static string GetHtmlContent(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlContentProperty);
        }

        public static void SetHtmlContent(DependencyObject obj, string value)
        {
            obj.SetValue(HtmlContentProperty, value);
        }

        private static async void OnHtmlContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WebView2 webView && e.NewValue is string htmlContent && !string.IsNullOrEmpty(htmlContent))
            {
                try
                {
                    // Đảm bảo WebView đã được khởi tạo
                    await webView.EnsureCoreWebView2Async();
                    
                    // Navigate to HTML content
                    webView.NavigateToString(htmlContent);
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi load HTML vào WebView: {ex.Message}");
                }
            }
        }
    }
}
