﻿using M;
using Manufaktura.Controls.Model;
using Manufaktura.Music.Model;
using Manufaktura.Music.Model.MajorAndMinor;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace MusicG
{
    public class MusicAdapter
    {
        //MIDI
        public static Chromosome CheckNote(Chromosome chromosome, string[] semitonesSelected)
        {
            for (int i = 0; i < semitonesSelected.Length; i++){   
                if (semitonesSelected[i].Contains('b') == true){
                    chromosome = ChangeNote(chromosome);
                    break;
                }
            } 
            return chromosome;
        }

        public static Chromosome ChangeNote(Chromosome chromosome)
        {
            for (int i = 0; i < chromosome.Genes.Count; i++){
                string noteValue = chromosome.Genes[i].GeneNote;

                switch (noteValue){
                    case "Db":
                        noteValue = "C#"; break;
                    case "Eb":
                        noteValue = "D#"; break;
                    case "Gb":
                        noteValue = "F#"; break;
                    case "Ab":
                        noteValue = "G#"; break;
                    case "Bb":
                        noteValue = "A#"; break;
                    default:
                        break;
                }
                chromosome.Genes[i].GeneNote = noteValue;
                chromosome.Genes[i].CodeGene();
            }
            return chromosome;
        }

        public static List<MidiNote> GetNotesSequence(Chromosome chromosome)
        {
            int counter = 0;
            var noteMap = new List<MidiNote>();

            for (int i = 0; i < chromosome.Genes.Count; i++){
                string noteValue = chromosome.Genes[i].GeneNote;
                string octaveValue = (chromosome.Genes[i].GeneOctave+1).ToString();
                double durationValue = chromosome.Genes[i].GeneDuration;

                int value = (int)(durationValue * 1920);
                noteMap.Add(new MidiNote(counter, 0, noteValue + octaveValue, 127, value - 1));
                counter += value;
            }
            return noteMap;
        }

        public static void SaveToMIDI(Chromosome chromosome, string scaleName)
        {
            var file = new MidiFile();

            List<MidiNote> noteMap = GetNotesSequence(chromosome);
            var sequenceTraks = new List<MidiSequence>();
            sequenceTraks.Add(MidiSequence.FromNoteMap(noteMap));
            var track = MidiSequence.Merge(sequenceTraks);
            var finalTrack = MidiSequence.Merge(track, track);
            file.Tracks.Add(finalTrack);

            //OPEN DIALOG
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "MIDI file (*.mid)|*.mid";
            saveFileDialog.FileName = "Melodia "+ scaleName;
            saveFileDialog.DefaultExt = ".mid";

            if (saveFileDialog.ShowDialog() == true){
                file.WriteTo(saveFileDialog.FileName);
            }
        }

        //SHEET MUSIC
        public static Score WriteSheetMusic(Chromosome chromosome, string scaleSet, string scaleName)
        {
            MajorScale scale = GetScale(scaleSet, scaleName);
            Score score = Score.CreateOneStaffScore(Clef.Treble, scale);
            var firstStaff = score.FirstStaff;
            firstStaff.Elements.Add(new TimeSignature(TimeSignatureType.Numbers, 4, 4));

            double sum = 0;

            Dictionary<string, string[]> selectedScaleDictionary = new Dictionary<string, string[]>(MusicData.scaleMajor);
            if (scaleSet == "minor"){
                selectedScaleDictionary = new Dictionary<string, string[]>(MusicData.scaleMinor);
            }
            else if (scaleSet == "major"){
                selectedScaleDictionary = new Dictionary<string, string[]>(MusicData.scaleMajor);
            }

            List<string> scaleValuesSign = new List<string>();
            List<string> scaleValues = selectedScaleDictionary[scaleName].ToList();

            for (int i = 0; i < scaleValues.Count; i++){
                if (scaleValues[i].Contains("#") || scaleValues[i].Contains("b")){
                    scaleValuesSign.Add(scaleValues[i].ToString().Replace("#", "").Replace("b", ""));
                }
            }

            for (int i = 0; i < chromosome.Genes.Count; i++){
                string noteValue = chromosome.Genes[i].GeneNote;
                string octaveValue = chromosome.Genes[i].GeneOctave.ToString();

                if (noteValue.Contains('#') == true){
                    noteValue = noteValue.Replace("#", "Sharp");
                }

                string noteFull = noteValue + octaveValue;
                double durationValue = chromosome.Genes[i].GeneDuration;
                sum += durationValue;

                Pitch pitch = GetNote(noteFull);
                RhythmicDuration rhytmicDuration = GetDuration(durationValue);

                Note note = new Note(pitch, rhytmicDuration);
                if (scaleValuesSign.Contains(noteValue)){
                    note.hasNatural = true;
                }

                score.FirstStaff.Elements.Add(note);
                if (sum % 1 == 0){
                    score.FirstStaff.Elements.Add(new Barline());
                }
            }
            return score;
        }

        public static MajorScale GetScale(string scaleSet, string scaleName)
        {
            MajorScale scale = MajorScale.C;

            if (scaleSet == "minor"){
                switch (scaleName){
                    case "A-moll":
                        scale = MajorScale.C; break;
                    case "E-moll":
                        scale = MajorScale.G; break;
                    case "H-moll":
                        scale = MajorScale.D; break;
                    case "Fis-moll":
                        scale = MajorScale.A; break;
                    case "Cis-moll":
                        scale = MajorScale.E; break;
                    case "Gis-moll":
                        scale = MajorScale.B; break;
                    case "D-moll":
                        scale = MajorScale.F; break;
                    case "G-moll":
                        scale = MajorScale.Bb; break;
                    case "C-moll":
                        scale = MajorScale.Eb; break;
                    case "F-moll":
                        scale = MajorScale.Ab; break;
                    case "B-moll":
                        scale = MajorScale.Db; break;
                    default:
                        break;
                }
            }
            else if (scaleSet == "major"){
                switch (scaleName){
                    case "C-dur":
                        scale = MajorScale.C; break;
                    case "G-dur":
                        scale = MajorScale.G; break;
                    case "D-dur":
                        scale = MajorScale.D; break;
                    case "A-dur":
                        scale = MajorScale.A; break;
                    case "E-dur":
                        scale = MajorScale.E; break;
                    case "H-dur":
                        scale = MajorScale.B; break;
                    case "F-dur":
                        scale = MajorScale.F; break;
                    case "B-dur":
                        scale = MajorScale.Bb; break;
                    case "Es-dur":
                        scale = MajorScale.Eb; break;
                    case "As-dur":
                        scale = MajorScale.Ab; break;
                    case "Des-dur":
                        scale = MajorScale.Db; break;
                    default:
                        break;
                }
            }
            return scale;
        }

        public static Pitch GetNote(string noteValue)
        {
            Pitch pitch = new Pitch();
            Pitch pitchValue = (Pitch)pitch.GetType().GetProperty(noteValue).GetValue(pitch, null);

            return pitchValue;
        }

        public static RhythmicDuration GetDuration(double durationValue)
        {
            RhythmicDuration rhytmicDuration = new RhythmicDuration();

            switch (durationValue){
                case 1.0:
                    rhytmicDuration = RhythmicDuration.Whole; break;
                case 0.75:
                    rhytmicDuration = RhythmicDuration.HalfDot; break;
                case 0.5:
                    rhytmicDuration = RhythmicDuration.Half; break;
                case 0.375:
                    rhytmicDuration = RhythmicDuration.QuarterDot; break;
                case 0.25:
                    rhytmicDuration = RhythmicDuration.Quarter; break;
                case 0.1875:
                    rhytmicDuration = RhythmicDuration.EighthDot; break;
                case 0.125:
                    rhytmicDuration = RhythmicDuration.Eighth; break;
                case 0.0625:
                    rhytmicDuration = RhythmicDuration.Sixteenth; break;
            }
            return rhytmicDuration;
        }
    }
}
