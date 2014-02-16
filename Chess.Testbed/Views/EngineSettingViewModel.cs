using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Testbed.Control;
using Chess.Uci;

namespace Chess.Testbed.Views
{
	public class EngineSettingViewModel : ViewModelBase
	{
		public EngineSettingViewModel(UciOption option) : this()
		{
			Option = option;
		}

		public EngineSettingViewModel()
		{
			SetDefaultCommand = new ModelCommand(SetDefault);
		}

		public ModelCommand SetDefaultCommand { get; private set; }

		UciOption option;
		public UciOption Option 
		{
			get { return option; }
			set
			{
				option = value;
				NotifyChanged(() => CheckBoxVisible);
				NotifyChanged(() => SpinnerVisible);
				NotifyChanged(() => ComboBoxVisible);
				NotifyChanged(() => ButtonVisible);
				NotifyChanged(() => StringVisible);
			}
		}

		public string Value
		{
			get { return Option != null ? Option.Value : null; }
			set
			{
				if (Option != null)
				{
					Option.Value = value;
					NotifyChanged();
				}
			}
		}

		public bool StringVisible
		{
			get { return Option != null && Option.Type == UciOptionType.String; }
		}

		public bool ComboBoxVisible
		{
			get { return Option != null && Option.Type == UciOptionType.Combo; }
		}

		public bool SpinnerVisible
		{
			get { return Option != null && Option.Type == UciOptionType.Spin; }
		}

		public bool CheckBoxVisible
		{
			get { return Option != null && Option.Type == UciOptionType.Check; }
		}

		public bool ButtonVisible
		{
			get { return Option != null && Option.Type == UciOptionType.Button; }
		}

		private void SetDefault()
		{
			Value = (Option != null && Option.DefaultValue != null) ? Option.DefaultValue.ToString() : null;
		}
	}
}
