using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.Testbed.Views
{
	/// <summary>
	/// Interaction logic for PlayView.xaml
	/// </summary>
	public partial class SchedulerView : UserControl
	{
		public SchedulerView()
		{
			InitializeComponent();
			DataContext = new SchedulerViewModel();
		}

		private void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			((SchedulerViewModel)DataContext).Opponents = ((ListBox)sender).SelectedItems.Cast<UciEngineSettings>().ToArray();
		}

		private void UserControlIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
				((SchedulerViewModel)DataContext).ReloadScheduledMatchesCommand.Execute(null);
		}
	}
}
