using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmForComposing.Commands
{
    public class OpenCommand : PlayerCommand
    {
        public OpenCommand(MainViewModel viewModel) : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            viewModel.Score = AppWindow.score;
        }
    }
}
