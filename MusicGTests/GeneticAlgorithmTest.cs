using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicG;

namespace MusicGTests
{
    [TestClass]
    public class GeneticAlgorithmTest
    {
        [TestMethod]
        public void SetSign_WhenCalled_ShouldReturnSemitonesWithSharp()
        {
            string[] scale = new[]{ "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = GeneticAlgorithm.SetSign(scale);

            Assert.AreEqual(semitones, MusicData.semitonesSharp);
        }

        [TestMethod]
        public void SetSign_WhenCalled_ShouldReturnSemitonesWithBemol()
        {
            string[] scale = new[] { "G", "A", "Bb", "C", "D", "Eb", "F" };
            string[] semitones = GeneticAlgorithm.SetSign(scale);

            Assert.AreEqual(semitones, MusicData.semitonesFlat);
        }

        [TestMethod]
        public void SetSign_WhenCalledWithNoKeySigns_ShouldReturnSemitonesWithSharp()
        {
            string[] scale = new[] { "C", "D", "E", "F", "G", "A", "B" };
            string[] semitones = GeneticAlgorithm.SetSign(scale);

            Assert.AreEqual(semitones, MusicData.semitonesSharp);
        }

        [TestMethod]
        public void CodeGene_WhenCalled_ShouldReturnCodedGene()
        {
            string geneToCode = "C#;5;0,75";
            string[] semitones = MusicData.semitonesSharp;
            string codedGene = GeneticAlgorithm.CodeGene(geneToCode, semitones);

            Assert.AreEqual(codedGene, "01000000000010101000000");
        }

        [TestMethod]
        public void DecodeGene_WhenCalled_ShouldReturnDecodedGene()
        {
            string geneToDecode = "01000000000010101000000";
            string[] semitones = MusicData.semitonesSharp;
            string decodedGene = GeneticAlgorithm.DecodeGene(geneToDecode, semitones);

            Assert.AreEqual(decodedGene, "C#;5;0,75");
        }

        [TestMethod]
        public void CodeNote_WhenCalled_ShouldReturnCodedNote()
        {
            string noteToCode = "F";
            string[] semitones = MusicData.semitonesSharp;
            string codedNote = GeneticAlgorithm.CodeNote(noteToCode, semitones);

            Assert.AreEqual(codedNote, "000001000000");
        }

        [TestMethod]
        public void DecodeNote_WhenCalled_ShouldReturnDecodedNote()
        {
            string noteToDecode = "000001000000";
            string[] semitones = MusicData.semitonesSharp;
            string decodedNote = GeneticAlgorithm.DecodeNote(noteToDecode, semitones);

            Assert.AreEqual(decodedNote, "F");
        }

        [TestMethod]
        public void CodeOctave_WhenCalled_ShouldReturnCodedOctave()
        {
            int octaveToCode = 3;
            string codedOctave = GeneticAlgorithm.CodeOctave(octaveToCode);

            Assert.AreEqual(codedOctave, "011");
        }

        [TestMethod]
        public void DecodeOctave_WhenCalled_ShouldReturnDecodedOctave()
        {
            string octaveToDecode = "011";
            string decodedOctave = GeneticAlgorithm.DecodeOctave(octaveToDecode);

            Assert.AreEqual(decodedOctave, "3");
        }

        [TestMethod]
        public void CodeDuration_WhenCalled_ShouldReturnCodedDuration()
        {
            double durationToCode = 0.75;
            string codedDuration = GeneticAlgorithm.CodeDuration(durationToCode);

            Assert.AreEqual(codedDuration, "01000000");
        }

        [TestMethod]
        public void DecodeDuration_WhenCalled_ShouldReturnDecodedDuration()
        {
            string durationToDecode = "01000000";
            string decodedDuration = GeneticAlgorithm.DecodeDuration(durationToDecode);

            Assert.AreEqual(decodedDuration, "0,75");
        }

        [TestMethod]
        public void RandomNumber_WhenCalled_ShouldReturnRandomDouble()
        {
            double min = 5.3;
            double max = 8.6;
            double randomNumber = GeneticAlgorithm.RandomNumber(min, max);

            Assert.IsTrue(randomNumber > min);
            Assert.IsTrue(randomNumber < max);
        }

        [TestMethod]
        public void CheckSum_WhenCalled_ShouldReturnTwoDurations()
        {
            double sum = 0.875;
            string durations = GeneticAlgorithm.CheckSum(sum);

            Assert.AreEqual(durations, "0,75;0,125");
        }

        [TestMethod]
        public void CheckSum_WhenCalled_ShouldReturnEmptyString()
        {
            double sum = 0;
            string durations = GeneticAlgorithm.CheckSum(sum);

            Assert.AreEqual(durations, "");
        }
    }
}
