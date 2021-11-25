using System;
using System.Collections.Generic;
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
        public static string[] gamaCdur = new[] { "C", "D", "E", "F", "G", "A", "B", "C" };
        string[] kodowanyChromosom;
        public static IDictionary<string, List<string>> scaleValues;

        private void PlayButton(object sender, RoutedEventArgs e)
        {
            GeneticAlgorithm.Play(kodowanyChromosom);
        }

        private void Compose(object sender, RoutedEventArgs e)
        {
            string SelectedItem = scale.Text;

            //MUSIC PARAMETERS
            string scaleValue;
            double measuresValue = double.Parse(measures.Text);

            //GENETIC PARAMETERS
            int populationValue = int.Parse(populationSize.Text);
            int iterationsValue = int.Parse(generations.Text);
            //double crossoverProbabilityValue = double.Parse(probCrossover.Text);
            //double MutationProbabilityValue = double.Parse(probMutation.Text);

            //GENETIC ALGORITHM
            string[] chromosomeChoosen;
            string[][] population;


            //**************************

            string[] chromosom = GeneticAlgorithm.GenerateChromosome(gamaCdur, measuresValue);
            kodowanyChromosom = GeneticAlgorithm.CodeChromosome(chromosom);
            string[] dekodowanyChromosom = GeneticAlgorithm.DecodeChromosome(kodowanyChromosom);

            string[][] populacja = GeneticAlgorithm.GeneratePopulation(populationValue, gamaCdur, measuresValue);
            List<double> ocena = GeneticAlgorithm.OcenaPopulacji(populacja);
            string[][] wysel = GeneticAlgorithm.RouletteWheelSelection(populacja, ocena);
            string[][] krzyzowanie = GeneticAlgorithm.Crossover(wysel, 0.755);

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
            Console.Read();
            //Buttons
            play.IsEnabled = false;
            saveAsMIDI.IsEnabled = false;

            //ComboBox scale values
            scaleValues = new Dictionary<string, List<string>>();
            scaleValues.Add("Durowe", new List<string> { "C-dur", "G-dur", "D-dur", "A-dur", "E-dur", "B-dur", "Fis-dur", "Cis-dur", "F-dur", "B-dur", "Es-dur", "As-dur" });
            scaleValues.Add("Molowe", new List<string> { "A-moll", "E-moll", "H-moll", "Fis-moll", "Cis-moll", "Gis-moll", "Dis-moll", "Ais-moll", "D-moll", "G-moll", "C-moll", "F-moll" });
            scale.ItemsSource = scaleValues;
        }
    }
}
