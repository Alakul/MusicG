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
        public static double[] duration = new[] { 1.0, 0.75, 0.5, 0.375, 0.25, 0.1875, 0.125, 0.0625 };

        public static int semitones = Music.semitones;
        public static int[] octaveValues = Music.octaveValues;

        public static Random random = new Random();


        //GENES
        public static string GenerateGene(string[] scale)
        {
            int noteRandom = random.Next(0, scale.Length);
            string noteValue = scale[noteRandom];

            int oktawaRandom = random.Next(0, octaveValues.Length);
            int octaveValue = octaveValues[oktawaRandom];

            int durationRandom = random.Next(0, duration.Length);
            double durationValue = duration[durationRandom];

            string gene = noteValue + ";" + octaveValue.ToString() + ";" + durationValue.ToString();
            return gene;
        }

        public static string CodeGene(string gene, string[] semitonesSelected)
        {
            string[] geneValues = gene.Split(';');
            string noteValue = geneValues[0];
            int octaveValue = int.Parse(geneValues[1]);
            double durationValue = double.Parse(geneValues[2]);

            string noteCoding = CodeNote(noteValue, semitonesSelected);
            string octaveCoding = CodeOctave(octaveValue);
            string durationCoding = CodeDuration(durationValue);
            string geneCoded = noteCoding + octaveCoding + durationCoding;

            return geneCoded;
        }

        //Do zmiany "poltony" i "poltonyKrzyzyk"
        public static string CodeNote(string noteValue, string[] semitonesSelected)
        {
            string noteCoded = "";
            for (int i = 0; i < semitones; i++){
                if (noteValue == semitonesSelected[i]){
                    for (int j = 0; j < semitones; j++){
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

        public static string DecodeGene(string gene, string[] semitonesSelected)
        {
            string noteValue = gene.Substring(0, 12);
            string octaveValue = gene.Substring(12, 3);
            string durationValue = gene.Substring(15, duration.Length);

            string noteDecoding = DecodeNote(noteValue, semitonesSelected);
            string octaveDecoding = DecodeOctave(octaveValue);
            string durationDecoding = DecodeDuration(durationValue);

            string geneDecoded = noteDecoding + ";"+ octaveDecoding + ";" + durationDecoding;
            return geneDecoded;
        }

        public static string DecodeNote(string noteValue, string[] semitonesSelected)
        {
            int noteIndex = noteValue.IndexOf('1');
            string noteDecoded = semitonesSelected[noteIndex].ToString();
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

            string[] chromosome = geneList.ToArray();
            return chromosome;
        }

        public static string[] CodeChromosome(string[] chromosome, string[] semitonesSelected)
        {
            string[] chromosomeCoded = new string[chromosome.Length];
            for (int i = 0; i < chromosome.Length; i++){
                chromosomeCoded[i] = CodeGene(chromosome[i], semitonesSelected);
            }
            return chromosomeCoded;
        }

        public static string[] DecodeChromosome(string[] chromosomeCoded, string[] semitonesSelected)
        {
            string[] chromosomeDecoded = new string[chromosomeCoded.Length];
            for (int i = 0; i < chromosomeCoded.Length; i++){
                chromosomeDecoded[i] = DecodeGene(chromosomeCoded[i], semitonesSelected);
            }
            return chromosomeDecoded;
        }

        //GENETIC ALGORITHM
        public static string[][] GeneratePopulation(int populationSize, string[] scale, double measuresValue, string[] semitonesSelected)
        {
            string[][] population = new string[populationSize][];
            string[] chromosomeGenerated, chromosomeCoded;

            for (int i = 0; i < populationSize; i++){
                chromosomeGenerated = GenerateChromosome(scale, measuresValue);
                chromosomeCoded = CodeChromosome(chromosomeGenerated, semitonesSelected);
                population[i] = chromosomeCoded;
            }
            return population;
        }

        //EVALUATION
        public static List<double> FitnessFunction(string[][] population, string[] semitonesSelected, string[] scale)
        {
            List<double> evaluationList = new List<double>();

            for (int i = 0; i < population.Length; i++){
                evaluationList.Add(EvaluateChromosome(population[i], semitonesSelected, scale));
            }
            return evaluationList;
        }

        public static double EvaluateChromosome(string[] chromosome, string[] semitonesSelected, string[] scale)
        {
            string[] chromosomeDecoded = DecodeChromosome(chromosome, semitonesSelected);

            double evaluationScale = EvaluateScale(chromosomeDecoded, scale);
            double evaluationOctave = EvaluateOctave(chromosomeDecoded);
            double evaluationInterval;

            double evaluation = evaluationScale + evaluationOctave;
            return evaluation;
        }

        public static double EvaluateScale(string[] chromosomeDecoded, string[] scale)
        {
            //Czy nuta należy do skali
            //Stosunek liczby nut należących/nienależących do licbzy nut
            double evaluation = 0;
            double chromosomeLength = chromosomeDecoded.Length;

            for (int i = 0; i < chromosomeDecoded.Length; i++){
                string[] geneValues = chromosomeDecoded[i].Split(';');
                string noteValue = geneValues[0];

                if (scale.Contains(noteValue)){
                    evaluation++;
                }
            }
            double ratio = evaluation / chromosomeLength;
            return ratio;
        }

        public static double EvaluateOctave(string[] chromosomeDecoded)
        {
            double evaluation = 0;
            int distance = 0;
            double chromosomeLength = chromosomeDecoded.Length;

            for (int i = 0; i < chromosomeDecoded.Length - 1; i++)
            {
                string[] geneValues1 = chromosomeDecoded[i].Split(';');
                int oktaveValue1 = int.Parse(geneValues1[1]);

                string[] geneValues2 = chromosomeDecoded[i + 1].Split(';');
                int oktaveValue2 = int.Parse(geneValues2[1]);

                if (oktaveValue1 < oktaveValue2)
                    distance = oktaveValue2 - oktaveValue1;
                else if (oktaveValue1 > oktaveValue2)
                    distance = oktaveValue1 - oktaveValue2;
                else if (oktaveValue1 == oktaveValue2)
                    distance = 0;

                if (distance == 0 || distance == 1){
                    evaluation++;
                }
            }
            double ratio = evaluation / chromosomeLength;
            return ratio;
        }

        public static double EvaluateInterval(string[] chromosomeDecoded)
        {
            //Czy interwał jest dobry
            //Stosunek liczby dobrych/niedobrych interwałów do liczby interwałów
            double evaluation = 0;
            double chromosomeLength = chromosomeDecoded.Length;

            double ratio = evaluation / chromosomeLength;
            return ratio;
        }

        //SELECTION
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

        public static string CheckSum(double sum)
        {
            int size = duration.Length;
            for (int i = 0; i < (size - 1); i++){
                for (int j = (i + 1); j < size; j++){
                    if (duration[i] + duration[j] == sum){
                        return duration[i].ToString() + ";" + duration[j].ToString();
                    }
                }
            }
            return "";
        }

        //CROSSOVER
        public static string[][] Crossover(string[][] chromosomesSelected, int crossoverProbability, double sumaCzasu, string[] semitonesSelected)
        {
            string[][] chromosomesAfterCrossover = new string[chromosomesSelected.Length][];
            string[] parent1, parent2;
            string[] decodedParent1, decodedParent2;
            List<string> child1, child2;
            int size1, size2;

            int crossPoint1, crossPoint2;

            for (int i = 0; i < chromosomesSelected.Length; i = i + 2){
                if (random.Next(1, 1001) < crossoverProbability){
                    size1 = chromosomesSelected[i].Length;
                    size2 = chromosomesSelected[i + 1].Length;

                    parent1 = chromosomesSelected[i].ToArray();
                    parent2 = chromosomesSelected[i + 1].ToArray();
                    decodedParent1 = DecodeChromosome(parent1, semitonesSelected);
                    decodedParent2 = DecodeChromosome(parent2, semitonesSelected);
                    child1 = new List<string>();
                    child2 = new List<string>();

                    crossPoint1 = 0;
                    crossPoint2 = 0;

                    if (size1 <= size2){
                        crossPoint1 = random.Next(1, chromosomesSelected[i].Length - 1);

                        //Obliczanie sumy od początku do punktu przecięcia
                        double sum = 0;
                        for (int j = 0; j < crossPoint1; j++){
                            string gene = decodedParent1[j];
                            string[] geneValues = gene.Split(';');
                            double durationValue = double.Parse(geneValues[2]);
                            sum += durationValue;
                        }

                        //Obliczanie punktu przeciecia dla drugiego potomka
                        double sum2 = 0;
                        while (sum2 < sum){
                            if (crossPoint2 == size2){
                                break;
                            }
                            else {
                                string gene = decodedParent2[crossPoint2];
                                string[] geneValues = gene.Split(';');
                                double durationValue = double.Parse(geneValues[2]);
                                sum2 += durationValue;
                                crossPoint2++;
                            }
                        }
                    }
                    else if (size1 > size2){
                        crossPoint2 = random.Next(1, chromosomesSelected[i + 1].Length - 1);

                        double sum = 0;
                        for (int j = 0; j < crossPoint2; j++){
                            string gene = decodedParent2[j];
                            string[] geneValues = gene.Split(';');
                            double durationValue = double.Parse(geneValues[2]);
                            sum += durationValue;
                        }

                        double sum2 = 0;
                        while (sum2 < sum){
                            if (crossPoint1 == size1){
                                break;
                            }
                            else {
                                string gene = decodedParent1[crossPoint1];
                                string[] geneValues = gene.Split(';');
                                double durationValue = double.Parse(geneValues[2]);
                                sum2 += durationValue;
                                crossPoint1++;
                            }
                        }
                    }

                    //Zapisywanie genów do potomków
                    for (int j = 0; j < crossPoint1; j++){
                        child1.Add(parent1[j]);
                    }
                    for (int j = crossPoint2; j < parent2.Length; j++){
                        child1.Add(parent2[j]);
                    }

                    for (int j = 0; j < crossPoint2; j++){
                        child2.Add(parent2[j]);
                    }
                    for (int j = crossPoint1; j < parent1.Length; j++){
                        child2.Add(parent1[j]);
                    }

                    //Sprawdzenie
                    //string[] pot1 = SprawdzCzasTrwania(potomek1.ToArray(), sumaCzasu, punktPrzeciecia1, poltonyWybrane);
                    //string[] pot2 = SprawdzCzasTrwania(potomek2.ToArray(), sumaCzasu, punktPrzeciecia2, poltonyWybrane);

                    chromosomesAfterCrossover[i] = child1.ToArray();
                    chromosomesAfterCrossover[i + 1] = child2.ToArray();
                }
                else {
                    chromosomesAfterCrossover[i] = chromosomesSelected[i];
                    chromosomesAfterCrossover[i + 1] = chromosomesSelected[i + 1];
                }
            }
            return chromosomesAfterCrossover;
        }

        //MUTATION
        public static string[][] MutationSemitones(string[][] chromosomesSelected, int crossoverProbability)
        {
            string[] chromosome;
            string[][] chromosomesAfterMutation = new string[chromosomesSelected.Length][];

            for (int i = 0; i < chromosomesSelected.Length; i++){
                if (random.Next(1, 1001) < crossoverProbability){
                    chromosome = ChangeNote(chromosomesSelected[i]);
                    chromosomesAfterMutation[i] = chromosome.ToArray();
                }
                else {
                    chromosomesAfterMutation[i] = chromosomesSelected[i];
                }
            }
            return chromosomesAfterMutation;
        }

        public static string[] ChangeNote(string[] chromosome)
        {
            int probability = 500;
            string[] chromosomeChanged = chromosome.ToArray();
            
            int geneRadom = random.Next(0, chromosome.Length);
            string gene = chromosome[geneRadom];
            string noteValue = gene.Substring(0, 12);
            int index = noteValue.IndexOf('1');
            char[] noteArray = noteValue.ToCharArray();
            string noteCoded = "";

            if (random.Next(1, 1001) < probability){
                //pol tonu wyzej
                if (index == 11){
                    noteArray[11] = '0';
                    noteArray[0] = '1';
                }
                else {
                    noteArray[index] = '0';
                    noteArray[index + 1] = '1';
                }
            }
            else {
                if (index == 0){
                    noteArray[11] = '1';
                    noteArray[0] = '0';
                }
                else {
                    noteArray[index] = '0';
                    noteArray[index - 1] = '1';
                }
            }

            for (int i = 0; i < noteArray.Length; i++){
                noteCoded += noteArray[i];
            }

            string octaveValue = gene.Substring(12, 3);
            string durationValue = gene.Substring(15, duration.Length);
            string geneValue = noteCoded + octaveValue + durationValue;

            chromosomeChanged[geneRadom] = geneValue;
            return chromosomeChanged;
        }




        //MIDI
        public static List<MidiNote> GetNotesSequence(string[] chromosome, string[] semitonesSelected)
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
                geneDecoded = DecodeGene(chromosome[i], semitonesSelected);
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

        public static void Play(string[] chromosome, string[] semitonesSelected)
        {
            List<MidiNote> noteMap = GetNotesSequence(chromosome, semitonesSelected);

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
