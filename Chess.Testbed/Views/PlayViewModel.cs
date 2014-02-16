using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Testbed.Views
{
	public class PlayViewModel : ViewModelBase
	{
		public PlayViewModel()
		{
			MasterState.Instance.RegisterAction(MasterState.EventEnginesChanged, () => NotifyChanged(() => Engines));
		}

		public ObservableCollection<UciEngineSettings> Engines
		{
			get { return new ObservableCollection<UciEngineSettings>(MasterState.Instance.Engines); }
		}

		private UciEngineSettings selectedEngine;
		public UciEngineSettings SelectedEngine
		{
			get { return selectedEngine; }
			set { selectedEngine = value; NotifyChanged(); }
		}

		private UciEngineSettings[] opponents;
		public UciEngineSettings[] Opponents
		{
			get { return opponents; }
			set { opponents = value; NotifyChanged(); }
		}
	}
}
