using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Testbed.Control;

namespace Chess.Testbed.Views
{
	public class TimeViewModel : ViewModelBase
	{
		public TimeViewModel()
		{
			CreateNewTimeSettingCommand = new ModelCommand(CreateNewSetting);
		}

		public ModelCommand CreateNewTimeSettingCommand { get; private set; }

		public ObservableCollection<TimeSettings> TimeSettings
		{
			get { return new ObservableCollection<TimeSettings>(MasterState.Instance.TimeSettings); }
		}

		private TimeSettings selectedSetting;
		public TimeSettings SelectedSetting
		{
			get { return selectedSetting; }
			set 
			{
				selectedSetting = value;
				NotifyChanged();
				NotifyChanged(() => TimePerMoveMins);
				NotifyChanged(() => TimePerMoveSecs);
				NotifyChanged(() => InitialTimeMins);
				NotifyChanged(() => InitialTimeSecs);
				NotifyChanged(() => IncrementTimeMins);
				NotifyChanged(() => IncrementTimeSecs);
				NotifyChanged(() => TimeControlMins);
				NotifyChanged(() => TimeControlSecs);
			}
		}

		public ObservableCollection<TimeMode> TimeModes
		{
			get { return new ObservableCollection<TimeMode>(Enum.GetValues(typeof (TimeMode)).Cast<TimeMode>()); }
		}

		// =================== Time per Move ===================

		public int? TimePerMoveMins
		{
			get
			{
				if (SelectedSetting != null && SelectedSetting.TimePerMove != null)
					return (int)Math.Floor(SelectedSetting.TimePerMove.Value / 60.0);
				return null;
			}
			set
			{
				if (SelectedSetting != null)
				{
					SelectedSetting.TimePerMove = value.GetValueOrDefault() * 60 + TimePerMoveSecs.GetValueOrDefault();
					NotifyChanged();
					NotifyChanged(() => TimePerMoveSecs);
				}
			}
		}

		public int? TimePerMoveSecs
		{
			get
			{
				if (SelectedSetting != null && SelectedSetting.TimePerMove != null)
					return (int)Math.Floor(SelectedSetting.TimePerMove.Value % 60.0);
				return null;
			}
			set
			{
				if (SelectedSetting != null)
				{
					SelectedSetting.TimePerMove = TimePerMoveMins.GetValueOrDefault() * 60 + value.GetValueOrDefault();
					NotifyChanged();
					NotifyChanged(() => TimePerMoveMins);
				}
			}
		}

		// =================== Initial Time ===================

		public int? InitialTimeMins
		{
			get
			{
				if (SelectedSetting != null && SelectedSetting.InitialTime != null)
					return (int)Math.Floor(SelectedSetting.InitialTime.Value / 60.0);
				return null;
			}
			set
			{
				if (SelectedSetting != null)
				{
					SelectedSetting.InitialTime = value.GetValueOrDefault() * 60 + InitialTimeSecs.GetValueOrDefault();
					NotifyChanged();
					NotifyChanged(() => InitialTimeSecs);
				}
			}
		}

		public int? InitialTimeSecs
		{
			get
			{
				if (SelectedSetting != null && SelectedSetting.InitialTime != null)
					return (int)Math.Floor(SelectedSetting.InitialTime.Value % 60.0);
				return null;
			}
			set
			{
				if (SelectedSetting != null)
				{
					SelectedSetting.InitialTime = InitialTimeMins.GetValueOrDefault() * 60 + value.GetValueOrDefault();
					NotifyChanged();
					NotifyChanged(() => InitialTimeMins);
				}
			}
		}

		// =================== Increment Time ===================

		public int? IncrementTimeMins
		{
			get
			{
				if (SelectedSetting != null && SelectedSetting.MoveIncrement != null)
					return (int)Math.Floor(SelectedSetting.MoveIncrement.Value / 60.0);
				return null;
			}
			set
			{
				if (SelectedSetting != null)
				{
					SelectedSetting.MoveIncrement = value.GetValueOrDefault() * 60 + IncrementTimeSecs.GetValueOrDefault();
					NotifyChanged();
					NotifyChanged(() => IncrementTimeSecs);
				}
			}
		}

		public int? IncrementTimeSecs
		{
			get
			{
				if (SelectedSetting != null && SelectedSetting.MoveIncrement != null)
					return (int)Math.Floor(SelectedSetting.MoveIncrement.Value % 60.0);
				return null;
			}
			set
			{
				if (SelectedSetting != null)
				{
					SelectedSetting.MoveIncrement = IncrementTimeMins.GetValueOrDefault() * 60 + value.GetValueOrDefault();
					NotifyChanged();
					NotifyChanged(() => IncrementTimeMins);
				}
			}
		}

		// =================== Time Control Length ===================

		public int? TimeControlMins
		{
			get
			{
				if (SelectedSetting != null && SelectedSetting.TimeControlWindow != null)
					return (int)Math.Floor(SelectedSetting.TimeControlWindow.Value / 60.0);
				return null;
			}
			set
			{
				if (SelectedSetting != null)
				{
					SelectedSetting.TimeControlWindow = value.GetValueOrDefault() * 60 + TimeControlSecs.GetValueOrDefault();
					NotifyChanged();
					NotifyChanged(() => TimeControlSecs);
				}
			}
		}

		public int? TimeControlSecs
		{
			get
			{
				if (SelectedSetting != null && SelectedSetting.TimeControlWindow != null)
					return (int)Math.Floor(SelectedSetting.TimeControlWindow.Value % 60.0);
				return null;
			}
			set
			{
				if (SelectedSetting != null)
				{
					SelectedSetting.TimeControlWindow = TimeControlMins.GetValueOrDefault() * 60 + value.GetValueOrDefault();
					NotifyChanged();
					NotifyChanged(() => TimeControlMins);
				}
			}
		}

		private void CreateNewSetting()
		{
			SelectedSetting = MasterState.Instance.CreateNewTimeSetting();
			NotifyChanged(() => TimeSettings);
		}
	}
}
