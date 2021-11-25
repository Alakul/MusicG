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

        private void PlayButton(object sender, RoutedEventArgs e)
        {
            GeneticAlgorithm.Play(kodowanyChromosom);
        }

        private void Compose(object sender, RoutedEventArgs e)
        {
            //MUSIC PARAMETERS
            string scaleValue;
            double measuresValue = double.Parse(measures.Text);

            //GENETIC PARAMETERS
            int populationValue = int.Parse(populationSize.Text);
            int iterationsValue = int.Parse(generations.Text);
            double crossoverProbabilityValue = double.Parse(probCrossover.Text);
            double MutationProbabilityValue = double.Parse(probMutation.Text);

            //GENETIC ALGORITHM
            string[] chromosomeChoosen;
            string[][] population;

            //Algorithm loop
            int i = 0;
            while (i <= iterationsValue)
            {
                i++;
            }



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

            //Buttons
            play.IsEnabled = false;
            saveAsMIDI.IsEnabled = false;

            //ComboBox scale values
            List<Item> items = new List<Item>();
            items.Add(new Item() { Scale = "Cdur", Category = "Durowa" });
            items.Add(new Item() { Scale = "C#dur", Category = "Durowa" });
            items.Add(new Item() { Scale = "Ddur", Category = "Durowa" });
            items.Add(new Item() { Scale = "Amoll", Category = "Molowa" });
            items.Add(new Item() { Scale = "Cmoll", Category = "Molowa" });

            ListCollectionView listCollectionView = new ListCollectionView(items);
            listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            scale.ItemsSource = listCollectionView;
        }
    }
}
