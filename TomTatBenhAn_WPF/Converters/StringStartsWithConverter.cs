using System;
using System.Globalization;
using System.Windows.Data;

namespace TomTatBenhAn_WPF.Converters
{
    public class StringStartsWithConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            var prefix = parameter as string;
            
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(prefix))
                return false;
                
            return str.StartsWith(prefix);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
