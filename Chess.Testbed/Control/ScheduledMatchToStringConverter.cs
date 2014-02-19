using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Chess.Testbed.Control
{
	public class ScheduledMatchToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!(value is ScheduledMatch))
				return null;

			var match = value as ScheduledMatch;
			var white = MasterState.Instance.Engines.SingleOrDefault(x => x.Id == match.WhiteId);
			var black = MasterState.Instance.Engines.SingleOrDefault(x => x.Id == match.BlackId);
			var tc = MasterState.Instance.TimeSettings.SingleOrDefault(x => x.Id == match.TimeControlId);

			var whiteName = white != null ? white.Name : "<Unknown>";
			var blackName = black != null ? black.Name : "<Unknown>";
			var tcName = tc != null ? tc.Name : "<Unknown>";
			return whiteName + " - " + blackName + " (" + tcName + ")";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
