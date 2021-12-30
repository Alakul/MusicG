using M;
using Manufaktura.Controls.Model;
using Manufaktura.Music.Model;
using Manufaktura.Music.Model.MajorAndMinor;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmForComposing
{
    class GeneticAlgorithm
    {
        public static int semitones = MusicData.semitones;
        public static int[] octaveValues = MusicData.octaveValues;
        public static double[] duration = MusicData.duration;
        public static double[] durationBase = MusicData.durationBase;

        public static Random random = new Random();

        //SET
        public static string[] SetSign(string[] scale)
        {
            string[] semitonesSelected = new string[12];
            for (int i = 0; i < scale.Length; i++){
                if (scale[i].Contains('#') == true){
                    semitonesSelected = MusicData.semitonesSharp;
                    break;
                }
                else if (scale[i].Contains('b') == true){
                    semitonesSelected = MusicData.semitonesFlat;
                    break;
                }
                else {
                    semitonesSelected = MusicData.semitonesSharp;
                }
            }
            return semitonesSelected;
        }

        //GENES
        public static string GenerateGene(string[] scale)
        {
            int noteRandom = random.Next(0, scale.Length);
            string noteValue = scale[noteRandom];

            int oktawaRandom = random.Next(0, octaveValues.Length);
            int octaveValue = octaveValues[oktawaRandom];

            int durationRandom = random.Next(0, durationBase.Length);
            double durationValue = durationBase[durationRandom];

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
        public static List<double> FitnessFunction(string[][] population, string[] semitonesSelected, string[] scale, int prefferedOctave)
        {
            List<double> evaluationList = new List<double>();

            for (int i = 0; i < population.Length; i++){
                evaluationList.Add(EvaluateChromosome(population[i], semitonesSelected, scale, prefferedOctave));
            }
            return evaluationList;
        }

        public static double EvaluateChromosome(string[] chromosome, string[] semitonesSelected, string[] scale, int prefferedOctave)
        {
            string[] chromosomeDecoded = DecodeChromosome(chromosome, semitonesSelected);

            double evaluationNote = EvaluateNote(chromosomeDecoded, scale);
            double evaluationDuration = EvaluateDuration(chromosomeDecoded);
            double evaluationOctave = EvaluateOctave(chromosomeDecoded, prefferedOctave);
            double evaluationInterval = EvaluateInterval(chromosomeDecoded, semitonesSelected);
            
            double evaluation = evaluationNote + evaluationDuration + evaluationOctave + evaluationInterval;
            return evaluation;
        }

        public static double EvaluateNote(string[] chromosomeDecoded, string[] scale)
        {
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

        public static double EvaluateDuration(string[] chromosomeDecoded)
        {
            //Czy nuta ma wartość z kropką
            //Stosunek liczby nut należących/nienależących do liczby nut
            double evaluation = 0;
            double chromosomeLength = chromosomeDecoded.Length;

            for (int i = 0; i < chromosomeDecoded.Length; i++){
                string[] geneValues = chromosomeDecoded[i].Split(';');
                string durationValue = geneValues[2];

                if (durationValue != "0.75" || durationValue != "0.375" || durationValue != "0.1875"){
                    evaluation++;
                }
            }
            double ratio = evaluation / chromosomeLength;
            return ratio;
        }

        public static double EvaluateOctave(string[] chromosomeDecoded, int prefferedOctave)
        {
            /*
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
            */

            double evaluation = 0;
            double chromosomeLength = chromosomeDecoded.Length;

            for (int i = 0; i < chromosomeDecoded.Length - 1; i++)
            {
                string[] geneValues = chromosomeDecoded[i].Split(';');
                int oktaveValue = int.Parse(geneValues[1]);

                if (oktaveValue == prefferedOctave){
                    evaluation++;
                }
            }
            double ratio = evaluation / chromosomeLength;
            return ratio;
        }

        public static double EvaluateInterval(string[] chromosomeDecoded, string[] semitonesSelected)
        {
            //Czy interwał jest dobry
            //Stosunek liczby dobrych/niedobrych interwałów do liczby interwałów
            double evaluation = 0;
            double chromosomeLength = chromosomeDecoded.Length;

            //do przedostatniego
            for (int i = 0; i < chromosomeDecoded.Length - 2; i++)
            {
                string[] geneValues1 = chromosomeDecoded[i].Split(';');
                string noteValue1 = geneValues1[0];
                int oktaveValue1 = int.Parse(geneValues1[1]);

                string[] geneValues2 = chromosomeDecoded[i + 1].Split(';');
                string noteValue2 = geneValues2[0];
                int oktaveValue2 = int.Parse(geneValues2[1]);

                int interval = 0;
                int indexNote1 = Array.IndexOf(semitonesSelected, noteValue1);
                int indexNote2 = Array.IndexOf(semitonesSelected, noteValue2);

                //Oblczenie interwału
                if (oktaveValue1 < oktaveValue2 && oktaveValue2-oktaveValue1 == 1){
                    interval = semitonesSelected.Length - 1 - indexNote1 + indexNote2 + 1;
                } 
                else if (oktaveValue1 > oktaveValue2 && oktaveValue1-oktaveValue2 == 1){
                    interval = semitonesSelected.Length - 1 - indexNote2 + indexNote1 + 1;
                }
                else if (oktaveValue1 == oktaveValue2){
                    if (indexNote1 > indexNote2){
                        interval = indexNote1 - indexNote2;
                    }
                    else if (indexNote1 < indexNote2){
                        interval = indexNote2 - indexNote1;
                    } 
                }
                else if ((oktaveValue1<oktaveValue2 && oktaveValue2-oktaveValue1 > 1) || (oktaveValue1>oktaveValue2 && oktaveValue1-oktaveValue2 > 1)){
                    interval = 13;
                }

                if (interval == 0){
                    evaluation += 0.25;
                }
                else if (interval == 1 || interval == 2 || interval == 3 || interval == 4){
                    evaluation += 1;
                }
                else if (interval == 5 || interval == 6 || interval == 7){
                    evaluation += 0.75;
                }
                else if (interval == 8 || interval == 9){
                    evaluation += 0.5;
                }
                else if (interval == 10 || interval == 11){
                    evaluation += 0.5;
                }
                else if (interval == 12){
                    evaluation += 0.2;
                }
                else if (interval > 12){
                    evaluation += 0;
                }

                /*
                //perfect consonants
                if (interval == 0 || interval == 5 || interval == 7 || interval == 12){
                    evaluation += 1;
                }
                //inferfect c
                else if (interval == 3 || interval == 4 || interval == 8 || interval == 9){
                    evaluation += 0.75;
                }
                //seconds
                else if (interval == 1 || interval == 2){
                    evaluation += 0.5;
                }
                //sevenths
                else if (interval == 10 || interval == 11){
                    evaluation += 0.25;
                }
                else if (interval > 12){
                    evaluation += 0;
                }
                */
            }

            double ratio = evaluation / (chromosomeLength - 1);
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
        public static string[][] Crossover(string[][] chromosomesSelected, int crossoverProbability, double measuresValue, string[] semitonesSelected)
        {
            string[][] chromosomesAfterCrossover = new string[chromosomesSelected.Length][];
            string[] parent1, parent2;
            string[] decodedParent1, decodedParent2;
            List<string> child1, child2;
            int size1, size2;

            int crossPoint1, crossPoint2;

            for (int i = 0; i < chromosomesSelected.Length; i = i + 2){
                if (random.Next(1, 1001) <= crossoverProbability){
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
                        crossPoint1 = random.Next(0, size1 - 1);

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
                        crossPoint2 = random.Next(0, size2 - 1);

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
                    string[] childRepair1 = CheckDuration(child1.ToArray(), measuresValue, crossPoint1, semitonesSelected);
                    string[] childRepair2 = CheckDuration(child2.ToArray(), measuresValue, crossPoint2, semitonesSelected);

                    chromosomesAfterCrossover[i] = childRepair1;
                    chromosomesAfterCrossover[i + 1] = childRepair2;
                }
                else {
                    chromosomesAfterCrossover[i] = chromosomesSelected[i];
                    chromosomesAfterCrossover[i + 1] = chromosomesSelected[i + 1];
                }
            }
            return chromosomesAfterCrossover;
        }

        public static string[] CheckDuration(string[] child, double measuresValue, int crossPoint, string[] semitonesSelected)
        { 
            string[] chromosomeDecoded = DecodeChromosome(child, semitonesSelected);
            string[] chromosomeRepaired;
            double sum = 0;

            for (int i = 0; i < chromosomeDecoded.Length; i++){
                string gene = chromosomeDecoded[i];
                string[] geneValues = gene.Split(';');
                double durationValue = double.Parse(geneValues[2]);
                sum += durationValue;
            }
            double difference = measuresValue - sum;

            List<string> chromosomeList = new List<string>(chromosomeDecoded);
            List<double> genesDuration = new List<double>();

            List<int> indexes;
            int index = 0;

            if (difference < 0){
                double differenceValue = Math.Abs(difference);
                indexes = CheckMeasure(chromosomeDecoded, crossPoint, differenceValue);

                while (differenceValue != 0){
                    for (int j = 0; j < indexes.Count; j++){
                        if (indexes[j] < crossPoint){
                            index = indexes[j];
                        }
                        else if (indexes[0] >= crossPoint){
                            index = indexes[0];
                        }
                    }

                    string gene = chromosomeDecoded[index];
                    string[] geneValues = gene.Split(';');
                    string noteValue = geneValues[0];
                    int octaveValue = int.Parse(geneValues[1]);
                    double durationValue = double.Parse(geneValues[2]);

                    if (durationValue == differenceValue){
                        chromosomeList.RemoveAt(index);
                        differenceValue = 0;
                    }
                    else if (durationValue > differenceValue){
                        double newValue = durationValue - differenceValue;
                        if (duration.Contains(newValue)){
                            string newGene = noteValue + ";" + octaveValue.ToString() + ";" + newValue.ToString();
                            if (index == chromosomeList.Count){
                                chromosomeList.Add(newGene);
                            }
                            else {
                                chromosomeList[index] = newGene;
                            } 
                            differenceValue = 0;
                        }
                        else {
                            string duration = CheckSum(newValue);
                            string[] durationValues = duration.Split(';');
                            double duration1 = double.Parse(durationValues[0]);
                            double duration2 = double.Parse(durationValues[1]);

                            chromosomeList.RemoveAt(index);
                            genesDuration.Add(duration1);
                            genesDuration.Add(duration2);

                            for (int i = 0; i < genesDuration.Count; i++){
                                string newGene = noteValue + ";" + octaveValue.ToString() + ";" + genesDuration[i].ToString();
                                chromosomeList.Insert(index + i, newGene);
                            }
                            differenceValue = 0;
                        }
                    }
                    else if (durationValue < differenceValue){
                        chromosomeList.RemoveAt(index);
                        indexes.RemoveAt(indexes.IndexOf(index));

                        differenceValue = differenceValue - durationValue;
                    }
                }
            }
            else if (difference > 0){
                if (duration.Contains(difference)){
                    genesDuration.Add(difference);
                }
                else {
                    string duration = CheckSum(difference);
                    string[] durationValues = duration.Split(';');
                    double duration1 = double.Parse(durationValues[0]);
                    double duration2 = double.Parse(durationValues[1]);

                    genesDuration.Add(duration1);
                    genesDuration.Add(duration2);
                }

                string gene = chromosomeDecoded[crossPoint - 1];
                string[] geneValues = gene.Split(';');
                string noteValue = geneValues[0];
                int octaveValue = int.Parse(geneValues[1]);

                for (int i = 0; i < genesDuration.Count; i++){
                    string newGene = noteValue + ";" + octaveValue.ToString() + ";" + genesDuration[i].ToString();
                    chromosomeList.Insert(crossPoint + i, newGene);
                }
            }

            chromosomeRepaired = chromosomeList.ToArray();
            chromosomeRepaired = CodeChromosome(chromosomeRepaired, semitonesSelected);

            return chromosomeRepaired;
        }

        public static List<int> CheckMeasure(string[] chromosomeDecoded, int crossPoint, double difference)
        {
            List<int> indexes = new List<int>();
            double sum = 0;
            double sum2 = 0;

            for (int i = 0; i < crossPoint; i++){
                string gene = chromosomeDecoded[i];
                string[] geneValues = gene.Split(';');
                double durationValue = double.Parse(geneValues[2]);

                sum += durationValue;
            }

            int measure = (int)Math.Ceiling(sum);

            for (int i = 0; i < chromosomeDecoded.Length; i++){
                string gene = chromosomeDecoded[i];
                string[] geneValues  = gene.Split(';');
                double durationValue = double.Parse(geneValues[2]);

                sum2 += durationValue;

                if (sum2 <= (measure + difference) && sum2 > measure - 1){
                    indexes.Add(i);
                }
            }
            return indexes;
        }


        //MUTATION
        public static string[][] MutationSemitones(string[][] chromosomesSelected, int mutationProbability)
        {
            string[][] chromosomesAfterMutation = new string[chromosomesSelected.Length][];

            for (int i = 0; i < chromosomesSelected.Length; i++){
                chromosomesAfterMutation[i] = ChangeNotes(chromosomesSelected[i], mutationProbability);
            }
            return chromosomesAfterMutation;
        }

        public static string[] ChangeNotes(string[] chromosome, int mutationProbability)
        {
            int probability = 500;
            string[] chromosomeChanged = chromosome.ToArray();

            for (int i = 0; i < chromosome.Length; i++){
                if (random.Next(1, 1001) <= mutationProbability){
                    string gene = chromosome[i];
                    string noteValue = gene.Substring(0, 12);
                    int index = noteValue.IndexOf('1');
                    char[] noteArray = noteValue.ToCharArray();
                    string noteCoded = "";

                    if (random.Next(1, 1001) < probability){
                        //pol tonu wyzej
                        if (index == 11){
                            noteArray[11] = '0';
                            noteArray[10] = '1';
                        }
                        else {
                            noteArray[index] = '0';
                            noteArray[index + 1] = '1';
                        }
                    }
                    else {
                        if (index == 0){
                            noteArray[1] = '1';
                            noteArray[0] = '0';
                        }
                        else {
                            noteArray[index] = '0';
                            noteArray[index - 1] = '1';
                        }
                    }

                    for (int j = 0; j < noteArray.Length; j++){
                        noteCoded += noteArray[j];
                    }

                    string octaveValue = gene.Substring(12, 3);
                    string durationValue = gene.Substring(15, duration.Length);
                    string geneValue = noteCoded + octaveValue + durationValue;

                    chromosomeChanged[i] = geneValue;
                }
            }

            return chromosomeChanged;
        }

        public static string[][] MutationOctave(string[][] chromosomesSelected, int mutationProbability)
        {
            string[][] chromosomesAfterMutation = new string[chromosomesSelected.Length][];

            for (int i = 0; i < chromosomesSelected.Length; i++){
                chromosomesAfterMutation[i] = ChangeOctave(chromosomesSelected[i], mutationProbability);
            }
            return chromosomesAfterMutation;
        }

        public static string[] ChangeOctave(string[] chromosome, int mutationProbability)
        {
            string[] chromosomeChanged = chromosome.ToArray();

            for (int i = 0; i < chromosome.Length; i++){
                if (random.Next(1, 1001) <= mutationProbability){
                    string gene = chromosome[i];
                    string octaveValue = gene.Substring(12, 3);

                    int index;
                    char[] octaveArray;
                    string octaveCoded;
                    int counter = 0;

                    do {
                        octaveCoded = "";
                        octaveArray = octaveValue.ToCharArray();
                        index = random.Next(0, octaveValue.Length);

                        if (octaveArray[index] == '0'){
                            octaveArray[index] = '1';
                        }
                        else if (octaveArray[index] == '1'){
                            octaveArray[index] = '0';
                        }

                        for (int j = 0; j < octaveArray.Length; j++){
                            octaveCoded += octaveArray[j];
                        }
                        counter++;

                    } while (octaveValues.Contains(Convert.ToInt32(octaveCoded, 2)) == false && counter != 3);

                    if (counter == 3){
                        octaveCoded = octaveValue;
                    }

                    string noteValue = gene.Substring(0, 12);
                    string durationValue = gene.Substring(15, duration.Length);
                    string geneValue = noteValue + octaveCoded + durationValue;

                    chromosomeChanged[i] = geneValue;
                }
            }
            return chromosomeChanged;
        }

    }
}
