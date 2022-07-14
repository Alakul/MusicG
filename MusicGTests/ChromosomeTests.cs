using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicG;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MusicGTests
{
    [TestClass]
    public class ChromosomeTests
    {
        private Random random = new Random();
        [TestMethod]
        public void ChromosomeConstructor_WhenCalled_ShouldReturnChromosomeWithHandedOverProperties()
        {
            string[] scale = new[] { "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = MusicData.semitonesSharp;
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            Chromosome chromosome = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);

            Assert.IsNotNull(chromosome.Genes);
        }

        [TestMethod]
        public void GenerateChromosome_WhenCalled_ShouldReturnChromosome()
        {
            string[] scale = new[] { "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = MusicData.semitonesSharp;
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            Chromosome chromosome = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);
            chromosome.Genes = null;

            MethodInfo methodInfo = typeof(Chromosome).GetMethod("GenerateChromosome", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { 4.0 };
            List<Gene> genes = (List<Gene>)methodInfo.Invoke(chromosome, parameters);

            Assert.IsNotNull(genes);
        }

        [TestMethod]
        public void EvaluateNote_WhenCalled_ShouldReturnEvaluationOfNote()
        {
            string[] scale = new[] { "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = MusicData.semitonesSharp;
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            Chromosome chromosome = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);
            chromosome.Genes = new List<Gene>() { new Gene(semitones, "C#", 4, 0.75), new Gene(semitones, "C", 5, 0.375) };

            MethodInfo methodInfo = typeof(Chromosome).GetMethod("EvaluateNote", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { chromosome };
            double evaluation = (double)methodInfo.Invoke(chromosome, parameters);

            Assert.IsNotNull(evaluation);
            Assert.AreEqual(0.5, evaluation);
        }

        [TestMethod]
        public void EvaluateDuration_WhenCalled_ShouldReturnEvaluationOfDuration()
        {
            string[] scale = new[] { "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = MusicData.semitonesSharp;
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            Chromosome chromosome = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);
            chromosome.Genes = new List<Gene>() { new Gene(semitones, "C#", 4, 0.75), new Gene(semitones, "C", 5, 0.375) };

            MethodInfo methodInfo = typeof(Chromosome).GetMethod("EvaluateDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { chromosome };
            double evaluation = (double)methodInfo.Invoke(chromosome, parameters);

            Assert.IsNotNull(evaluation);
            Assert.AreEqual(0, evaluation);
        }

        [TestMethod]
        public void EvaluateOctave_WhenCalled_ShouldReturnEvaluationOfOctave()
        {
            string[] scale = new[] { "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = MusicData.semitonesSharp;
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            Chromosome chromosome = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);
            chromosome.Genes = new List<Gene>(){ new Gene(semitones, "C#", 4, 0.75), new Gene(semitones, "C", 5, 1.0) };

            MethodInfo methodInfo = typeof(Chromosome).GetMethod("EvaluateOctave", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { chromosome };
            double evaluation = (double)methodInfo.Invoke(chromosome, parameters);

            Assert.IsNotNull(evaluation);
            Assert.AreEqual(0.5, evaluation);
        }

        [TestMethod]
        public void EvaluateInterval_WhenCalled_ShouldReturnEvaluationOfInterval()
        {
            string[] scale = new[] { "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = MusicData.semitonesSharp;
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            Chromosome chromosome = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);
            chromosome.Genes = new List<Gene>() { new Gene(semitones, "C#", 4, 0.75), new Gene(semitones, "C", 5, 1.0),
            new Gene(semitones, "A", 4, 0.5), new Gene(semitones, "G", 5, 0.125)};

            MethodInfo methodInfo = typeof(Chromosome).GetMethod("EvaluateInterval", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { chromosome };
            double evaluation = (double)methodInfo.Invoke(chromosome, parameters);

            Assert.IsNotNull(evaluation);
            Assert.AreEqual(0.6666666666666666, evaluation);
        }

        [TestMethod]
        public void CalculateInterval_WhenCalled_ShouldReturnIntervalOfTwoNotes()
        {
            string[] scale = new[] { "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = MusicData.semitonesSharp;
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            Chromosome chromosome = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);
            Gene gene1 = new Gene(semitones, "B", 4, 1.0);
            Gene gene2 = new Gene(semitones, "B", 5, 1.0);

            MethodInfo methodInfo = typeof(Chromosome).GetMethod("CalculateInterval", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { gene1, gene2 };
            int interval = (int)methodInfo.Invoke(chromosome, parameters);

            Assert.AreEqual(12, interval);
        }

        [TestMethod]
        public void CalculateEvaluationInterval_WhenCalled_ShouldReturnEvaluationOfIntervalOfTwoNotes()
        {
            string[] scale = new[] { "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = MusicData.semitonesSharp;
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            Chromosome chromosome = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);
            int interval = 12;

            MethodInfo methodInfo = typeof(Chromosome).GetMethod("CalculateEvaluationInterval", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { interval };
            double evaluation = (double)methodInfo.Invoke(chromosome, parameters);

            Assert.AreEqual(0.25, evaluation);
        }
    }
}
