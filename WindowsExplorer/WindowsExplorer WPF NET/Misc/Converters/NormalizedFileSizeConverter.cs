using System;
using System.Globalization;

namespace WindowsExplorer_WPF_NET.Misc.Converters
{
    public class NormalizedFileSizeConverter: System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetNormalized((long)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string GetNormalized(long Size)
        {
            if (Size < 1024)
            {
                return $"{Size} bytes";
            }
            if (Size < 1024 * 1024)
            {
                return $"{Math.Round((double)(Size / 1024), 2)} kb";
            }

            if (Size < 1024 * 1024 * 1024)
            {
                return $"{Math.Round((double)(Size / (1024 * 1024)), 2)} mb";
            }
            else
            {
                return $"{Math.Round((double)(Size / (1024 * 1024 * 1024)), 2)} gb";
            }
        }
    }
}
