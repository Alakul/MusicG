using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GeneticAlgorithmForComposing.Commands
{
	public abstract class PlayerCommand : ICommand
	{
		protected MainViewModel viewModel;

		protected PlayerCommand(MainViewModel viewModel)
		{
			this.viewModel = viewModel;
		}

		public event EventHandler CanExecuteChanged;

		public abstract bool CanExecute(object parameter);

		public abstract void Execute(object parameter);

		public void FireCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}
}
