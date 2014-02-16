using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Testbed
{
	[Serializable]
	public class MasterState
	{
		public const string EventEnginesChanged = "EnginesChanged";

		private static MasterState state = new MasterState();
		public static MasterState Instance { get { return state; } }


		private List<UciEngineSettings> engines;
		private Dictionary<string, Action> actionDictionary;

		private MasterState()
		{
			engines = new List<UciEngineSettings>();
			actionDictionary = new Dictionary<string, Action>();
		}

		public IEnumerable<UciEngineSettings> Engines { get { return engines; } }
		
		public UciEngineSettings CreateNewEngine()
		{
			var engine = new UciEngineSettings { Name = "New Engine" };
			engines.Add(engine);
			return engine;
		}

		public void RemoveEngine(UciEngineSettings engine)
		{
			if (engines.Contains(engine))
				engines.Remove(engine);
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
			return Serializer.SerializeToXml(new SerializedState { EngineSettings = Engines.ToArray() });
		}

		public void DeserializeState(string data)
		{
			var s = (SerializedState)Serializer.DeserializeFromXml(data, typeof(SerializedState));
			engines = s.EngineSettings.ToList();
		}

		[Serializable]
		public class SerializedState
		{
			public UciEngineSettings[] EngineSettings { get; set; }
		}

		
	}
}
