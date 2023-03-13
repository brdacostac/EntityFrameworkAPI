using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace LolApp.Resources.Converters
{
	public class Base64ToImageSourceConverter : ByteArrayToImageSourceConverter, IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			string base64 = value as string;
            if (string.IsNullOrWhiteSpace(base64)) return null;
			try
			{
				byte[] bytes = System.Convert.FromBase64String(base64);
				return base.ConvertFrom(bytes, culture);
			}
			catch
			{
				return null;
			}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
			ImageSource source = value as ImageSource;
			if (source == null) return null;
            byte[] bytes = base.ConvertBackTo(source, culture) as byte[];
			return System.Convert.ToBase64String(bytes);
        }
	}
}

