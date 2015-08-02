using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiParser.lib.MidiEvent
{
    /// <summary>
    /// Voice when a key is pressed or a note turns on
    /// </summary>
    public class ChannelNoteOn : ChannelVoice
    {
        /// <summary>
        /// Note number (tone)
        /// </summary>
        public byte Note { get; private set; }

        /// <summary>
        /// Velocity of this Note
        /// </summary>
        public byte Velocity { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="time">Absolute time in ticks</param>
        /// <param name="channel">Channel this occures in</param>
        /// <param name="note">Note number</param>
        /// <param name="velocity">Note velocity</param>
        public ChannelNoteOn(int time, int channel, byte note, byte velocity)
            :base(time, channel)
        {
            this.Note = note;
            this.Velocity = velocity;
        }
    }
}
