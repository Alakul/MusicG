using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MusicG.Commands;
using Manufaktura.Controls.Model;

namespace MusicG.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy GenerateMusic.xaml
    /// </summary>
    public partial class GenerateMusic : UserControl
    {
        GeneticAlgorithm geneticAlgorithm;
        Chromosome chromosomeChoosen;
        double chromosomeChoosenEvaluation;
        double[,] evaluationArray;
        List<double> fitnessList;
        int generationsCount;
        
        int selectedScaleValue;
        string scaleSet;
        string scaleName;
        Dictionary<string, string[]> selectedScaleDictionary;

        //MUSIC
        MainViewModel viewModel;

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
            //COMMAND
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
            chromosomeChoosen = MusicAdapter.CheckNote(chromosomeChoosen, geneticAlgorithm.SemitonesSelected);
            MusicAdapter.SaveToMIDI(chromosomeChoosen, scaleName);
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
            geneticAlgorithm = null;
            StartGeneticAlgorithm();
        }

        private void ContinueButton(object sender, RoutedEventArgs e)
        {
            StartGeneticAlgorithm();
        }

        public void StartGeneticAlgorithm()
        {
            int populationValue = int.Parse(populationSize.Text);
            int generationsValue = int.Parse(generations.Text);
            int crossoverProbabilityValue = int.Parse(probCrossover.Text);
            int mutationProbabilityValue = int.Parse(probMutation.Text);
            int prefferedOctave = int.Parse(octave.Text);

            int tournamentSize = int.Parse(tournament.Text);
            int selectedSelection = int.Parse(selection.SelectedIndex.ToString());
            int selectedMutation = int.Parse(mutation.SelectedIndex.ToString());

            double[] criteriaWeight = { Convert.ToDouble(criterion1Weight.Text), Convert.ToDouble(criterion2Weight.Text), Convert.ToDouble(criterion3Weight.Text), Convert.ToDouble(criterion4Weight.Text) };
            double[] intervalWeight = { Convert.ToDouble(step1Weight.Text), Convert.ToDouble(step2Weight.Text), Convert.ToDouble(step3Weight.Text),
                Convert.ToDouble(step4Weight.Text), Convert.ToDouble(step5Weight.Text), Convert.ToDouble(step6Weight.Text),
                Convert.ToDouble(step7Weight.Text), Convert.ToDouble(step8Weight.Text), Convert.ToDouble(step9Weight.Text) };

            if (geneticAlgorithm == null){
                string[] scaleSelected = SetDictionary();
                double measuresValue = double.Parse(measures.Text);

                GeneticAlgorithm newGeneticAlgorithm = new GeneticAlgorithm(scaleSelected, measuresValue, prefferedOctave, populationValue,
                    crossoverProbabilityValue, mutationProbabilityValue, generationsValue, criteriaWeight, intervalWeight,
                    tournamentSize, selectedSelection, selectedMutation);

                geneticAlgorithm = newGeneticAlgorithm;
            }
            else {
                geneticAlgorithm.NewGeneration(generationsValue, prefferedOctave, crossoverProbabilityValue,
                    mutationProbabilityValue, criteriaWeight, intervalWeight, tournamentSize, selectedSelection,
                    selectedMutation);
            }

            generationsCount = geneticAlgorithm.Generations;
            chromosomeChoosenEvaluation = geneticAlgorithm.GetTheBestFitness();
            chromosomeChoosen = geneticAlgorithm.GetTheBestChromosome();
            fitnessList = geneticAlgorithm.Fitness;
            evaluationArray = geneticAlgorithm.EvaluationArray;

            SetViewElements();
            SetDetails(criteriaWeight, evaluationArray, fitnessList.IndexOf(chromosomeChoosenEvaluation));
        }

        private void SetDetails(double[] criteriaWeight, double[,] evaluationArray, int evaluationIndex)
        {
            criterion1WText.Text = "Waga";
            criterion1EText.Text = "Ocena";
            criterion1MText.Text = "Maksymalna ocena";

            for (int i=0; i<4; i++){
                TextBlock criterion = (TextBlock)FindName("criterion" + (i+ 1).ToString() + "Text");
                criterion.Text = "Kryterium " + (i + 1).ToString();

                TextBlock criterionW = (TextBlock)FindName("criterion" + (i + 1).ToString() + "W");
                criterionW.Text = criteriaWeight[i].ToString();

                TextBlock criterionE = (TextBlock)FindName("criterion" + (i + 1).ToString() + "E");
                criterionE.Text = Math.Round(evaluationArray[evaluationIndex, i], 2).ToString();

                TextBlock criterionM = (TextBlock)FindName("criterion" + (i + 1).ToString() + "M");
                criterionM.Text = "1";
            }
        }

        private void SetViewElements()
        {
            showEvaluation.Text = "Ocena: " + Math.Round(chromosomeChoosenEvaluation, 2).ToString();
            showGenerations.Text = "Generacje: " + generationsCount.ToString();

            //BUTTTONS
            play.IsEnabled = true;
            saveAsMIDI.IsEnabled = true;
            stop.IsEnabled = false;
            tmp.IsEnabled = true;

            //NOTATION
            scaleName = selectedScaleDictionary.ElementAt(selectedScaleValue).Key;
            AppWindow.score = MusicAdapter.WriteSheetMusic(chromosomeChoosen, scaleSet, scaleName);
            noteViewer.ScoreSource = AppWindow.score;

            //COMMAND
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

            //BUTTONS
            play.IsEnabled = false;
            saveAsMIDI.IsEnabled = false;
            stop.IsEnabled = false;

            //SCALE VALUES
            scale.ItemsSource = MusicData.scaleValues;
        }
    }
}
