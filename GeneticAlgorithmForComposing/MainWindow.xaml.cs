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

            

            //MUSIC
            string scaleValue;
            string measuresValue = measures.Text;

            //GENETIC
            int populationValue = int.Parse(populationSize.Text);
            string iterationsValue = generations.Text;
            string probabilityCrossoverValue = probCrossover.Text;
            string probabilityMutationValue = probMutation.Text;

            


            double sumaCzasu = 4.0;
            string[] chromosom = GeneticAlgorithm.GenerateChromosome(gamaCdur, sumaCzasu);
            kodowanyChromosom = GeneticAlgorithm.CodeChromosome(chromosom);
            string[] dekodowanyChromosom = GeneticAlgorithm.DecodeChromosome(kodowanyChromosom);

            string[][] populacja = GeneticAlgorithm.GeneratePopulation(populationValue, gamaCdur, sumaCzasu);
            List<double> ocena = GeneticAlgorithm.OcenaPopulacji(populacja);
            string[][] wysel = GeneticAlgorithm.RouletteWheelSelection(populacja, ocena);
            string[][] krzyzowanie = GeneticAlgorithm.Crossover(wysel, 755);

            string rozw = string.Join(" ", krzyzowanie[0]);
            show.Text = rozw.ToString();

        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            






            int rozmiarPopulacji = 40;
            int iteracje = 1000;
            int i = 0;
            int liczbaTaktow = 4;
            string metrum = "4/4";


            double sumaCzasu = 4.0;
            string allela = GeneticAlgorithm.GenerateGene(gamaCdur);
            string kodowanie = GeneticAlgorithm.CodeGene(allela);
            string dekodowanie = GeneticAlgorithm.DecodeGene(kodowanie);
            //Console.WriteLine("Allela " + allela);
            //Console.WriteLine("Zakodowana allela " + kodowanie);
            //Console.WriteLine("Dekodowana allela " + dekodowanie);

            string[] chromosom = GeneticAlgorithm.GenerateChromosome(gamaCdur, sumaCzasu);
            kodowanyChromosom = GeneticAlgorithm.CodeChromosome(chromosom);
            string[] dekodowanyChromosom = GeneticAlgorithm.DecodeChromosome(kodowanyChromosom);

            string[][] populacja = GeneticAlgorithm.GeneratePopulation(rozmiarPopulacji, gamaCdur, sumaCzasu);
            List<double> ocena = GeneticAlgorithm.OcenaPopulacji(populacja);
            string[][] wysel = GeneticAlgorithm.RouletteWheelSelection(populacja, ocena);
            string[][] krzyzowanie = GeneticAlgorithm.Crossover(wysel, 755);
            //Console.WriteLine(populacja[2][6]);
            /*
            for (int u = 0; u <2; u++)
            {
                System.Console.Write("Element({0}): ", u);

                for (int j = 0; j < wysel[u].Length; j++)
                {
                    System.Console.Write("{0}{1}", wysel[u][j], j == (wysel[u].Length - 1) ? "" : " ");
                }
                System.Console.WriteLine();
            }


            for (int w = 0; w < 2; w++)
            {
                System.Console.Write("Element({0}): ", w);
                for (int j = 0; j < krzyzowanie[w].Length; j++)
                {
                    System.Console.Write("{0}{1}", krzyzowanie[w][j], j == (krzyzowanie[w].Length - 1) ? "" : " ");
                }
                System.Console.WriteLine();
            }
            */


            for (int j = 0; j < kodowanyChromosom.Length; j++)
            {
                Console.Write(kodowanyChromosom[j] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("****************************************");
            for (int j = 0; j < dekodowanyChromosom.Length; j++)
            {
                Console.Write(dekodowanyChromosom[j] + " ");
            }

            /*
            while (i <= iteracje)
            {
                i++;
            }
            */

            Console.Read();


            


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
