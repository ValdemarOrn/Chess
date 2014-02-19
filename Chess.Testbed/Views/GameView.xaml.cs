using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	/// Interaction logic for GameView.xaml
	/// </summary>
	public partial class GameView : UserControl
	{
		public GameView()
		{
			InitializeComponent();
			DataContext = new GameViewModel();
			((INotifyCollectionChanged)ListBoxWhitePlayerLog.Items).CollectionChanged += GameView_CollectionChanged;
			((INotifyCollectionChanged)ListBoxBlackPlayerLog.Items).CollectionChanged += GameView_CollectionChanged;
		}

		void GameView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ListBox s = null;
			if (sender == ListBoxWhitePlayerLog.Items)
				s = ListBoxWhitePlayerLog;
			if (sender == ListBoxBlackPlayerLog.Items)
				s = ListBoxBlackPlayerLog;

			if (s != null && s.Items.Count > 0)
				s.ScrollIntoView(s.Items[s.Items.Count - 1]);
		}
	}
}
