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

namespace GeneticAlgorithmForComposing
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] chromosomeChoosen;
        string[] semitonesSelected;


        private void PlayButton(object sender, RoutedEventArgs e)
        {
            GeneticAlgorithm.Play(chromosomeChoosen, semitonesSelected);
        }

        private void SaveToMIDIButton(object sender, RoutedEventArgs e)
        {
            GeneticAlgorithm.SaveToMIDI(chromosomeChoosen, "nazwa333", semitonesSelected);
            show.Text = "Zapisano";
        }

        private string[] SetDictionary()
        {
            //SET SCALE DICTIONARY
            int selectedScale = int.Parse(scale.SelectedIndex.ToString());
            int selectedScaleValue = int.Parse(scaleValue.SelectedIndex.ToString());
            Dictionary<string, string[]> selectedScaleDictionary = new Dictionary<string, string[]>();

            if (selectedScale == 0){
                selectedScaleDictionary = new Dictionary<string, string[]>(Music.scaleMajor);
            }
            else if (selectedScale == 1){
                selectedScaleDictionary = new Dictionary<string, string[]>(Music.scaleMinor);
            }
            string[] scaleSelected = selectedScaleDictionary.Values.ElementAt(selectedScaleValue);

            return scaleSelected;
        }

        public string[] SetSign(string[] scale)
        {
            string[] semitonesSelected = new string[12];
            for (int i = 0; i < scale.Length; i++){
                if (scale[i].Contains('#') == true){
                    semitonesSelected = Music.semitonesSharp;
                    break;
                }
                else if (scale[i].Contains('b') == true){
                    semitonesSelected = Music.semitonesFlat;
                    break;
                }
                else {
                    semitonesSelected = Music.semitonesSharp;
                }
            }
            return semitonesSelected;
        }

        public void GetSelection()
        {
            


        }

        public void GetMutation()
        {

        }

        private void Compose(object sender, RoutedEventArgs e)
        {
            //MUSIC PARAMETERS
            double measuresValue = double.Parse(measures.Text);
            string[] scaleSelected = SetDictionary();

            //GENETIC PARAMETERS
            int populationValue = int.Parse(populationSize.Text);
            int iterationsValue = int.Parse(generations.Text);
            int crossoverProbabilityValue = int.Parse(probCrossover.Text);
            int mutationProbabilityValue = int.Parse(probMutation.Text);
            int tournamentSize;

            //GENETIC ALGORITHM
            double chromosomeChoosenEvaluation;//ocena wybranego osobnika

            semitonesSelected = SetSign(scaleSelected);
            string[][] population = GeneticAlgorithm.GeneratePopulation(populationValue, scaleSelected, measuresValue, semitonesSelected);
            List<double> evaluationPopulation;
            string[][] populationAfterSelection;
            string[][] populationAfterCrossover;
            string[][] populationAfterMutation;
            List<double> evaluation = GeneticAlgorithm.FitnessFunction(population, semitonesSelected, scaleSelected);
            int counter = 0;

            //OPERATORS
            int selectedSelection = int.Parse(selection.SelectedIndex.ToString());
            //int selectedMutation = int.Parse(selection.SelectedIndex.ToString());

            chromosomeChoosenEvaluation = evaluation.Max();
            chromosomeChoosen = population[evaluation.IndexOf(chromosomeChoosenEvaluation)];

            while (counter <= iterationsValue)
            {
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
                if (selectedSelection == 0){
                    populationAfterMutation = GeneticAlgorithm.MutationSemitones(populationAfterCrossover, mutationProbabilityValue);
                }
                else {
                    populationAfterMutation = GeneticAlgorithm.MutationSemitones(populationAfterCrossover, mutationProbabilityValue);
                }

                //EVALUATION
                evaluation = GeneticAlgorithm.FitnessFunction(populationAfterMutation, semitonesSelected, scaleSelected);
                population = populationAfterMutation;
                evaluationPopulation = evaluation;

                if (evaluation.Max() > chromosomeChoosenEvaluation){
                    chromosomeChoosenEvaluation = evaluation.Max();
                    chromosomeChoosen = population[evaluation.IndexOf(chromosomeChoosenEvaluation)];
                }

                counter++;
            }



            //CHOOSEN
            string[] chromosomeChoosenDecoded = GeneticAlgorithm.DecodeChromosome(chromosomeChoosen, semitonesSelected);
            string choosen = string.Join(" ", chromosomeChoosenDecoded);
            show.Text = choosen.ToString() +" "+ chromosomeChoosenEvaluation.ToString();


            //**************************
            /*
            semitonesSelected = SetSign(scaleSelected);
            string[] chromosom = GeneticAlgorithm.GenerateChromosome(scaleSelected, measuresValue);
            kodowanyChromosom = GeneticAlgorithm.CodeChromosome(chromosom, semitonesSelected);
            string[] dekodowanyChromosom = GeneticAlgorithm.DecodeChromosome(kodowanyChromosom, semitonesSelected);

            string[][] populacja = GeneticAlgorithm.GeneratePopulation(populationValue, scaleSelected, measuresValue, semitonesSelected);
            List<double> ocena = GeneticAlgorithm.OcenaPopulacji(populacja, semitonesSelected);
            string[][] wysel;


            
            if (selectedSelection == 0){
                tournamentSize = int.Parse(tournament.Text);
                wysel = GeneticAlgorithm.TournamentSelection(populacja, ocena, tournamentSize);
            }
            else {
                wysel = GeneticAlgorithm.RouletteWheelSelection(populacja, ocena);
            }
            

            string[][] krzyzowanie = GeneticAlgorithm.Crossover(wysel, 755, measuresValue, semitonesSelected);
            */
            //**************************


            //Buttons
            play.IsEnabled = true;
            saveAsMIDI.IsEnabled = true;
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            //Buttons
            play.IsEnabled = false;
            saveAsMIDI.IsEnabled = false;

            //ComboBox scale values
            scale.ItemsSource = Music.scaleValues;
        }
    }
}
