using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicG
{
    public class Chromosome
    {
        public List<Gene> Genes { get; set; }
        public double Fitness { get; private set; }

        private Random random;
        private readonly string[] scale;
        private readonly string[] semitonesSelected;
        private readonly int prefferedOctave;
        private readonly double[] criteriaWeight;
        private readonly double[] intervalWeight;

        public Chromosome(string[] scale, string[] semitonesSelected, double measuresValue, int prefferedOctave, double[] criteriaWeight, double[] intervalWeight, Random random)
        {
            this.scale = scale;
            this.semitonesSelected = semitonesSelected;
            this.random = random;
            Genes = GenerateChromosome(measuresValue);

            this.prefferedOctave = prefferedOctave;
            this.criteriaWeight = criteriaWeight;
            this.intervalWeight = intervalWeight;
        }
        private List<Gene> GenerateChromosome(double measuresValue)
        {
            double sum = 0;
            double durationValue;
            List<Gene> genes = new List<Gene>();

            for (int i = 1; i <= measuresValue; i++){
                while (sum != i){
                    Gene gene = new Gene(semitonesSelected, random);
                    durationValue = gene.GeneDuration;

                    if (sum + durationValue <= i){
                        genes.Add(gene);
                        sum += durationValue;
                    }
                }
            }
            return genes;
        }

        public double[] EvaluateChromosome(Chromosome chromosome)
        {
            double evaluationNote = EvaluateNote(chromosome);
            double evaluationDuration = EvaluateDuration(chromosome);
            double evaluationOctave = EvaluateOctave(chromosome);
            double evaluationInterval = EvaluateInterval(chromosome);

            double evaluation = (criteriaWeight[0] * evaluationNote) + (criteriaWeight[1] * evaluationDuration) + (criteriaWeight[2] * evaluationOctave) + (criteriaWeight[3] * evaluationInterval);
            double[] evaluationValues = { evaluationNote, evaluationDuration, evaluationOctave, evaluationInterval, evaluation };

            Fitness = evaluation;
            return evaluationValues;
        }

        private double EvaluateNote(Chromosome chromosomeDecoded)
        {
            double evaluation = 0;
            double chromosomeLength = chromosomeDecoded.Genes.Count;

            for (int i = 0; i < chromosomeLength; i++){
                string noteValue = chromosomeDecoded.Genes[i].GeneNote;
                if (scale.Contains(noteValue)){
                    evaluation++;
                }
            }
            double ratio = evaluation / chromosomeLength;
            return ratio;
        }
        private double EvaluateDuration(Chromosome chromosomeDecoded)
        {
            double evaluation = 0;
            double chromosomeLength = chromosomeDecoded.Genes.Count;

            for (int i = 0; i < chromosomeLength; i++){
                double durationValue = chromosomeDecoded.Genes[i].GeneDuration;
                if (durationValue != 0.75 && durationValue != 0.375 && durationValue != 0.1875){
                    evaluation++;
                }
            }
            double ratio = evaluation / chromosomeLength;
            return ratio;
        }
        private double EvaluateOctave(Chromosome chromosomeDecoded)
        {
            double evaluation = 0;
            double chromosomeLength = chromosomeDecoded.Genes.Count;

            for (int i = 0; i < chromosomeLength - 1; i++){
                int oktaveValue = chromosomeDecoded.Genes[i].GeneOctave;
                if (oktaveValue == prefferedOctave){
                    evaluation++;
                }
            }
            double ratio = evaluation / chromosomeLength;
            return ratio;
        }
        private double EvaluateInterval(Chromosome chromosomeDecoded)
        {
            double evaluation = 0;
            double chromosomeLength = chromosomeDecoded.Genes.Count;

            for (int i = 0; i <= chromosomeLength - 2; i++){
                int interval = CalculateInterval(chromosomeDecoded.Genes[i], chromosomeDecoded.Genes[i + 1]);
                evaluation += CalculateEvaluationInterval(interval);
            }

            double ratio = evaluation / (chromosomeLength - 1);
            return ratio;
        }

        private int CalculateInterval(Gene gene1, Gene gene2)
        {
            string noteValue1 = gene1.GeneNote;
            int oktaveValue1 = gene1.GeneOctave;

            string noteValue2 = gene2.GeneNote;
            int oktaveValue2 = gene2.GeneOctave;

            int interval = 0;
            int indexNote1 = Array.IndexOf(semitonesSelected, noteValue1);
            int indexNote2 = Array.IndexOf(semitonesSelected, noteValue2);

            if (oktaveValue1 < oktaveValue2 && oktaveValue2 - oktaveValue1 == 1){
                interval = semitonesSelected.Length - 1 - indexNote1 + indexNote2 + 1;
            }
            else if (oktaveValue1 > oktaveValue2 && oktaveValue1 - oktaveValue2 == 1){
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
            else if ((oktaveValue1 < oktaveValue2 && oktaveValue2 - oktaveValue1 > 1) || (oktaveValue1 > oktaveValue2 && oktaveValue1 - oktaveValue2 > 1)){
                interval = 13;
            }

            return interval;
        }

        private double CalculateEvaluationInterval(int interval)
        {
            double evaluation = 0;

            if (interval == 0){
                evaluation += intervalWeight[0];
            }
            else if (interval == 1 || interval == 2){
                evaluation += intervalWeight[1];
            }
            else if (interval == 3 || interval == 4){
                evaluation += intervalWeight[2];
            }
            else if (interval == 5 || interval == 6){
                evaluation += intervalWeight[3];
            }
            else if (interval == 7){
                evaluation += intervalWeight[4];
            }
            else if (interval == 8 || interval == 9){
                evaluation += intervalWeight[5];
            }
            else if (interval == 10 || interval == 11){
                evaluation += intervalWeight[6];
            }
            else if (interval == 12){
                evaluation += intervalWeight[7];
            }
            else if (interval > 12){
                evaluation += intervalWeight[8];
            }

            return evaluation;
        }
    }
}
