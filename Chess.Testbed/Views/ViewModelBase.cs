using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Chess.Testbed.Views
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		#region Notify Change

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyChanged<T>(System.Linq.Expressions.Expression<Func<T>> exp)
		{
			var name = GetPropertyName(exp);
			NotifyChanged(name);
		}

		protected void NotifyChanged([CallerMemberName]string property = null)
		{
			if (PropertyChanged != null)
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
		}

		protected static string GetPropertyName<T>(System.Linq.Expressions.Expression<Func<T>> exp)
		{
			return (((System.Linq.Expressions.MemberExpression)(exp.Body)).Member).Name;
		}

		#endregion
	}
}
