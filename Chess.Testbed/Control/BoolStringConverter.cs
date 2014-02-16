using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Chess.Testbed.Control
{
	public class BoolStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null && targetType == typeof(Nullable<bool>))
				return null;
			if (value == null && targetType == typeof(string))
				return null;
			if (value == null && targetType == typeof(bool))
				return false;

			if (value is bool || value is Nullable<bool>)
				return (bool)value ? "True" : "False";
			if (value is string)
				return new[] { "true", "1", "on", "enabled" }.Contains(((string)value).ToLower());
			else
				throw new Exception("Invalid type");
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Convert(value, targetType, parameter, culture);
		}
	}
}
