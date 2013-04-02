using System;
using System.Windows;
using System.Windows.Data;

namespace OOTP.Lab4.Converters
{
	/// <summary>
	/// Convert null variable to visibility poroprties
	/// </summary>
	class NullToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value == null ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
