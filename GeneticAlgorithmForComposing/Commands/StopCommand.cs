using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmForComposing.Commands
{
	public class StopCommand : PlayerCommand
	{
		public StopCommand(MainViewModel viewModel) : base(viewModel)
		{
		}

		public override bool CanExecute(object parameter)
		{
			return viewModel.Player != null;
		}

		public override void Execute(object parameter)
		{
			viewModel.Player?.Stop();
		}
	}
}
