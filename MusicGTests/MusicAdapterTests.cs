using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicG;
using System;
using System.Collections.Generic;

namespace MusicGTests
{
    [TestClass]
    public class MusicAdapterTests
    {
        private Random random = new Random();

        [TestMethod]
        public void CheckNote_WhenCalled_ShouldReturnChromosomeWithoutChange()
        {
            string[] scale = new[] { "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = MusicData.semitonesSharp;
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            Chromosome chromosome = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);
            Chromosome chromosomeAfterCheck = MusicAdapter.CheckNote(chromosome, scale);

            Assert.AreEqual(chromosome, chromosomeAfterCheck);
        }

        [TestMethod]
        public void ChangeNote_WhenCalled_ShouldReturnChromosomeWithChangedNotes()
        {
            string[] scale = new[] { "Ab", "Bb", "C", "Db", "Eb", "F", "G" };
            string[] semitones = MusicData.semitonesFlat;
            double[] criteriaWeight = { 1.0, 1.0, 0.75, 0.5 };
            double[] intervalWeight = { 0, 1, 1, 0.75, 0.75, 0.5, 0.5, 0.25, 0 };

            Chromosome chromosome = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);
            chromosome.Genes = new List<Gene>() { new Gene(semitones, "Db", 4, 0.75), new Gene(semitones, "C", 5, 1.0),
                new Gene(semitones, "A", 4, 0.5), new Gene(semitones, "Bb", 5, 0.125)};
            MusicAdapter.ChangeNote(chromosome);

            Chromosome chromosomeExpected = new Chromosome(scale, semitones, 4.0, 4, criteriaWeight, intervalWeight, random);
            chromosomeExpected.Genes = new List<Gene>() { new Gene(semitones, "C#", 4, 0.75), new Gene(semitones, "C", 5, 1.0),
                new Gene(semitones, "A", 4, 0.5), new Gene(semitones, "A#", 5, 0.125)};

            for (int i=0; i<chromosome.Genes.Count; i++){
                Assert.AreEqual(chromosomeExpected.Genes[i].GeneNote, chromosome.Genes[i].GeneNote);
            }  
        }


    }
}
