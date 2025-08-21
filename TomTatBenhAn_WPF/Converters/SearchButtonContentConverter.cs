using System;
using System.Globalization;
using System.Windows.Data;

namespace TomTatBenhAn_WPF.Converters
{
    public class SearchButtonContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSearching)
            {
                return isSearching ? "Đang tìm..." : "Tìm kiếm";
            }
            return "Tìm kiếm";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
