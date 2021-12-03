using M;
using Manufaktura.Controls.Model;
using Manufaktura.Music.Model;
using Manufaktura.Music.Model.MajorAndMinor;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmForComposing
{
    class MusicController
    {
        //MIDI
        public static List<MidiNote> GetNotesSequence(string[] chromosome, string[] semitonesSelected)
        {
            int counter = 0;
            int value = 60;

            var noteMap = new List<MidiNote>();

            for (int i = 0; i < chromosome.Length; i++)
            {
                string geneDecoded = GeneticAlgorithm.DecodeGene(chromosome[i], semitonesSelected);
                string[] geneValues = geneDecoded.Split(';');
                string noteValue = geneValues[0];
                string octaveValue = geneValues[1];
                double durationValue = double.Parse(geneValues[2]);

                switch (durationValue)
                {
                    case 1.0:
                        value = 1920;
                        break;
                    case 0.75:
                        value = 1440;
                        break;
                    case 0.5:
                        value = 960;
                        break;
                    case 0.375:
                        value = 720;
                        break;
                    case 0.25:
                        value = 480;
                        break;
                    case 0.1875:
                        value = 360;
                        break;
                    case 0.125:
                        value = 240;
                        break;
                    case 0.0625:
                        value = 120;
                        break;
                }

                noteMap.Add(new MidiNote(counter, 0, noteValue + octaveValue, 127, value - 1));
                counter += value;
            }
            return noteMap;
        }

        public static void SaveToMIDI(string[] chromosome, string[] semitonesSelected)
        {
            var file = new MidiFile();

            List<MidiNote> noteMap = GetNotesSequence(chromosome, semitonesSelected);
            var sequenceTraks = new List<MidiSequence>();
            sequenceTraks.Add(MidiSequence.FromNoteMap(noteMap));
            var track = MidiSequence.Merge(sequenceTraks);
            var finalTrack = MidiSequence.Merge(track, track);
            file.Tracks.Add(finalTrack);

            //OPEN DIALOG
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "MIDI file (*.mid)|*.mid";
            saveFileDialog.FileName = "Melody";
            saveFileDialog.DefaultExt = ".mid";

            if (saveFileDialog.ShowDialog() == true)
            {
                file.WriteTo(saveFileDialog.FileName);
            }
        }

        //SHEET MUSIC
        public static void GetNote()
        {
            //zwraca nutę w postaci dla notacji

        }

        public static void GetDuration()
        {
            //zwraca czas trwania nuty w postaci dla notacji

        }

        public static Score WriteSheetMusic()
        {/*
            string[] chromosomeDecoded = DecodeChromosome(chromosome, semitonesSelected);

            for (int i = 0; i < chromosomeDecoded.Length; i++)
            {
                string gene = chromosomeDecoded[i];
                string[] geneValues = gene.Split(';');
                string noteValue = geneValues[0];
                int octaveValue = int.Parse(geneValues[1]);
                double durationValue = double.Parse(geneValues[2]);
            }
            */
            var r = RhythmicDuration.Quarter;
            Score score = Score.CreateOneStaffScore(Clef.Treble, new MajorScale(Step.C, false));
            score.FirstStaff.Elements.Add(new Note(Pitch.C4, r));
            score.FirstStaff.Elements.Add(new Note(Pitch.E4, r));
            score.FirstStaff.Elements.Add(new Note(Pitch.D4, r));
            score.FirstStaff.Elements.Add(new Note(Pitch.D4, r));
            score.FirstStaff.Elements.Add(new Barline());
            score.FirstStaff.Elements.Add(new Note(Pitch.D4, r));

            return score;
        }

    }
}
