using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicG
{
    public class GeneticAlgorithm
    {
        public List<Chromosome> Population { get; set; }
        public List<double> Fitness { get; set; }
        public double[,] EvaluationArray { get; set; }
        public int Generations { get; set; }
        

        public string[] Scale { get; set; }
        public string[] SemitonesSelected { get; set; }
        private double MeasuresValue { get; set; }
        private int PopulationSize { get; set; }

        public int PrefferedOctave { get; set; }
        public int CrossoverProbability { get; set; }
        public int MutationProbability { get; set; }

        private double[] criteriaWeight;
        private double[] intervalWeight;

        private Random random = new Random();
        private double[] duration = MusicData.duration;
        private int[] octaveValues = MusicData.octaveValues;

        public GeneticAlgorithm(string[] scale, double measuresValue, int prefferedOctave, int populationSize,
            int crossoverProbabilityValue, int mutationProbabilityValue, int generations,
            double[] criteriaWeight, double[] intervalWeight, int tournamentSize, int selectedSelection, int selectedMutation)
        {
            Scale = scale;
            SemitonesSelected = SetSign();
            PopulationSize = populationSize;
            MeasuresValue = measuresValue;
            NewGeneration(generations, prefferedOctave, crossoverProbabilityValue, mutationProbabilityValue,
                criteriaWeight, intervalWeight, tournamentSize, selectedSelection, selectedMutation);
        }

        private string[] SetSign()
        {
            string[] semitonesSelected = new string[12];
            for (int i = 0; i < Scale.Length; i++){
                if (Scale[i].Contains('#') == true){
                    semitonesSelected = MusicData.semitonesSharp;
                    break;
                }
                else if (Scale[i].Contains('b') == true){
                    semitonesSelected = MusicData.semitonesFlat;
                    break;
                }
                else {
                    semitonesSelected = MusicData.semitonesSharp;
                }
            }
            return semitonesSelected;
        }

        public void NewGeneration(int generations, int prefferedOctave, int crossoverProbabilityValue, int mutationProbabilityValue,
            double[] criteriaWeight, double[] intervalWeight, int tournamentSize, int selectedSelection,int selectedMutation)
        {
            SetProperty(prefferedOctave, crossoverProbabilityValue, mutationProbabilityValue,
            criteriaWeight, intervalWeight);

            if (Population == null){
                GeneratePopulation();
            }

            int counter = 1;
            while (counter <= generations){
                if (selectedSelection == 0){
                    TournamentSelection(tournamentSize);
                }
                else {
                    RouletteWheelSelection();
                }

                Crossover();
                
                if (selectedMutation == 0){
                    MutationSemitones();
                }
                else {
                    MutationOctave();
                }

                FitnessFunction();
                counter++;
            }
            Generations += generations;
        }

        private void SetProperty(int prefferedOctave, int crossoverProbabilityValue, int mutationProbabilityValue,
            double[] criteriaWeight, double[] intervalWeight)
        {
            PrefferedOctave = prefferedOctave;
            CrossoverProbability = crossoverProbabilityValue;
            MutationProbability = mutationProbabilityValue;
            this.criteriaWeight = criteriaWeight;
            this.intervalWeight = intervalWeight;
        }

        private void GeneratePopulation()
        {
            Population = new List<Chromosome>();
            for (int i = 0; i < PopulationSize; i++){
                Chromosome chromosome = new Chromosome(Scale, SemitonesSelected, MeasuresValue, PrefferedOctave, criteriaWeight, intervalWeight, random);
                Population.Add(chromosome);
            }
            FitnessFunction();
        }

        private void FitnessFunction()
        {
            Fitness = new List<double>();
            EvaluationArray = new double[Population.Count, 4];

            for (int i = 0; i < Population.Count; i++){
                Chromosome chromosome = Population[i];
                double[] evaluationValues = chromosome.EvaluateChromosome(Population[i]);
                Fitness.Add(evaluationValues[4]);
                EvaluationArray[i, 0] = evaluationValues[0];
                EvaluationArray[i, 1] = evaluationValues[1];
                EvaluationArray[i, 2] = evaluationValues[2];
                EvaluationArray[i, 3] = evaluationValues[3];
            }
        }
        public void RouletteWheelSelection()
        {
            double fitnessSum = 0;
            for (int i = 0; i < Fitness.Count; i++){
                fitnessSum += Fitness[i];
            }

            double[] intervals = new double[Fitness.Count];
            double number = 0;
            for (int i = 0; i < intervals.Length - 1; i++){
                intervals[i] = number;
                number += Fitness[i];
            }
            intervals[intervals.Length - 1] = number;

            List<Chromosome> chromosomesSelected = new List<Chromosome>();
            double drawnNumber;
            int loopCounter = 0;

            while (loopCounter < Fitness.Count){
                drawnNumber = RandomNumber(0, fitnessSum);

                for (int i = 0; i < intervals.Length; i++){
                    if (i == intervals.Length - 1){
                        if (drawnNumber >= intervals[i]){
                            chromosomesSelected.Add(Population[i]);
                        }
                    }
                    else {
                        if (drawnNumber >= intervals[i] && drawnNumber < intervals[i + 1]){
                            chromosomesSelected.Add(Population[i]);
                        }
                    }
                }
                loopCounter++;
            }
            Population = chromosomesSelected;
        }

        public void TournamentSelection(int tournamentSize)
        {
            List<double> tmpFitnessList = new List<double>();
            List<int> tmpIndexList = new List<int>();

            int loopCounter = 0;
            int drawnNumber;
            int bestChromosomeIndex;
            double bestChromosomeFitness;
            int populationSize = Population.Count;
            List<Chromosome> chromosomesSelected = new List<Chromosome> ();
            
            while (loopCounter < populationSize){
                for (int i = 0; i < tournamentSize; i++){
                    drawnNumber = random.Next(0, populationSize);
                    tmpFitnessList.Add(Fitness[drawnNumber]);
                    tmpIndexList.Add(drawnNumber);
                }

                bestChromosomeFitness = tmpFitnessList.Max<double>();
                bestChromosomeIndex = tmpFitnessList.IndexOf(bestChromosomeFitness);
                chromosomesSelected.Add(Population[tmpIndexList[bestChromosomeIndex]]);

                tmpFitnessList.Clear();
                tmpIndexList.Clear();
                loopCounter++;
            }
            Population = chromosomesSelected;
        }
        private double RandomNumber(double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
        public void Crossover()
        {
            List<Chromosome> chromosomesAfterCrossover = new List<Chromosome>();
            List<Gene> child1, child2;
            int crossPoint1, crossPoint2;

            for (int i = 0; i < Population.Count; i += 2){
                Chromosome parent1 = Population[i];
                Chromosome parent2 = Population[i + 1];

                if (random.Next(1, 1001) <= CrossoverProbability){
                    (crossPoint1, crossPoint2) = GetCrossPoints(parent1, parent2);
                    (child1, child2) = GetChildChromosomes(parent1, parent2, crossPoint1, crossPoint2);

                    Chromosome chromosome1 = new Chromosome(Scale, SemitonesSelected, MeasuresValue, PrefferedOctave, criteriaWeight, intervalWeight, random){
                        Genes = CheckDuration(child1, crossPoint1)
                    };
                    chromosomesAfterCrossover.Add(chromosome1);

                    Chromosome chromosome2 = new Chromosome(Scale, SemitonesSelected, MeasuresValue, PrefferedOctave, criteriaWeight, intervalWeight, random){
                        Genes = CheckDuration(child2, crossPoint2)
                    };
                    chromosomesAfterCrossover.Add(chromosome2);
                }
                else {
                    chromosomesAfterCrossover.Add(parent1);
                    chromosomesAfterCrossover.Add(parent2);
                }
            }
            Population = chromosomesAfterCrossover;
        }

        private (int, int) GetCrossPoints(Chromosome parent1, Chromosome parent2)
        {
            int chromosomeLength1 = parent1.Genes.Count;
            int chromosomeLength2 = parent2.Genes.Count;
            int crossPoint1 = 0;
            int crossPoint2 = 0;

            if (chromosomeLength1 <= chromosomeLength2){
                (crossPoint1, crossPoint2) = SetCrossPoints(parent1, parent2);
            }
            else if (chromosomeLength1 > chromosomeLength2){
                (crossPoint2, crossPoint1) = SetCrossPoints(parent2, parent1);
            }
            return (crossPoint1, crossPoint2);
        }

        private (List<Gene>, List<Gene>) GetChildChromosomes(Chromosome parent1, Chromosome parent2, int crossPoint1, int crossPoint2)
        {
            List<Gene> child1 = new List<Gene>();
            List<Gene> child2 = new List<Gene>();

            for (int j = 0; j < crossPoint1; j++){
                child1.Add(parent1.Genes[j]);
            }
            for (int j = crossPoint2; j < parent2.Genes.Count; j++){
                child1.Add(parent2.Genes[j]);
            }

            for (int j = 0; j < crossPoint2; j++){
                child2.Add(parent2.Genes[j]);
            }
            for (int j = crossPoint1; j < parent1.Genes.Count; j++){
                child2.Add(parent1.Genes[j]);
            }

            return (child1, child2);
        }

        private (int, int) SetCrossPoints(Chromosome parent1, Chromosome parent2)
        {
            int chromosomeLength1 = parent1.Genes.Count;
            int chromosomeLength2 = parent2.Genes.Count;
            int crossPoint1 = random.Next(0, chromosomeLength1 - 1);
            int crossPoint2 = 0;

            double sum1 = 0;
            for (int j = 0; j < crossPoint1; j++){
                double durationValue = parent1.Genes[j].GeneDuration;
                sum1 += durationValue;
            }

            double sum2 = 0;
            while (sum2 < sum1){
                if (crossPoint2 == chromosomeLength2){
                    break;
                }
                else {
                    double durationValue = parent2.Genes[crossPoint2].GeneDuration;
                    sum2 += durationValue;
                    crossPoint2++;
                }
            }
            return (crossPoint1, crossPoint2);
        }


        private string CheckSum(double sum)
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

        private List<Gene> CheckDuration(List<Gene> child, int crossPoint)
        {
            List<Gene> chromosomeDecoded = child.ToList();
            List<Gene> chromosomeRepaired;
            double sum = 0;
            
            for (int i = 0; i < chromosomeDecoded.Count; i++){
                double durationValue = chromosomeDecoded[i].GeneDuration;
                sum += durationValue;
            }
            double difference = MeasuresValue - sum;

            List<Gene> chromosomeList = new List<Gene>(chromosomeDecoded);
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

                    string noteValue = chromosomeDecoded[index].GeneNote;
                    int octaveValue = chromosomeDecoded[index].GeneOctave;
                    double durationValue = chromosomeDecoded[index].GeneDuration;

                    if (durationValue == differenceValue){
                        chromosomeList.RemoveAt(index);
                        differenceValue = 0;
                    }
                    else if (durationValue > differenceValue){
                        double newValue = durationValue - differenceValue;
                        if (duration.Contains(newValue)){

                            Gene newGene = new Gene(SemitonesSelected, noteValue, octaveValue, newValue);
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
                                Gene newGene = new Gene(SemitonesSelected, noteValue, octaveValue, genesDuration[i]);
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

                string noteValue = chromosomeDecoded[crossPoint - 1].GeneNote;
                int octaveValue = chromosomeDecoded[crossPoint - 1].GeneOctave;

                for (int i = 0; i < genesDuration.Count; i++){

                    Gene newGene = new Gene(SemitonesSelected, noteValue, octaveValue, genesDuration[i]);
                    chromosomeList.Insert(crossPoint + i, newGene);
                }
            }

            chromosomeRepaired = chromosomeList;
            return chromosomeRepaired;
        }

        private static List<int> CheckMeasure(List<Gene> chromosomeDecoded, int crossPoint, double difference)
        {
            List<int> indexes = new List<int>();
            double sum = 0;
            double sum2 = 0;

            for (int i = 0; i < crossPoint; i++){
                double durationValue = chromosomeDecoded[i].GeneDuration;
                sum += durationValue;
            }

            int measure = (int)Math.Ceiling(sum);

            for (int i = 0; i < chromosomeDecoded.Count; i++){
                double durationValue = chromosomeDecoded[i].GeneDuration;
                sum2 += durationValue;

                if (sum2 <= (measure + difference) && sum2 > measure - 1){
                    indexes.Add(i);
                }
            }
            return indexes;
        }

        public void MutationSemitones()
        {
            List<Chromosome> chromosomesAfterMutation = new List<Chromosome>();
            for (int i = 0; i < Population.Count; i++){
                chromosomesAfterMutation.Add(ChangeNotes(Population[i]));
            }
            Population = chromosomesAfterMutation;
        }

        private Chromosome ChangeNotes(Chromosome chromosome)
        {
            List<Gene> chromosomeChanged = chromosome.Genes;
            Gene gene;
            string noteCoded;

            for (int i = 0; i < chromosome.Genes.Count; i++){
                if (random.Next(1, 1001) <= MutationProbability){    
                    (gene, noteCoded) = ChangeGeneNote(chromosome.Genes[i]);
                    gene.GeneNoteCoded = noteCoded;
                    gene.DecodeGene();
                    chromosomeChanged[i] = gene;
                }
            }
            chromosome.Genes = chromosomeChanged;
            return chromosome;
        }

        private (Gene, string) ChangeGeneNote(Gene gene)
        {
            int probability = 500;
            string noteValue = gene.GeneNoteCoded;
            int index = noteValue.IndexOf('1');
            char[] noteArray = noteValue.ToCharArray();

            if (index == 11){
                noteArray[11] = '0';
                noteArray[10] = '1';
            }
            else if (index == 0){
                noteArray[1] = '1';
                noteArray[0] = '0';
            }
            else {
                if (random.Next(1, 1001) < probability){
                    noteArray[index] = '0';
                    noteArray[index + 1] = '1';
                }
                else {
                    noteArray[index] = '0';
                    noteArray[index - 1] = '1';
                }
            }

            string noteCoded = SaveArrayToString(noteArray);
            return (gene, noteCoded);
        }

        private string SaveArrayToString(char[] array)
        {
            string codedValue = "";
            for (int j = 0; j < array.Length; j++){
                codedValue += array[j];
            }
            return codedValue;
        }

        public void MutationOctave()
        {
            List<Chromosome> chromosomesAfterMutation = new List<Chromosome>();
            for (int i = 0; i < Population.Count; i++){
                chromosomesAfterMutation.Add(ChangeOctave(Population[i]));
            }
            Population = chromosomesAfterMutation;
        }

        private Chromosome ChangeOctave(Chromosome chromosome)
        {
            List<Gene> chromosomeChanged = chromosome.Genes.ToList();

            for (int i = 0; i < chromosome.Genes.Count; i++){
                if (random.Next(1, 1001) <= MutationProbability)
                {
                    Gene gene = chromosome.Genes[i];
                    string octaveValue = gene.GeneOctaveCoded;
                    char[] octaveArray;
                    string octaveCoded = "";

                    for (int j = 0; j < 3; j++){
                        octaveArray = octaveValue.ToCharArray();
                        if (octaveArray[j] == '0'){
                            octaveArray[j] = '1';
                        }
                        else if (octaveArray[j] == '1'){
                            octaveArray[j] = '0';
                        }

                        octaveCoded = SaveArrayToString(octaveArray);
                        if (octaveValues.Contains(Convert.ToInt32(octaveCoded, 2)) == true){
                            break;
                        }

                    }

                    if (octaveValues.Contains(Convert.ToInt32(octaveCoded, 2)) == false){
                        octaveCoded = octaveValue;
                    }

                    gene.GeneOctaveCoded = octaveCoded;
                    gene.DecodeGene();
                    chromosomeChanged[i] = gene;
                }
            }

            chromosome.Genes = chromosomeChanged;
            return chromosome;
        }

        public Chromosome GetTheBestChromosome()
        {
            double evaluation = GetTheBestFitness();
            Chromosome chromosome = Population[Fitness.IndexOf(evaluation)];
            return chromosome;
        }

        public double GetTheBestFitness()
        {
            double max = Fitness.Max();
            return max;
        }
    }
}
