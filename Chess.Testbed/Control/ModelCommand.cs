using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Chess.Testbed.Control
{
	public class ModelCommand : ICommand
	{
		private readonly Action _execute;
		private readonly Func<bool> _canExecute;

		public ModelCommand(Action execute) : this(execute, null) { }

		public ModelCommand(Action execute, Func<bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return _canExecute != null ? _canExecute() : true;
		}

		public void Execute(object parameter)
		{
			if (_execute != null)
				_execute();
		}

		public void RefreshCanExecuteChanged()
		{
			CanExecuteChanged(this, EventArgs.Empty);
		}		
	}
}
