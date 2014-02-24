using Chess.Testbed.Control;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;

namespace Chess.Testbed.Views
{
	public class EnginesViewModel : ViewModelBase
	{
		public EnginesViewModel()
		{
			CreateNewEngineCommand = new ModelCommand(CreateNewEngine);
			DeleteEngineCommand = new ModelCommand(DeleteEngine);
			BrowseEngineCommand = new ModelCommand(BrowseEngine, () => SelectedEngine != null);
			ReloadEngineCommand = new ModelCommand(ReloadEngine, () => SelectedEngine != null && EngineCommand != null && File.Exists(EngineCommand));
		}

		public ModelCommand CreateNewEngineCommand { get; private set; }
		public ModelCommand DeleteEngineCommand { get; private set; }
		public ModelCommand BrowseEngineCommand { get; private set; }
		public ModelCommand ReloadEngineCommand { get; private set; }

		public ObservableCollection<UciEngineSettings> Engines
		{
			get { return new ObservableCollection<UciEngineSettings>(MasterState.Instance.Engines); }
		}

		public string EngineCommand
		{
			get { return SelectedEngine != null ? SelectedEngine.Command : null; }
			set
			{
				if (SelectedEngine != null)
				{
					SelectedEngine.Command = value;
					ReloadEngineCommand.RefreshCanExecuteChanged();
					NotifyChanged();
				}
			}
		}
	
		private UciEngineSettings selectedEngine;
		public UciEngineSettings SelectedEngine
		{
			get { return selectedEngine; }
			set 
			{ 
				selectedEngine = value;
				NotifyChanged();
				NotifyChanged(() => EngineCommand);
				NotifyChanged(() => SelectedEngineOptions);
				BrowseEngineCommand.RefreshCanExecuteChanged();
			}
		}

		public ObservableCollection<EngineSettingViewModel> SelectedEngineOptions
		{
			get
			{
				return SelectedEngine != null
					? new ObservableCollection<EngineSettingViewModel>(SelectedEngine.Options.Select(x => new EngineSettingViewModel(x)))
					: new ObservableCollection<EngineSettingViewModel>();
			}
		}

		private void CreateNewEngine()
		{
			SelectedEngine = MasterState.Instance.CreateNewEngine();
			NotifyChanged(() => Engines);
		}

		private void DeleteEngine()
		{
			MasterState.Instance.RemoveEngine(SelectedEngine);
			NotifyChanged(() => Engines);
		}

		private void BrowseEngine()
		{
			var dialog = new OpenFileDialog 
			{ 
				Multiselect = false,
				CheckFileExists = true,
				Title = "Select Engine"
			};
			var res = dialog.ShowDialog();
			if (!res.GetValueOrDefault())
				return;

			EngineCommand = dialog.FileName;
		}

		private void ReloadEngine()
		{
			try
			{
				SelectedEngine.LoadEngineData();
				NotifyChanged(() => SelectedEngine);
				NotifyChanged(() => SelectedEngineOptions);
			}
			catch (Exception e)
			{
				Log.Exception(e);
				Log.ErrorDialog("Unable to load engine data.\n" + e.Message);
			}
		}
	
	}
}
