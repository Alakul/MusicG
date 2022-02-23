using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GeneticAlgorithmForComposing.Commands;
using Manufaktura.Controls.Model;

namespace GeneticAlgorithmForComposing.Menu
{
    /// <summary>
    /// Logika interakcji dla klasy GenerateMusic.xaml
    /// </summary>
    public partial class GenerateMusic : UserControl
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
        //public static Score score;

        private void BackButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }

        private void EnterButton(object sender, RoutedEventArgs e)
        {
            imageArrow.Source = new BitmapImage(new Uri(@"/Images/arrowGray.png", UriKind.Relative));
        }

        private void LeaveButton(object sender, RoutedEventArgs e)
        {
            imageArrow.Source = new BitmapImage(new Uri(@"/Images/arrowBlack.png", UriKind.Relative));
        }

        private void OnComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selection.SelectedIndex.ToString() == "1") {
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

            if (selectedScale == 0)
            {
                selectedScaleDictionary = new Dictionary<string, string[]>(MusicData.scaleMajor);
                scaleSet = "major";
            }
            else if (selectedScale == 1)
            {
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

            double[] criteriaWeight = { Convert.ToDouble(criterion1Weight.Text), Convert.ToDouble(criterion2Weight.Text), Convert.ToDouble(criterion3Weight.Text), Convert.ToDouble(criterion4Weight.Text) };
            double[] intervalWeight = { Convert.ToDouble(step1Weight.Text), Convert.ToDouble(step2Weight.Text), Convert.ToDouble(step3Weight.Text),
                Convert.ToDouble(step4Weight.Text), Convert.ToDouble(step5Weight.Text), Convert.ToDouble(step6Weight.Text),
                Convert.ToDouble(step7Weight.Text), Convert.ToDouble(step8Weight.Text), Convert.ToDouble(step9Weight.Text) };

            //GENETIC ALGORITHM
            (List<double> evaluationValue, double[,] evaluationArray) = GeneticAlgorithm.FitnessFunction(population, semitonesSelected, scaleSelected, prefferedOctave, criteriaWeight, intervalWeight);
            evaluation = evaluationValue;
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

            while (counter <= iterationsValue)
            {
                //SELECTION
                if (selectedSelection == 0)
                {
                    tournamentSize = int.Parse(tournament.Text);
                    populationAfterSelection = GeneticAlgorithm.TournamentSelection(population, evaluation, tournamentSize);
                }
                else
                {
                    populationAfterSelection = GeneticAlgorithm.RouletteWheelSelection(population, evaluation);
                }

                //CROSSOVER
                populationAfterCrossover = GeneticAlgorithm.Crossover(populationAfterSelection, crossoverProbabilityValue, measuresValue, semitonesSelected);

                //MUTATION
                if (selectedMutation == 0)
                {
                    populationAfterMutation = GeneticAlgorithm.MutationSemitones(populationAfterCrossover, mutationProbabilityValue);
                }
                else
                {
                    populationAfterMutation = GeneticAlgorithm.MutationOctave(populationAfterCrossover, mutationProbabilityValue);
                }

                //EVALUATION
                (evaluationValue, evaluationArray) = GeneticAlgorithm.FitnessFunction(populationAfterMutation, semitonesSelected, scaleSelected, prefferedOctave, criteriaWeight, intervalWeight);
                evaluation = evaluationValue;
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
            SetDetails(criteriaWeight, evaluationArray, evaluation.IndexOf(chromosomeChoosenEvaluation));
        }

        private void SetDetails(double[] criteriaWeight, double[,] evaluationArray, int evaluation)
        {
            criterion1Text.Text = "Kryterium 1";
            criterion2Text.Text = "Kryterium 2";
            criterion3Text.Text = "Kryterium 3";
            criterion4Text.Text = "Kryterium 4";
            criterion1WText.Text = "Waga";
            criterion1EText.Text = "Ocena";
            criterion1MText.Text = "Maksymalna ocena";

            criterion1W.Text = criteriaWeight[0].ToString();
            criterion2W.Text = criteriaWeight[1].ToString();
            criterion3W.Text = criteriaWeight[2].ToString();
            criterion4W.Text = criteriaWeight[3].ToString();

            criterion1E.Text = Math.Round(evaluationArray[evaluation, 0], 2).ToString();
            criterion2E.Text = Math.Round(evaluationArray[evaluation, 1], 2).ToString();
            criterion3E.Text = Math.Round(evaluationArray[evaluation, 2], 2).ToString();
            criterion4E.Text = Math.Round(evaluationArray[evaluation, 3], 2).ToString();

            criterion1M.Text = "1";
            criterion2M.Text = "1";
            criterion3M.Text = "1";
            criterion4M.Text = "1";
        }

        private void Set()
        {
            showEvaluation.Text = "Ocena: " + Math.Round(chromosomeChoosenEvaluation, 2).ToString();
            showGenerations.Text = "Generacje: " + generationsSum.ToString();

            //Buttons
            play.IsEnabled = true;
            saveAsMIDI.IsEnabled = true;
            stop.IsEnabled = false;
            tmp.IsEnabled = true;

            //Music notation
            scaleName = selectedScaleDictionary.ElementAt(selectedScaleValue).Key;
            AppWindow.score = MusicController.WriteSheetMusic(chromosomeChoosen, semitonesSelected, scaleSet, scaleName);
            noteViewer.ScoreSource = AppWindow.score;

            //Execute command
            object parameter = null;
            OpenCommand openCommand = new OpenCommand(viewModel);
            openCommand.Execute(parameter);
        }

        public GenerateMusic()
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
