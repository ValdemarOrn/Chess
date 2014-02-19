using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Testbed.Control;

namespace Chess.Testbed.Views
{
	public class SchedulerViewModel : ViewModelBase
	{
		public SchedulerViewModel()
		{
			MasterState.Instance.RegisterAction(MasterState.EventEnginesChanged, () => NotifyChanged(() => Engines));
			MasterState.Instance.RegisterAction(MasterState.EventTimeSettingsChanged, () => NotifyChanged(() => TimeSettings));
			MasterState.Instance.RegisterAction(MasterState.EventScheduledMatchesChanged, () => NotifyChanged(() => ScheduledMatches));
			InsertMachesCommand = new ModelCommand(InsertMaches);
			ReloadScheduledMatchesCommand = new ModelCommand(ReloadScheduledMatches);
			MatchCount = 1;
			PlayWhite = true;
			PlayBlack = true;
		}
    
		public ModelCommand InsertMachesCommand { get; private set; }
		public ModelCommand ReloadScheduledMatchesCommand { get; private set; }

		public ObservableCollection<UciEngineSettings> Engines
		{
			get { return new ObservableCollection<UciEngineSettings>(MasterState.Instance.Engines); }
		}

		public ObservableCollection<TimeSettings> TimeSettings
		{
			get { return new ObservableCollection<TimeSettings>(MasterState.Instance.TimeSettings); }
		}

		public ObservableCollection<ScheduledMatch> ScheduledMatches
		{
			get { return new ObservableCollection<ScheduledMatch>(MasterState.Instance.ScheduledMatches); }
		}

		private UciEngineSettings competitor;
		public UciEngineSettings Competitor
		{
			get { return competitor; }
			set { competitor = value; NotifyChanged(); }
		}

		private UciEngineSettings[] opponents;
		public UciEngineSettings[] Opponents
		{
			get { return opponents; }
			set { opponents = value; NotifyChanged(); }
		}

		private TimeSettings timeSetting;
		public TimeSettings TimeSetting
		{
			get { return timeSetting; }
			set { timeSetting = value; NotifyChanged(); }
		}

		private int matchCount;
		public int MatchCount
		{
			get { return matchCount; }
			set { matchCount = value; NotifyChanged(); }
		}

		private bool playWhite;
		public bool PlayWhite
		{
			get { return playWhite; }
			set { playWhite = value; NotifyChanged(); }
		}

		private bool playBlack;
		public bool PlayBlack
		{
			get { return playBlack; }
			set { playBlack = value; NotifyChanged(); }
		}

		private void ReloadScheduledMatches()
		{
			NotifyChanged(() => ScheduledMatches);
		}

		private void InsertMaches()
		{
			if (Competitor == null || Opponents == null || Opponents.Length == 0)
			{
				Log.WarnDialog("You must select a competitor and at least one opponent");
				return;
			}

			if (TimeSetting == null)
			{
				Log.WarnDialog("You must select a time control setting");
				return;
			}

			var competitorId = Competitor.Id;
			var opponentIds = Opponents.Select(x => x.Id).ToArray();
			var timeControlId = TimeSetting.Id;
			var matches = new List<ScheduledMatch>();

			for (int i = 0; i < MatchCount; i++)
			{
				for (int j = 0; j < opponentIds.Length; j++)
				{
					if (PlayWhite)
						matches.Add(new ScheduledMatch {WhiteId = competitorId, BlackId = opponentIds[j], TimeControlId = timeControlId});
					if (PlayBlack)
						matches.Add(new ScheduledMatch { WhiteId = opponentIds[j], BlackId = competitorId, TimeControlId = timeControlId });
				}
			}

			MasterState.Instance.QueueMatches(matches);
		}

	}
}
