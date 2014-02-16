using Chess.Uci;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Testbed
{
	[Serializable]
	public class UciEngineSettings
	{
		// Inputs
		public string Name{ get; set; }
		public string Command { get; set; }
		public string Parameters { get; set; }

		// Loaded from engine
		public string EngineId { get; set; }
		public string AuthorId { get; set; }
		public UciOption[] Options { get; set; }

		public UciEngineSettings()
		{
			Options = new UciOption[0];
		}

		public void LoadEngineData()
		{
			var engine = new UciProcess(this);
			try
			{
				engine.Start();
				EngineId = engine.IdName;
				AuthorId = engine.IdAuthor;
				var oldOptions = Options;
				Options = engine.Options.ToArray();

				// apply defaults
				foreach (var opt in Options)
				{
					opt.Value = (opt.DefaultValue ?? "").ToString();
				}

				// reapply the settings
				foreach (var oldOption in oldOptions)
				{
					var newOption = Options.SingleOrDefault(x => x.Name == oldOption.Name);
					if (newOption == null)
						continue;

					newOption.Value = oldOption.Value;
				}
			}
			finally
			{
				engine.Quit();
			}
		}
	}
}
