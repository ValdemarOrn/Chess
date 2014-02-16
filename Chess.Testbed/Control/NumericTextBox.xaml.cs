using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.Testbed.Control
{
	/// <summary>
	/// Interaction logic for NumericTextBox.xaml
	/// </summary>
	public partial class NumericTextBox : TextBox
	{
		public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(int?), typeof(NumericTextBox), 
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(int?), typeof(NumericTextBox),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int?), typeof(NumericTextBox),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public int? Min
		{
			get { return (int?)GetValue(MinProperty); }
			set { SetValue(MinProperty, value); }
		}

		public int? Max
		{
			get { return (int?)GetValue(MaxProperty); }
			set { SetValue(MaxProperty, value); }
		}

		public int? Value
		{
			get { return (int?)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public NumericTextBox()
		{
			InitializeComponent();

			var prop = DependencyPropertyDescriptor.FromProperty(MinProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) => SetValue());

			prop = DependencyPropertyDescriptor.FromProperty(MaxProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) => SetValue());

			prop = DependencyPropertyDescriptor.FromProperty(ValueProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) => SetValue());

			this.LostFocus += NumericTextBox_LostFocus;
		}

		void NumericTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			int value;
			var ok = int.TryParse(Text, out value);
			Value = ok ? value : (int?)null;
		}

		private void SetValue()
		{
			if (Min.HasValue && Value < Min)
			{
				Value = Min.Value;
				return;
			}
			else if (Max.HasValue && Value > Max)
			{
				Value = Max.Value;
				return;
			}

			Text = Value.ToString();
		}

		bool ignoreScroll;
		private void ScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
		{
			if (ignoreScroll)
				return;

			var val = e.NewValue;
			ignoreScroll = true;
			if (val == 0)
				Value++;
			else if (val == 2)
				Value--;
			(sender as ScrollBar).Value = 1;
			ignoreScroll = false;
		}
	}
}
