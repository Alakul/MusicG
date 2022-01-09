using GeneticAlgorithmForComposing.Commands;
using Manufaktura.Controls.Audio;
using Manufaktura.Controls.Desktop.Audio;
using Manufaktura.Controls.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Manufaktura.Controls.Audio.ScorePlayer;

namespace GeneticAlgorithmForComposing
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] chromosomeChoosen;
        string[] chromosomeChanged;
        double chromosomeChoosenEvaluation;

        List<double> evaluation;
        string[][] population;

        double measuresValue;
        string[] scaleSelected;
        int generationsSum;

        string[] semitonesSelected;
        int selectedScaleValue;
        string scaleSet;
        string scaleName;
        Dictionary<string, string[]> selectedScaleDictionary;

        //MUSIC
        MainViewModel viewModel;
        public static Score score;

        private void OnComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selection.SelectedIndex.ToString() == "1"){
                tournament.Text = "2";
            }
        }

        private void PlayButton(object sender, RoutedEventArgs e)
        {
            //Execute command
            object parameter = null;
            PlayCommand playCommand = new PlayCommand(viewModel);
            playCommand.Execute(parameter);

            tmp.IsEnabled = false;
            play.IsEnabled = false;
            stop.IsEnabled = true;
        }

        private void Stop()
        {
            //Execute command
            object parameter = null;
            StopCommand stopCommand = new StopCommand(viewModel);
            stopCommand.Execute(parameter);
        }

        public async void StopButton(object sender, RoutedEventArgs e)
        {
            Stop();
            await Task.Delay(2000);

            stop.IsEnabled = false;
            tmp.IsEnabled = true;
            play.IsEnabled = true;
        }

        private void SaveToMIDIButton(object sender, RoutedEventArgs e)
        {
            chromosomeChanged = MusicController.CheckNote(chromosomeChoosen, semitonesSelected);
            MusicController.SaveToMIDI(chromosomeChanged, scaleName);
        }

        private string[] SetDictionary()
        {
            //SET SCALE DICTIONARY
            int selectedScale = int.Parse(scale.SelectedIndex.ToString());
            selectedScaleValue = int.Parse(scaleValue.SelectedIndex.ToString());
            selectedScaleDictionary = new Dictionary<string, string[]>();

            if (selectedScale == 0){
                selectedScaleDictionary = new Dictionary<string, string[]>(MusicData.scaleMajor);
                scaleSet = "major";
            }
            else if (selectedScale == 1){
                selectedScaleDictionary = new Dictionary<string, string[]>(MusicData.scaleMinor);
                scaleSet = "minor";
            }
            string[] scaleSelected = selectedScaleDictionary.Values.ElementAt(selectedScaleValue);
            
            return scaleSelected;
        }

        private void ComposeButton(object sender, RoutedEventArgs e)
        {
            //MUSIC PARAMETERS
            measuresValue = double.Parse(measures.Text);
            scaleSelected = SetDictionary();

            //GENETIC PARAMETERS
            int populationValue = int.Parse(populationSize.Text);

            //GENETIC ALGORITHM
            semitonesSelected = GeneticAlgorithm.SetSign(scaleSelected);
            population = GeneticAlgorithm.GeneratePopulation(populationValue, scaleSelected, measuresValue, semitonesSelected);
            generationsSum = 0;
            StartGeneticAlgorithm();
            Set();
        }

        private void ContinueButton(object sender, RoutedEventArgs e)
        {
            StartGeneticAlgorithm();
            Set();
        }

        private void StartGeneticAlgorithm()
        {
            //GENETIC PARAMETERS
            int iterationsValue = int.Parse(generations.Text);
            int crossoverProbabilityValue = int.Parse(probCrossover.Text);
            int mutationProbabilityValue = int.Parse(probMutation.Text);
            int tournamentSize;
            int prefferedOctave = int.Parse(octave.Text);

            //GENETIC ALGORITHM
            evaluation = GeneticAlgorithm.FitnessFunction(population, semitonesSelected, scaleSelected, prefferedOctave);
            int counter = 0;

            //OPERATORS
            int selectedSelection = int.Parse(selection.SelectedIndex.ToString());
            int selectedMutation = int.Parse(mutation.SelectedIndex.ToString());

            string[][] populationAfterSelection;
            string[][] populationAfterCrossover;
            string[][] populationAfterMutation;

            chromosomeChoosenEvaluation = evaluation.Max();
            chromosomeChoosen = population[evaluation.IndexOf(chromosomeChoosenEvaluation)];
            generationsSum += iterationsValue;

            while (counter <= iterationsValue){
                //SELECTION
                if (selectedSelection == 0){
                    tournamentSize = int.Parse(tournament.Text);
                    populationAfterSelection = GeneticAlgorithm.TournamentSelection(population, evaluation, tournamentSize);
                }
                else {
                    populationAfterSelection = GeneticAlgorithm.RouletteWheelSelection(population, evaluation);
                }

                //CROSSOVER
                populationAfterCrossover = GeneticAlgorithm.Crossover(populationAfterSelection, crossoverProbabilityValue, measuresValue, semitonesSelected);

                //MUTATION
                if (selectedMutation == 0){
                    populationAfterMutation = GeneticAlgorithm.MutationSemitones(populationAfterCrossover, mutationProbabilityValue);
                }
                else {
                    populationAfterMutation = GeneticAlgorithm.MutationOctave(populationAfterCrossover, mutationProbabilityValue);
                }

                //EVALUATION
                evaluation = GeneticAlgorithm.FitnessFunction(populationAfterMutation, semitonesSelected, scaleSelected, prefferedOctave);
                population = populationAfterMutation;

                chromosomeChoosenEvaluation = evaluation.Max();
                chromosomeChoosen = population[evaluation.IndexOf(chromosomeChoosenEvaluation)];

                /*
                if (evaluation.Max() > chromosomeChoosenEvaluation){
                    chromosomeChoosenEvaluation = evaluation.Max();
                    chromosomeChoosen = population[evaluation.IndexOf(chromosomeChoosenEvaluation)];
                }
                */
                counter++;
            }
        }

        private void Set()
        {
            //CHOOSEN
            string[] chromosomeChoosenDecoded = GeneticAlgorithm.DecodeChromosome(chromosomeChoosen, semitonesSelected);

            string choosen = string.Join(" ", chromosomeChoosenDecoded);
            show.Text = choosen.ToString() + " " + chromosomeChoosenEvaluation.ToString();

            showEvaluation.Text = "Ocena: " + Math.Round(chromosomeChoosenEvaluation, 2).ToString();
            showGenerations.Text = "Generacje: " + generationsSum.ToString();

            //Buttons
            play.IsEnabled = true;
            saveAsMIDI.IsEnabled = true;
            stop.IsEnabled = false;
            tmp.IsEnabled = true;

            //Music notation
            scaleName = selectedScaleDictionary.ElementAt(selectedScaleValue).Key;
            score = MusicController.WriteSheetMusic(chromosomeChoosen, semitonesSelected, scaleSet, scaleName);
            noteViewer.ScoreSource = score;

            //Execute command
            object parameter = null;
            OpenCommand openCommand = new OpenCommand(viewModel);
            openCommand.Execute(parameter);
        }

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;

            scaleSet = "major";

            //Buttons
            play.IsEnabled = false;
            saveAsMIDI.IsEnabled = false;
            stop.IsEnabled = false;

            //ComboBox scale values
            scale.ItemsSource = MusicData.scaleValues;
        }
    }
}
