using GeneticAlgorithmForComposing.Commands;
using Manufaktura.Controls.Audio;
using Manufaktura.Controls.Desktop.Audio;
using Manufaktura.Controls.Model;
using System;

namespace GeneticAlgorithmForComposing
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            OctaveValue = 4;
            MeasureValue = 4;
            PopulationSize = 40;
            GenerationValue = 10;

            CrossoverProbability = 755;
            MutationProbability = 55;

            TournamentValue = 2;

            OpenCommand = new OpenCommand(this);
            PlayCommand = new PlayCommand(this);
            StopCommand = new StopCommand(this);
        }

        public int OctaveValue { get; set; }
        public int MeasureValue { get; set; }
        public int PopulationSize { get; set; }
        public int GenerationValue { get; set; }

        public int CrossoverProbability { get; set; }
        public int MutationProbability { get; set; }

        public int TournamentValue { get; set; }



        private ScorePlayer player;
        private Score score;

        public OpenCommand OpenCommand { get; }
        public PlayCommand PlayCommand { get; }
        public ScorePlayer Player => player;

        public Score Score
        {
            get {
                return score;
            }
            set {
                score = value;
                if (player != null){
                    ((IDisposable)player).Dispose();
                }
                player = new MidiTaskScorePlayer(score);
                OnPropertyChanged();
                OnPropertyChanged(() => Player);
                PlayCommand?.FireCanExecuteChanged();
                StopCommand?.FireCanExecuteChanged();
            }
        }

        public StopCommand StopCommand { get; }
    }
}
