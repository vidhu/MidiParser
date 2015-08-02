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

        NoteSequence _sequence = null;
        /// <summary>
        /// Sequence of notes in this track
        /// </summary>
        public NoteSequence NoteSequence {
            get
            {
                if (this._sequence == null)
                    _sequence = new NoteSequence(this);
                return this._sequence;
            }
        }

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
