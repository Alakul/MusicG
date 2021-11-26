using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmForComposing
{
    public static class Music
    {
        public static Dictionary<string, string[]> scaleValues = new Dictionary<string, string[]>(){
            {"Durowe", new string[] { "C-dur", "G-dur", "D-dur", "A-dur", "E-dur", "H-dur", "Fis-dur", "Cis-dur", "F-dur", "B-dur", "Es-dur", "As-dur" }},
            { "Molowe", new string[] { "A-moll", "E-moll", "H-moll", "Fis-moll", "Cis-moll", "Gis-moll", "Dis-moll", "Ais-moll", "D-moll", "G-moll", "C-moll", "F-moll" }}
        };

        public static Dictionary<string, string[]> scaleMajor = new Dictionary<string, string[]>(){
            {"C-dur",   new string[] { "C", "D", "E", "F", "G", "A", "B" }},
            { "G-dur",   new string[] { "G", "A", "B", "C", "D", "E", "F#" }},
            { "D-dur",   new string[] { "D", "E", "F#", "G", "A", "B", "C#" }},
            { "A-dur",   new string[] { "A", "B", "C#", "D", "E", "F#", "G#" }},
            { "E-dur",   new string[] { "E", "F#", "G#", "A", "B", "C#", "D#" }},
            { "H-dur",   new string[] { "B", "C#", "D#", "E", "F#", "G#", "A#" }},
            { "Fis-dur", new string[] { "F#", "G#", "A#", "B", "C#", "D#", "F" }},
            { "Cis-dur", new string[] { "C#", "D#", "F", "F#", "G#", "A#", "C" }},
            { "F-dur",   new string[] { "F", "G", "A", "Bb", "C", "D", "E"}},
            { "B-dur",   new string[] { "Bb", "C", "D", "Eb", "F", "G", "A" }},
            { "Es-dur",  new string[] { "Eb", "F", "G", "Ab", "Bb", "C", "D" }},
            { "As-dur",  new string[] { "Ab", "Bb", "C", "Db", "Eb", "F", "G" }}
        };

        public static Dictionary<string, string[]> scaleMinor = new Dictionary<string, string[]>(){
            {"A-moll",    new string[] { "A", "B", "C", "D", "E", "F", "G" }},
            { "E-moll",   new string[] { "E", "F#", "G", "A", "B", "C", "D" }},
            { "H-moll",   new string[] { "B", "C#", "D", "E", "F#", "G", "A" }},
            { "Fis-moll", new string[] { "F#", "G#", "A", "B", "C#", "D", "E" }},
            { "Cis-moll", new string[] { "C#", "D#", "E", "F#", "G#", "A", "B" }},
            { "Gis-moll", new string[] { "G#", "A#", "B", "C#", "D#", "E", "F#" }},
            { "Dis-moll", new string[] { "D#", "F", "F#", "G#", "A#", "B", "C#" }},
            { "Ais-moll", new string[] { "A#", "C", "C#", "D#", "F", "F#", "G#" }},
            { "D-moll",   new string[] { "D", "E", "F", "G", "A", "Bb", "C" }},
            { "G-moll",   new string[] { "G", "A", "Bb", "C", "D", "Eb", "F" }},
            { "C-moll",   new string[] { "C", "D", "Eb", "F", "G", "Ab", "Bb" }},
            { "F-moll",   new string[] { "F", "G", "Ab", "Bb", "C", "Db", "Eb" }}
        };

        
        public static string[] semitonesSharp = new[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        public static string[] semitonesFlat = new[] { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };
        public static int semitones = 12;

        public static double[] duration = new[] { 1.0, 0.75, 0.5, 0.375, 0.25, 0.1875, 0.125, 0.09375, 0.0625 };
        public static int[] octaveValues = new[] { 1, 2, 3, 4, 5, 6, 7 };
    }
}
