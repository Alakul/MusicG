using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicG;
using System;
using System.Reflection;

namespace MusicGTests
{
    [TestClass]
    public class GeneTests
    {
        private Random random = new Random();

        [TestMethod]
        public void GeneConstructor_WhenCalled_ShouldReturnGene()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, random);
            
            Assert.IsNotNull(gene);
        }

        [TestMethod]
        public void GeneConstructor_WhenCalled_ShouldReturnGeneWithHandedOverProperties()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, "C#", 3, 0.75);

            Assert.IsNotNull(gene);
            Assert.AreEqual(gene.GeneNote, "C#");
            Assert.AreEqual(gene.GeneOctave, 3);
            Assert.AreEqual(gene.GeneDuration, 0.75);
            Assert.AreEqual(gene.GeneNoteCoded, "010000000000");
            Assert.AreEqual(gene.GeneOctaveCoded, "011");
            Assert.AreEqual(gene.GeneDurationCoded, "01000000");
        }

        [TestMethod]
        public void GenerateNote_WhenCalled_ShouldReturnNote()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, random);

            MethodInfo methodInfo = typeof(Gene).GetMethod("GenerateNote",BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { };
            string note = (string)methodInfo.Invoke(gene, parameters);

            Assert.IsNotNull(note);
        }

        [TestMethod]
        public void GenerateOctave_WhenCalled_ShouldReturnOctave()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, random);

            MethodInfo methodInfo = typeof(Gene).GetMethod("GenerateOctave", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { };
            int octave = (int)methodInfo.Invoke(gene, parameters);

            Assert.IsNotNull(octave);
        }

        [TestMethod]
        public void GenerateDuration_WhenCalled_ShouldReturnDuration()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, random);

            MethodInfo methodInfo = typeof(Gene).GetMethod("GenerateDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { };
            double duration = (double)methodInfo.Invoke(gene, parameters);

            Assert.IsNotNull(duration);
        }

        [TestMethod]
        public void CodeNote_WhenCalled_ShouldReturnCodedNote()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, random);

            MethodInfo methodInfo = typeof(Gene).GetMethod("CodeNote", BindingFlags.NonPublic | BindingFlags.Instance);
            string noteValue = "C#";
            object[] parameters = { noteValue };
            string noteCoded = (string)methodInfo.Invoke(gene, parameters);

            Assert.AreEqual("010000000000", noteCoded);
        }

        [TestMethod]
        public void CodeOctave_WhenCalled_ShouldReturnCodedOctave()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, random);

            MethodInfo methodInfo = typeof(Gene).GetMethod("CodeOctave", BindingFlags.NonPublic | BindingFlags.Instance);
            int octaveValue = 3;
            object[] parameters = { octaveValue };
            string octaveCoded = (string)methodInfo.Invoke(gene, parameters);

            Assert.AreEqual("011", octaveCoded);
        }

        [TestMethod]
        public void CodeDuration_WhenCalled_ShouldReturnCodedDuration()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, random);

            MethodInfo methodInfo = typeof(Gene).GetMethod("CodeDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            double octaveValue = 0.75;
            object[] parameters = { octaveValue };
            string durationCoded = (string)methodInfo.Invoke(gene, parameters);

            Assert.AreEqual("01000000", durationCoded);
        }

        [TestMethod]
        public void DecodeNote_WhenCalled_ShouldReturnDecodedNote()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, "D#", 5, 1.0);

            MethodInfo methodInfo = typeof(Gene).GetMethod("DecodeNote", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {  };
            string noteDecoded = (string)methodInfo.Invoke(gene, parameters);

            Assert.AreEqual("D#", noteDecoded);
        }

        [TestMethod]
        public void DecodeOctave_WhenCalled_ShouldReturnDecodedOctave()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, "D#", 5, 1.0);

            MethodInfo methodInfo = typeof(Gene).GetMethod("DecodeOctave", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { };
            int octaveDecoded = (int)methodInfo.Invoke(gene, parameters);

            Assert.AreEqual(5, octaveDecoded);
        }

        [TestMethod]
        public void DecodeDuration_WhenCalled_ShouldReturnDecodedDuration()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, "D#", 5, 1.0);

            MethodInfo methodInfo = typeof(Gene).GetMethod("DecodeDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { };
            double durationDecoded = (double)methodInfo.Invoke(gene, parameters);

            Assert.AreEqual(1.0, durationDecoded);
        }

        [TestMethod]
        public void CodeGene_WhenCalled_ShouldReturnCodedGene()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, "D#", 5, 1.0);
            gene.GeneNote = "F#";
            gene.GeneOctave = 4;
            gene.GeneDuration = 0.0625;

            gene.CodeGene();

            Assert.AreEqual("000000100000", gene.GeneNoteCoded);
            Assert.AreEqual("100", gene.GeneOctaveCoded);
            Assert.AreEqual("00000001", gene.GeneDurationCoded);
        }

        [TestMethod]
        public void DecodeGene_WhenCalled_ShouldReturnDecodedGene()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, "D#", 5, 1.0);
            gene.GeneNoteCoded = "000000100000";
            gene.GeneOctaveCoded = "100";
            gene.GeneDurationCoded = "00000001";

            gene.DecodeGene();

            Assert.AreEqual("F#", gene.GeneNote);
            Assert.AreEqual(4, gene.GeneOctave);
            Assert.AreEqual(0.0625, gene.GeneDuration);
        }

        [TestMethod]
        public void GetGeneDecoded_WhenCalled_ShouldReturnGeneDecodedString()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, "D#", 5, 1.0);

            string geneDecoded = gene.GetGeneDecoded();

            Assert.AreEqual("D#;5;1", geneDecoded);
        }

        [TestMethod]
        public void GetGeneCoded_WhenCalled_ShouldReturnGeneCodedString()
        {
            string[] semitones = MusicData.semitonesSharp;
            Gene gene = new Gene(semitones, "D#", 5, 1.0);

            string geneCoded = gene.GetGeneCoded();

            Assert.AreEqual("00010000000010110000000", geneCoded);
        }
    }
}
