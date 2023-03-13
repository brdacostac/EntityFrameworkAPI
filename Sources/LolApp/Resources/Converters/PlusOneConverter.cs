using System;
using System.Globalization;

namespace LolApp.Resources.Converters
{
    public class PlusOneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int i = -1;
            try
            {
                i = (int)value;
            }
            catch (InvalidCastException e)
            {
                throw new InvalidCastException("PlusOneConverter : the value must be an int");
            }
            return i + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

