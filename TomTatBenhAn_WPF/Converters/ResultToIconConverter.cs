using System;
using System.Globalization;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace TomTatBenhAn_WPF.Converters
{
    public class ResultToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return PackIconKind.HelpCircle;

            string result = value.ToString().ToLower();

            return result switch
            {
                "đạt" or "đạt yêu cầu" or "pass" or "passed" => PackIconKind.CheckCircle,
                "không đạt" or "fail" or "failed" => PackIconKind.CloseCircle,
                "cảnh báo" or "warning" or "warn" => PackIconKind.AlertCircle,
                "chưa xác định" or "unknown" or "pending" => PackIconKind.HelpCircle,
                _ => PackIconKind.InformationCircle
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
