using M;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmForComposing
{
    class GeneticAlgorithm
    {
        public static string[] gamaCdur = new[] { "C", "D", "E", "F", "G", "A", "B", "C" };
        public static int[] oktawa = new[] { 1, 2, 3, 4, 5, 6, 7 };
        public static double[] czasTrwaniaFull = new[] { 1.0, 0.75, 0.5, 0.375, 0.25, 0.1875, 0.125, 0.09375, 0.0625 };
        public static double[] duration = new[] { 1.0, 0.5, 0.25, 0.125, 0.0625 };

        public static string[] poltonyKrzyzyk = new[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        public static string[] poltonyBemol = new[] { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };
        public static int poltony = 12;

        public static Random random = new Random();



        //GENES
        public static string GenerateGene(string[] scale)
        {
            int noteRandom = random.Next(0, scale.Length);
            string noteValue = scale[noteRandom];

            int oktawaRandom = random.Next(0, oktawa.Length);
            int octaveValue = oktawa[oktawaRandom];

            int durationRandom = random.Next(0, duration.Length);
            double durationValue = duration[durationRandom];

            string gene = noteValue + ";" + octaveValue.ToString() + ";" + durationValue.ToString();
            return gene;
        }

        public static string CodeGene(string gene)
        {
            string[] geneValues = gene.Split(';');
            string noteValue = geneValues[0];
            int octaveValue = int.Parse(geneValues[1]);
            double durationValue = double.Parse(geneValues[2]);

            string noteCoding = CodeNote(noteValue);
            string octaveCoding = CodeOctave(octaveValue);
            string durationCoding = CodeDuration(durationValue);
            string geneCoded = noteCoding + octaveCoding + durationCoding;

            return geneCoded;
        }

        //Do zmiany "poltony" i "poltonyKrzyzyk"
        public static string CodeNote(string noteValue)
        {
            string noteCoded = "";
            for (int i = 0; i < poltony; i++){
                if (noteValue == poltonyKrzyzyk[i]){
                    for (int j = 0; j < poltony; j++){
                        if (j == i){
                            noteCoded += "1";
                        }
                        else{
                            noteCoded += "0";
                        }
                    }
                }
            }
            return noteCoded;
        }

        public static string CodeOctave(int octaveValue)
        {
            string octaveCoded = Convert.ToString(octaveValue, 2).PadLeft(3, '0');
            return octaveCoded;
        }

        public static string CodeDuration(double durationValue)
        {
            string durationCoded = "";
            for (int i = 0; i < duration.Length; i++){
                if (durationValue == duration[i]){
                    for (int j = 0; j < duration.Length; j++){
                        if (j == i){
                            durationCoded += "1";
                        }
                        else{
                            durationCoded += "0";
                        }
                    }
                }
            }
            return durationCoded;
        }

        public static string DecodeGene(string gene)
        {
            string noteValue = gene.Substring(0, 12);
            string octaveValue = gene.Substring(12, 3);
            string durationValue = gene.Substring(15, duration.Length);

            string noteDecoding = DecodeNote(noteValue);
            string octaveDecoding = DecodeOctave(octaveValue);
            string durationDecoding = DecodeDuration(durationValue);

            string geneDecoded = noteDecoding + ";"+ octaveDecoding + ";" + durationDecoding;
            return geneDecoded;
        }

        public static string DecodeNote(string noteValue)
        {
            int noteIndex = noteValue.IndexOf('1');
            string noteDecoded = poltonyKrzyzyk[noteIndex].ToString();
            return noteDecoded;
        }

        public static string DecodeOctave(string octaveValue)
        {
            int octaveDecoded = Convert.ToInt32(octaveValue, 2);
            return octaveDecoded.ToString();
        }

        public static string DecodeDuration(string durationValue)
        {
            int durationIndex = durationValue.IndexOf('1');
            string durationDecoded = duration[durationIndex].ToString();
            return durationDecoded;
        }

        //CHROMOSOME
        //Zmienic "sumaCzasu"
        public static string[] GenerateChromosome(string[] scale, double sumaCzasu)
        {
            List<string> geneList = new List<string>();
            double sum = 0;
            double durationValue;
            string[] geneValues;

            for (int i = 1; i <= sumaCzasu; i++){
                while (sum != i){
                    string geneGenerated = GenerateGene(scale);
                    geneValues = geneGenerated.Split(';');
                    durationValue = double.Parse(geneValues[2]);

                    if (sum + durationValue <= i){
                        geneList.Add(geneGenerated);
                        sum += durationValue;
                    }
                }
            }

            string[] chromosome = new string[geneList.Count];
            for (int j = 0; j < geneList.Count; j++){
                chromosome[j] = geneList[j];
            }

            return chromosome;
        }

        public static string[] CodeChromosome(string[] chromosome)
        {
            string[] chromosomeCoded = new string[chromosome.Length];
            for (int i = 0; i < chromosome.Length; i++){
                chromosomeCoded[i] = CodeGene(chromosome[i]);
            }
            return chromosomeCoded;
        }

        public static string[] DecodeChromosome(string[] chromosomeCoded)
        {
            string[] chromosomeDecoded = new string[chromosomeCoded.Length];
            for (int i = 0; i < chromosomeCoded.Length; i++){
                chromosomeDecoded[i] = DecodeGene(chromosomeCoded[i]);
            }
            return chromosomeDecoded;
        }

        //GENETIC ALGORITHM
        public static string[][] GeneratePopulation(int populationSize, string[] scale, double sumaCzasu)
        {
            string[][] population = new string[populationSize][];
            string[] chromosomeGenerated, chromosomeCoded;

            for (int i = 0; i < populationSize; i++){
                chromosomeGenerated = GenerateChromosome(scale, sumaCzasu);
                chromosomeCoded = CodeChromosome(chromosomeGenerated);
                population[i] = chromosomeCoded;
            }
            return population;
        }

        public static void FitnessFunction()
        {

        }

        public static double RandomNumber(double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static string[][] RouletteWheelSelection(string[][] population, List<double> fitnessList)
        {
            //Obliczanie sumy ocen
            double fitnessSum = 0;
            for (int i = 0; i < fitnessList.Count; i++){
                fitnessSum += fitnessList[i];
            }

            //Tworzenie przedziałów
            double[] intervals = new double[fitnessList.Count];
            double number = 0;
            for (int i = 0; i < intervals.Length - 1; i++){
                intervals[i] = number;
                number += fitnessList[i];
            }
            intervals[intervals.Length - 1] = number;

            //Selekcja
            string[][] chromosomesSelected = new string[population.Length][];
            double drawnNumber;
            int counter = 0;
            int loopCounter = 0;

            while (loopCounter < fitnessList.Count){
                drawnNumber = RandomNumber(0, fitnessSum);

                for (int i = 0; i < intervals.Length; i++){
                    if (i == intervals.Length - 1){
                        if (drawnNumber >= intervals[i]){
                            chromosomesSelected[counter] = population[i];
                            counter++;
                        }
                    }
                    else{
                        if (drawnNumber >= intervals[i] && drawnNumber < intervals[i + 1]){
                            chromosomesSelected[counter] = population[i];
                            counter++;
                        }
                    }
                }
                loopCounter++;
            }
            return chromosomesSelected;
        }

        public static string[][] TournamentSelection(string[][] population, List<double> fitnessList, int tournamentSize)
        {
            List<double> tmpFitnessList = new List<double>();
            List<int> tmpIndexList = new List<int>();

            int loopCounter = 0;
            int drawnNumber;
            int bestChromosomeIndex;
            double bestChromosomeFitness;
            string[][] chromosomesSelected = new string[population.Length][];

            while (loopCounter < population.Length){
                for (int i = 0; i < tournamentSize; i++){
                    drawnNumber = random.Next(0, population.Length);
                    tmpFitnessList.Add(fitnessList[drawnNumber]);
                    tmpIndexList.Add(drawnNumber);
                }

                //Im większa ocena tym lepszy osobnik
                bestChromosomeFitness = tmpFitnessList.Max<double>();
                bestChromosomeIndex = tmpFitnessList.IndexOf(bestChromosomeFitness);
                chromosomesSelected[loopCounter] = population[tmpIndexList[bestChromosomeIndex]];

                tmpFitnessList.Clear();
                tmpIndexList.Clear();
                loopCounter++;
            }
            return chromosomesSelected;
        }

        public static string[][] Crossover(string[][] chromosomesSelected, int crossoverProbability)
        {
            int crossPoint = 0;
            int value1, value2;
            string[] child1, child2, parent1, parent2;

            string[][] chromosomesAfterCrossover = new string[chromosomesSelected.Length][];

            for (int i = 0; i < chromosomesSelected.Length; i = i + 2){
                if (random.Next(1, 1001) < crossoverProbability){
                    parent1 = chromosomesSelected[i];
                    parent2 = chromosomesSelected[i + 1];
                    child1 = parent1.ToArray();
                    child2 = parent2.ToArray();

                    value1 = chromosomesSelected[i].Length;
                    value2 = chromosomesSelected[i + 1].Length;

                    if (value1 <= value2){
                        crossPoint = random.Next(0, chromosomesSelected[i].Length - 1);
                    }
                    else if (value1 > value2){
                        crossPoint = random.Next(0, chromosomesSelected[i + 1].Length - 1);
                    }

                    for (int k = 0; k < crossPoint; k++){
                        child1[k] = parent2[k];
                        child2[k] = parent1[k];
                    }

                    chromosomesAfterCrossover[i] = child1;
                    chromosomesAfterCrossover[i + 1] = child2;
                }
                else {
                    chromosomesAfterCrossover[i] = chromosomesSelected[i];
                    chromosomesAfterCrossover[i + 1] = chromosomesSelected[i + 1];
                }
            }
            return chromosomesAfterCrossover;
        }

        public static void Mutation()
        {

        }






        public static double OcenaNuty(string nutaPierwsza, string nutaDruga)
        {
            //Np. preferowana nuta lub odległości między nutami

            //DekodowanieAlleli
            string dekodowanaPierwsza = DecodeGene(nutaPierwsza);
            string dekodowanaDruga = DecodeGene(nutaDruga);

            string[] wartosciPierwsza = dekodowanaPierwsza.Split(';');
            string nutaP = wartosciPierwsza[0];
            int oktawaP = int.Parse(wartosciPierwsza[1]);
            double czasTrwaniaP = double.Parse(wartosciPierwsza[2]);

            string[] wartosciDruga = dekodowanaDruga.Split(';');
            string nutaD = wartosciDruga[0];
            int oktawaD = int.Parse(wartosciDruga[1]);
            double czasTrwaniaD = double.Parse(wartosciDruga[2]);

            //Obliczanie odległości w interwale
            ///

            double odleglosc = OdlegloscOktawa(oktawaP, oktawaD);
            return odleglosc;
        }

        public static double OdlegloscOktawa(int oktawaP, int oktawaD)
        {
            int odleglosc = 0;
            double waga = 0;

            if (oktawaP < oktawaD)
                odleglosc = oktawaD - oktawaP;
            else if (oktawaP > oktawaD)
                odleglosc = oktawaP - oktawaD;
            else if (oktawaP == oktawaD)
                odleglosc = 0;

            if (odleglosc == 0)
                waga = 1.0;
            else if (odleglosc == 1)
                waga = 0.7;
            else if (odleglosc > 1)
                waga = 0.3;

            return waga;
        }

        public static List<double> OcenaPopulacji(string[][] populacja)
        {
            List<double> listaOcen = new List<double>();
            double ocena;

            for (int i = 0; i < populacja.Length; i++)
            {
                ocena = 0;

                //Przejście przez wszystkie nuty aż do przedostatniej
                for (int j = 0; j < populacja[i].Length - 1; j++)
                {
                    ocena += OcenaNuty(populacja[i][j], populacja[i][j + 1]);
                }
                listaOcen.Add(ocena);
            }
            return listaOcen;
        }








        //MIDI
        public static List<MidiNote> GetNotesSequence(string[] chromosome)
        {
            string geneDecoded;
            string[] geneValues;
            string noteValue;
            string octaveValue;
            double durationValue;
            int counter = 0;
            int value = 60;

            //Tworzenie sekwencji nut
            var noteMap = new List<MidiNote>();

            for (int i = 0; i < chromosome.Length; i++)
            {
                geneDecoded = DecodeGene(chromosome[i]);
                geneValues = geneDecoded.Split(';');
                noteValue = geneValues[0];
                octaveValue = geneValues[1];
                durationValue = double.Parse(geneValues[2]);

                //Do poprawy
                if (durationValue == 0.25){
                    value = 30;
                }
                else {
                    value = 60;
                }

                noteMap.Add(new MidiNote(counter, 0, noteValue + octaveValue, 127, 240));
                counter += value;
            }
            return noteMap;
        }

        public static void Play(string[] chromosome)
        {
            List<MidiNote> noteMap = GetNotesSequence(chromosome);

            using (var stream = MidiDevice.Streams[0])
            {
                stream.Open();
                var sequence = MidiSequence.FromNoteMap(noteMap);
                stream.Start();
                Console.Error.WriteLine("Press any key to exit...");
                stream.Send(sequence.Events);
                Console.ReadKey();
            }
        }
    }
}
