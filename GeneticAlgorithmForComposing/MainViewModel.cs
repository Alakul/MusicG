using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmForComposing
{
    class MainViewModel
    {
        public MainViewModel()
        {
            this.MeasureValue = 4;
            this.PopulationSize = 40;
            this.GenerationValue = 100;

            this.CrossoverProbability = 0.755;
            this.MutationProbability = 0.05;
        }

        public int MeasureValue { get; set; }
        public int PopulationSize { get; set; }
        public int GenerationValue { get; set; }

        //Probability
        public double CrossoverProbability { get; set; }
        public double MutationProbability { get; set; }
    }
}
