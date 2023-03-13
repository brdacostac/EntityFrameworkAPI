using System;
using System.Globalization;

namespace LolApp.Resources.Converters
{
    public class ImageRatioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double parentWidth = (double)value;
                double ratio = (double)parameter;
                return parentWidth*ratio;
            }
            catch
            {
                return 0.0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

