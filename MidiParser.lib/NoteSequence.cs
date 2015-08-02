using MidiParser.lib.MidiEvent;
using System.Collections.Generic;
using System.Linq;

namespace MidiParser.lib
{
    public class NoteSequence
    {
        /// <summary>
        /// MidiTrack this Sequence belongs to
        /// </summary>
        public MidiTrack Track { get; private set; }

        /// <summary>
        /// Data structure to hold the Midi Sequence
        /// Key:    Note key
        /// Valye:  List of Notes
        /// </summary>
        private Dictionary<int, List<Note>> _seq = new Dictionary<int, List<Note>>();

        /// <summary>
        /// Creates a Note sequence from the given MidiTrack
        /// </summary>
        /// <param name="track">Midi Track to read</param>
        public NoteSequence(MidiTrack track)
        {
            this.Track = track;
            this.Reload();
        }

        /// <summary>
        /// Scans the voices in the MidiTrack to generate note sequence
        /// </summary>
        public void Reload()
        {
            foreach (ChannelVoice voice in this.Track.Voice)
            {
                if (voice is ChannelNoteOn)
                {
                    ChannelNoteOn noteOn = (ChannelNoteOn)voice;
                    if (!_seq.ContainsKey(noteOn.Note)) _seq[noteOn.Note] = new List<Note>();
                    _seq[noteOn.Note].Add(new Note(this.Track, voice.Time, 0));
                }
                else if (voice is ChannelNoteOff)
                {
                    ChannelNoteOff noteOff = (ChannelNoteOff)voice;
                    _seq[noteOff.Note].Last().End = noteOff.Time;
                }
            }
        }

        /// <summary>
        /// List of Notes
        /// </summary>
        /// <param name="key">Get notes for this key</param>
        /// <returns></returns>
        public List<Note> this[int key]
        {
            get
            {
                return _seq[key];
            }
        }
    }
}
