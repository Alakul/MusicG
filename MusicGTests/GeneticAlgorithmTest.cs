using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicG;

namespace MusicGTests
{
    [TestClass]
    public class GeneticAlgorithmTest
    {
        [TestMethod]
        public void SetSign_Test()
        {
            string[] scale = new[]{ "E", "F#", "G#", "A", "B", "C#", "D#" };
            string[] semitones = GeneticAlgorithm.SetSign(scale);

            Assert.AreEqual(semitones, MusicData.semitonesSharp);
        }

        [TestMethod]
        public void CodeGene_Test()
        {
            string geneToCode = "C#;5;0,75";
            string[] semitones = MusicData.semitonesSharp;
            string codedGene = GeneticAlgorithm.CodeGene(geneToCode, semitones);

            Assert.AreEqual(codedGene, "01000000000010101000000");
        }

        [TestMethod]
        public void DecodeGene_Test()
        {
            string geneToDecode = "01000000000010101000000";
            string[] semitones = MusicData.semitonesSharp;
            string decodedGene = GeneticAlgorithm.DecodeGene(geneToDecode, semitones);

            Assert.AreEqual(decodedGene, "C#;5;0,75");
        }

        [TestMethod]
        public void CodeNote_Test()
        {
            string noteToCode = "F";
            string[] semitones = MusicData.semitonesSharp;
            string codedNote = GeneticAlgorithm.CodeNote(noteToCode, semitones);

            Assert.AreEqual(codedNote, "000001000000");
        }

        [TestMethod]
        public void DecodeNote_Test()
        {
            string noteToDecode = "000001000000";
            string[] semitones = MusicData.semitonesSharp;
            string decodedNote = GeneticAlgorithm.DecodeNote(noteToDecode, semitones);

            Assert.AreEqual(decodedNote, "F");
        }

        [TestMethod]
        public void CodeOctave_Test()
        {
            int octaveToCode = 3;
            string codedNote = GeneticAlgorithm.CodeOctave(octaveToCode);

            Assert.AreEqual(codedNote, "011");
        }

        [TestMethod]
        public void DecodeOctave_Test()
        {
            string octaveToDecode = "011";
            string decodedNote = GeneticAlgorithm.DecodeOctave(octaveToDecode);

            Assert.AreEqual(decodedNote, "3");
        }

        [TestMethod]
        public void CodeDuration_Test()
        {
            double durationToCode = 0.75;
            string codedNote = GeneticAlgorithm.CodeDuration(durationToCode);

            Assert.AreEqual(codedNote, "01000000");
        }

        [TestMethod]
        public void DecodeDuration_Test()
        {
            string durationToDecode = "01000000";
            string decodedNote = GeneticAlgorithm.DecodeDuration(durationToDecode);

            Assert.AreEqual(decodedNote, "0,75");
        }
    }
}
