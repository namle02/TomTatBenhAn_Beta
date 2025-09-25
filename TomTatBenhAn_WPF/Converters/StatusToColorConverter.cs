using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TomTatBenhAn_WPF.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Gray);

            string status = value.ToString().ToLower();

            return status switch
            {
                "hoạt động" or "active" or "kích hoạt" => new SolidColorBrush(Color.FromRgb(76, 175, 80)), // Green #4CAF50
                "tạm dừng" or "inactive" or "pause" or "paused" => new SolidColorBrush(Color.FromRgb(255, 152, 0)), // Orange #FF9800
                "ngừng hoạt động" or "disabled" or "stopped" => new SolidColorBrush(Color.FromRgb(244, 67, 54)), // Red #F44336
                "đang xử lý" or "processing" or "pending" => new SolidColorBrush(Color.FromRgb(33, 150, 243)), // Blue #2196F3
                "chờ duyệt" or "waiting" or "approval" => new SolidColorBrush(Color.FromRgb(156, 39, 176)), // Purple #9C27B0
                _ => new SolidColorBrush(Color.FromRgb(158, 158, 158)) // Gray #9E9E9E (default)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
