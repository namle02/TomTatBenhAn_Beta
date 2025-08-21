using System;
using System.Globalization;
using System.Windows.Data;

namespace TomTatBenhAn_WPF.Converters
{
    /// <summary>
    /// Converter để chuyển đổi giữa giá trị string KetQuaDieuTri và boolean cho checkbox
    /// Đảm bảo chỉ có 1 checkbox được chọn tại một thời điểm
    /// </summary>
    public class KetQuaDieuTriConverter : IValueConverter
    {
        /// <summary>
        /// Chuyển đổi từ string KetQuaDieuTri sang bool cho checkbox
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string ketQuaDieuTri = value.ToString() ?? "";
            string expectedValue = parameter.ToString() ?? "";

            // So sánh không phân biệt hoa thường
            return string.Equals(ketQuaDieuTri, expectedValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Chuyển đổi từ bool checkbox sang string KetQuaDieuTri
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && parameter != null)
            {
                if (isChecked)
                {
                    // Nếu checkbox được chọn, trả về giá trị tương ứng
                    return parameter.ToString();
                }
                else
                {
                    // Nếu checkbox bị bỏ chọn, để trống (sẽ không có checkbox nào được chọn)
                    return string.Empty;
                }
            }
            
            return Binding.DoNothing;
        }
    }
}
