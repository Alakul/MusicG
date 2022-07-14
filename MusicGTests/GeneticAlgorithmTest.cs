using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicG;
using System;
using System.Reflection;

namespace MusicGTests
{
    [TestClass]
    public class GeneticAlgorithmTest
    {

        [TestMethod]
        public void SetSign_WhenCalled_ShouldReturnSemitonesWithSharp()
        {
            string[] scale = new[]{ "E", "F#", "G#", "A", "B", "C#", "D#" };
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(scale, 2.0, 4, 40, 1000, 50, 10,
                criteriaWeight, intervalWeight, 3, 0, 0);

            MethodInfo methodInfo = typeof(GeneticAlgorithm).GetMethod("SetSign", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {  };
            string[] semitones = (string[])methodInfo.Invoke(geneticAlgorithm, parameters);

            Assert.AreEqual(semitones, MusicData.semitonesSharp);
        }

        [TestMethod]
        public void SetSign_WhenCalled_ShouldReturnSemitonesWithBemol()
        {
            string[] scale = new[] { "G", "A", "Bb", "C", "D", "Eb", "F" };
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(scale, 2.0, 4, 40, 1000, 50, 10,
                criteriaWeight, intervalWeight, 3, 0, 0);

            MethodInfo methodInfo = typeof(GeneticAlgorithm).GetMethod("SetSign", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { };
            string[] semitones = (string[])methodInfo.Invoke(geneticAlgorithm, parameters);

            Assert.AreEqual(semitones, MusicData.semitonesFlat);
        }

        [TestMethod]
        public void SetSign_WhenCalledWithNoKeySigns_ShouldReturnSemitonesWithSharp()
        {
            string[] scale = new[] { "C", "D", "E", "F", "G", "A", "B" };
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(scale, 2.0, 4, 40, 1000, 50, 10,
                criteriaWeight, intervalWeight, 3, 0, 0);

            MethodInfo methodInfo = typeof(GeneticAlgorithm).GetMethod("SetSign", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { };
            string[] semitones = (string[])methodInfo.Invoke(geneticAlgorithm, parameters);

            Assert.AreEqual(semitones, MusicData.semitonesSharp);
        }

        [TestMethod]
        public void GeneratePopulation_WhenCalled_ShouldGeneratePopulation()
        {
            string[] scale = new[] { "C", "D", "E", "F", "G", "A", "B" };
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(scale, 2.0, 4, 40, 1000, 50, 10,
                criteriaWeight, intervalWeight, 3, 0, 0);
            geneticAlgorithm.Population = null;

            MethodInfo methodInfo = typeof(GeneticAlgorithm).GetMethod("GeneratePopulation", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { };
            methodInfo.Invoke(geneticAlgorithm, parameters);

            Assert.IsNotNull(geneticAlgorithm.Population);
            Assert.AreEqual(40, geneticAlgorithm.Population.Count);
        }

        [TestMethod]
        public void FitnessFunction_WhenCalled_ShouldCalculateFitness()
        {
            string[] scale = new[] { "C", "D", "E", "F", "G", "A", "B" };
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(scale, 2.0, 4, 40, 1000, 50, 10,
                criteriaWeight, intervalWeight, 3, 0, 0);

            MethodInfo methodInfo = typeof(GeneticAlgorithm).GetMethod("FitnessFunction", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { };
            methodInfo.Invoke(geneticAlgorithm, parameters);

            Assert.IsNotNull(geneticAlgorithm.Fitness);
            Assert.IsNotNull(geneticAlgorithm.EvaluationArray);
        }

        [TestMethod]
        public void RandomNumber_WhenCalled_ShouldReturnRandomDouble()
        {
            double min = 5.3;
            double max = 8.6;
            string[] scale = new[] { "C", "D", "E", "F", "G", "A", "B" };
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(scale, 2.0, 4, 40, 1000, 50, 10,
                criteriaWeight, intervalWeight, 3, 0, 0);

            MethodInfo methodInfo = typeof(GeneticAlgorithm).GetMethod("RandomNumber", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { min, max  };
            double randomNumber = (double)methodInfo.Invoke(geneticAlgorithm, parameters);

            Assert.IsTrue(randomNumber > min);
            Assert.IsTrue(randomNumber < max);
        }

        [TestMethod]
        public void CheckSum_WhenCalled_ShouldReturnTwoDurations()
        {
            double sum = 0.875;
            string[] scale = new[] { "C", "D", "E", "F", "G", "A", "B" };
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(scale, 2.0, 4, 40, 1000, 50, 10,
                criteriaWeight, intervalWeight, 3, 0, 0);

            MethodInfo methodInfo = typeof(GeneticAlgorithm).GetMethod("CheckSum", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { sum };
            string sumValue = (string)methodInfo.Invoke(geneticAlgorithm, parameters);

            Assert.AreEqual(sumValue, "0,75;0,125");
        }

        [TestMethod]
        public void CheckSum_WhenCalled_ShouldReturnEmptyString()
        {
            double sum = 0;
            string[] scale = new[] { "C", "D", "E", "F", "G", "A", "B" };
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(scale, 2.0, 4, 40, 1000, 50, 10,
                criteriaWeight, intervalWeight, 3, 0, 0);

            MethodInfo methodInfo = typeof(GeneticAlgorithm).GetMethod("CheckSum", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { sum };
            string sumValue = (string)methodInfo.Invoke(geneticAlgorithm, parameters);

            Assert.AreEqual(sumValue, "");
        }
    }
}
