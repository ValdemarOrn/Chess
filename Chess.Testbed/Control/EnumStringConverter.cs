using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Chess.Testbed.Control
{
	public class EnumStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var str = value.ToString();
			var output = new List<char>();
			foreach (var ch in str)
			{
				if (Char.IsUpper(ch))
					output.Add(' ');

				output.Add(ch);
			}

			return new string(output.ToArray()).Trim();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
