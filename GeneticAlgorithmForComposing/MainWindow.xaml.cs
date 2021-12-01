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
        string[] kodowanyChromosom;
        string[] semitonesSelected;


        private void PlayButton(object sender, RoutedEventArgs e)
        {
            GeneticAlgorithm.Play(kodowanyChromosom, semitonesSelected);
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
            //double crossoverProbabilityValue = double.Parse(probCrossover.Text);
            //double MutationProbabilityValue = double.Parse(probMutation.Text);

            //GENETIC ALGORITHM
            string[] chromosomeChoosen;
            string[][] population;


            //**************************
            semitonesSelected = SetSign(scaleSelected);
            string[] chromosom = GeneticAlgorithm.GenerateChromosome(scaleSelected, measuresValue);
            kodowanyChromosom = GeneticAlgorithm.CodeChromosome(chromosom, semitonesSelected);
            string[] dekodowanyChromosom = GeneticAlgorithm.DecodeChromosome(kodowanyChromosom, semitonesSelected);

            string[][] populacja = GeneticAlgorithm.GeneratePopulation(populationValue, scaleSelected, measuresValue, semitonesSelected);
            List<double> ocena = GeneticAlgorithm.OcenaPopulacji(populacja, semitonesSelected);
            string[][] wysel;


            int selectedSelection = int.Parse(selection.SelectedIndex.ToString());
            if (selectedSelection == 0){
                wysel = GeneticAlgorithm.TournamentSelection(populacja, ocena, 2);
            }
            else {
                wysel = GeneticAlgorithm.RouletteWheelSelection(populacja, ocena);
            }
            

            string[][] krzyzowanie = GeneticAlgorithm.Crossover(wysel, 0.755, measuresValue, semitonesSelected);

            //**************************


            //Show
            string rozw = string.Join(" ", krzyzowanie[0]);
            show.Text = rozw.ToString();

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
