using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Chess.Testbed.Control;

namespace Chess.Testbed.Views
{
	public class MainWindowViewModel : ViewModelBase
	{
		public ModelCommand SaveSettingsCommand { get; private set; }

		public MainWindowViewModel()
		{
			SaveSettingsCommand = new ModelCommand(SaveSettings);
			LoadSettings();
		}

		private void LoadSettings()
		{
			try
			{
				var filename = Path.Combine(MasterState.ExeDir, "Chess.Testbed.exe.settings");
				if (!File.Exists(filename))
					return;

				var data = File.ReadAllText(filename);
				MasterState.Instance.DeserializeState(data);
			}
			catch (Exception e)
			{
				Log.ErrorDialog("Unable to load configuration settings");
			}
		}

		private void SaveSettings()
		{
			var data = MasterState.Instance.SerializeState();
			var filename = Path.Combine(MasterState.ExeDir, "Chess.Testbed.exe.settings");
			File.WriteAllText(filename, data);
		}
	}
}
