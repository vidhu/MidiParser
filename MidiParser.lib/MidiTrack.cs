using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidiParser.lib.MidiEvent;
using System.Diagnostics;

namespace MidiParser.lib
{
    public class MidiTrack
    {
        /// <summary>
        /// The file that contains this Midi Track
        /// </summary>
        public MidiFile Parent { get; private set; }

        /// <summary>
        /// Name of this track
        /// </summary>
        public string Name;

        
        List<ChannelVoice> _voice = new List<ChannelVoice>();
        /// <summary>
        /// Contains the ChannelVoices in this track
        /// </summary>
        public List<ChannelVoice> Voice { get { return _voice; } private set { _voice = value; } }

        /// <summary>
        /// Number of ticks in this Track
        /// </summary>
        public long Ticks = 0;

        /// <summary>
        /// Tempo of this track (us per quarternote)
        /// </summary>
        public long Tempo { get { return this.Parent.Tempo; } }

        /// <summary>
        /// Division of this track (ticks per quarternote)
        /// </summary>
        public long Division { get { return this.Parent.Division; } }

        /// <summary>
        /// Creates a new Midi Track
        /// </summary>
        /// <param name="parent"></param>
        public MidiTrack(MidiFile parent)
        {
            this.Parent = parent;
        }

        /// <summary>
        /// Generates a dictionary where the key is the Note's key
        /// and the Value if a sequence of notes in that key
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<Note>> GetNoteSequence()
        {
            Dictionary<int, List<Note>> seq = new Dictionary<int, List<Note>>();

            foreach(ChannelVoice voice in this.Voice)
            {
                if(voice is ChannelNoteOn)
                {
                    ChannelNoteOn noteOn = (ChannelNoteOn)voice;
                    if(!seq.ContainsKey(noteOn.Note)) seq[noteOn.Note] = new List<Note>();
                    seq[noteOn.Note].Add(new Note(this, voice.Time, 0));
                }
                else if(voice is ChannelNoteOff)
                {
                    ChannelNoteOff noteOff = (ChannelNoteOff)voice;
                    seq[noteOff.Note].Last().End = noteOff.Time;
                }
            }

            return seq;
        }

        /// <summary>
        /// Returns the total time in milliseconds for this track
        /// </summary>
        /// <returns></returns>
        public double GetTotalTime()
        {
            return ((double)this.Ticks * this.Parent.Tempo) / (double)(this.Parent.Division * 1000);
        }

        public override string ToString()
        {
            return "Midi Track: " + this.Name;
        }
    }
}
