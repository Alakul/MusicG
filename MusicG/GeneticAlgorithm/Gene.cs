using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicG
{
    public class Gene
    {
        public string GeneNote { get; set; } 
        public int GeneOctave { get; set; }
        public double GeneDuration { get; set; }

        public string GeneNoteCoded { get; set; }
        public string GeneOctaveCoded { get; set; }
        public string GeneDurationCoded { get; set; }


        private string[] semitonesSelected;
        private Random random;
        private int semitones = MusicData.semitones;
        private int[] octaveValues = MusicData.octaveValues;
        private double[] duration = MusicData.duration;
        private double[] durationBase = MusicData.durationBase;

        public Gene(string[] semitonesSelected, Random random)
        {
            this.semitonesSelected = semitonesSelected;
            this.random = random;

            GeneNote = GenerateNote();
            GeneOctave = GenerateOctave();
            GeneDuration = GenerateDuration();
            GeneNoteCoded = CodeNote(GeneNote);
            GeneOctaveCoded = CodeOctave(GeneOctave);
            GeneDurationCoded = CodeDuration(GeneDuration);
        }

        public Gene(string[] semitonesSelected, string noteValue, int octaveValue, double durationValue)
        {
            this.semitonesSelected = semitonesSelected;

            GeneNote = noteValue;
            GeneOctave = octaveValue;
            GeneDuration = durationValue;
            GeneNoteCoded = CodeNote(noteValue);
            GeneOctaveCoded = CodeOctave(octaveValue);
            GeneDurationCoded = CodeDuration(durationValue);
        }

        private string GenerateNote()
        {
            int noteRandom = random.Next(0, semitonesSelected.Length);
            string noteValue = semitonesSelected[noteRandom];
            return noteValue;
        }

        private int GenerateOctave()
        {
            int oktaveRandom = random.Next(0, octaveValues.Length);
            int octaveValue = octaveValues[oktaveRandom];
            return octaveValue;
        }

        private double GenerateDuration()
        {
            int durationRandom = random.Next(0, durationBase.Length);
            double durationValue = durationBase[durationRandom];
            return durationValue;
        }

        private string CodeNote(string noteValue)
        {
            string noteCoded = "";
            for (int i = 0; i < semitones; i++){
                if (noteValue == semitonesSelected[i]){
                    for (int j = 0; j < semitones; j++){
                        if (j == i){
                            noteCoded += "1";
                        }
                        else {
                            noteCoded += "0";
                        }
                    }
                }
            }
            return noteCoded;
        }

        private string CodeOctave(int octaveValue)
        {
            string octaveCoded = Convert.ToString(octaveValue, 2).PadLeft(3, '0');
            return octaveCoded;
        }

        private string CodeDuration(double durationValue)
        {
            string durationCoded = "";
            for (int i = 0; i < duration.Length; i++){
                if (durationValue == duration[i]){
                    for (int j = 0; j < duration.Length; j++){
                        if (j == i){
                            durationCoded += "1";
                        }
                        else {
                            durationCoded += "0";
                        }
                    }
                }
            }
            return durationCoded;
        }

        public string DecodeNote()
        {
            int noteIndex = GeneNoteCoded.IndexOf('1');
            string noteDecoded = semitonesSelected[noteIndex].ToString();
            return noteDecoded;
        }

        public int DecodeOctave()
        {
            int octaveDecoded = Convert.ToInt32(GeneOctaveCoded, 2);
            return octaveDecoded;
        }

        private double DecodeDuration()
        {
            int durationIndex = GeneDurationCoded.IndexOf('1');
            double durationDecoded = duration[durationIndex];
            return durationDecoded;
        }

        public void CodeGene()
        {
            GeneNoteCoded = CodeNote(GeneNote);
            GeneOctaveCoded = CodeOctave(GeneOctave);
            GeneDurationCoded = CodeDuration(GeneDuration);
        }

        public void DecodeGene()
        {
            GeneNote = DecodeNote();
            GeneOctave = DecodeOctave();
            GeneDuration = DecodeDuration();
        }

        public string GetGeneDecoded()
        {
            return GeneNote + ";" + GeneOctave + ";" + GeneDuration;
        }

        public string GetGeneCoded()
        {
            return GeneNoteCoded + GeneOctaveCoded + GeneDurationCoded;
        }
    }
}
