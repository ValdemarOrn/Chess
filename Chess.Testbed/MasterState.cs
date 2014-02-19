using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Chess.Testbed
{
	[Serializable]
	public class MasterState
	{
		public static string ExeDir = Path.GetDirectoryName(Assembly.GetAssembly(typeof (MasterState)).Location);

		public const string EventEnginesChanged = "EnginesChanged";
		public const string EventTimeSettingsChanged = "TimeSettingsChanged";
		public const string EventScheduledMatchesChanged = "ScheduledMatchesChanged";

		private static MasterState state = new MasterState();
		public static MasterState Instance { get { return state; } }

		private Dictionary<string, Action> actionDictionary;

		private List<UciEngineSettings> engines;
		private List<TimeSettings> timeSettings;
		private Queue<ScheduledMatch> scheduledMatches;
		
		private MasterState()
		{
			engines = new List<UciEngineSettings>();
			timeSettings = new List<TimeSettings>();
			actionDictionary = new Dictionary<string, Action>();
			scheduledMatches = new Queue<ScheduledMatch>();
		}

		public IEnumerable<UciEngineSettings> Engines { get { return engines; } }
		public IEnumerable<TimeSettings> TimeSettings { get { return timeSettings; } }
		public IEnumerable<ScheduledMatch> ScheduledMatches { get { return scheduledMatches; } }

		public UciEngineSettings CreateNewEngine()
		{
			var engine = new UciEngineSettings { Name = "New Engine" };
			engine.Id = engines.Max(x => x.Id) + 1;
			engines.Add(engine);
			TriggerAction(EventEnginesChanged);
			return engine;
		}

		public void RemoveEngine(UciEngineSettings engine)
		{
			if (engines.Contains(engine))
			{
				engines.Remove(engine);
				TriggerAction(EventEnginesChanged);
			}
		}

		public TimeSettings CreateNewTimeSetting()
		{
			var setting = new TimeSettings { Name = "New Time Setting" };
			setting.Id = timeSettings.Max(x => x.Id) + 1;
			timeSettings.Add(setting);
			TriggerAction(EventTimeSettingsChanged);
			return setting;
		}

		public void RemoveTimeSetting(TimeSettings setting)
		{
			if (timeSettings.Contains(setting))
			{
				timeSettings.Remove(setting);
				TriggerAction(EventTimeSettingsChanged);
			}
		}

		public void QueueMatches(IEnumerable<ScheduledMatch> matches)
		{
			foreach (var match in matches)
				scheduledMatches.Enqueue(match);

			TriggerAction(EventScheduledMatchesChanged);
		}

		public ScheduledMatch DequeueMatch()
		{
			if (scheduledMatches.Count == 0)
				return null;

			var match = scheduledMatches.Dequeue();
			TriggerAction(EventScheduledMatchesChanged);
			return match;
		}

		public void RegisterAction(string name, Action action)
		{
			if (actionDictionary.ContainsKey(name))
				actionDictionary[name] += action;
			else
				actionDictionary[name] = action;
		}

		public void TriggerAction(string name)
		{
			if (!actionDictionary.ContainsKey(name))
				return;

			var action = actionDictionary[name];
			action.Invoke();
		}

		public string SerializeState()
		{
			return Serializer.SerializeToXml(new SerializedState
			{
				EngineSettings = Engines.ToArray(),
				TimeSettings = TimeSettings.ToArray(),
				ScheduledMatches = ScheduledMatches.ToArray()
			});
		}

		public void DeserializeState(string data)
		{
			var s = Serializer.DeserializeFromXml<SerializedState>(data);
			
			if (s.EngineSettings != null)
				engines = s.EngineSettings.ToList();
			
			if (s.TimeSettings != null)
				timeSettings = s.TimeSettings.ToList();

			if (s.ScheduledMatches != null)
				scheduledMatches = new Queue<ScheduledMatch>(s.ScheduledMatches);
		}

		[Serializable]
		public class SerializedState
		{
			public UciEngineSettings[] EngineSettings { get; set; }
			public TimeSettings[] TimeSettings { get; set; }
			public ScheduledMatch[] ScheduledMatches { get; set; }
		}

		
	}
}
