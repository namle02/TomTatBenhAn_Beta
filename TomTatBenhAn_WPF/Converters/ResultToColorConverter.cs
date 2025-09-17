using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TomTatBenhAn_WPF.Converters
{
    public class ResultToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Gray);

            string result = value.ToString().ToLower();

            return result switch
            {
                "đạt" or "đạt yêu cầu" or "pass" or "passed" => new SolidColorBrush(Color.FromRgb(76, 175, 80)), // Green #4CAF50
                "không đạt" or "fail" or "failed" => new SolidColorBrush(Color.FromRgb(244, 67, 54)), // Red #F44336
                "cảnh báo" or "warning" or "warn" => new SolidColorBrush(Color.FromRgb(255, 152, 0)), // Orange #FF9800
                "chưa xác định" or "unknown" or "pending" => new SolidColorBrush(Color.FromRgb(158, 158, 158)), // Gray #9E9E9E
                _ => new SolidColorBrush(Color.FromRgb(33, 150, 243)) // Blue #2196F3 (default)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
