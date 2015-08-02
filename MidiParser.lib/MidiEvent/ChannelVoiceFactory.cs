using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiParser.lib.MidiEvent
{
    /// <summary>
    /// Factory Class to create a Voice based on given data
    /// </summary>
    public class ChannelVoiceFactory
    {
        /// <summary>
        /// Creates a new ChannelVoice
        /// </summary>
        /// <param name="time">Absolute time in ticks</param>
        /// <param name="data">Byte array containing the MIDI Voice</param>
        /// <param name="index">Index where the voice starts</param>
        /// <returns></returns>
        public static ChannelVoice Create(int time, byte[] data, int index)
        {
            ChannelVoice voice = null;

            int type = data[index] & 0xF0;
            int channel = data[index] & 0x0F;

            switch (type)
            {
                case 0x80:
                    voice = new ChannelNoteOff(time, channel, data[index + 1], data[index + 2]);
                    break;
                case 0x90:
                    voice = new ChannelNoteOn(time, channel, data[index + 1], data[index + 2]);
                    break;
            }

            return voice;
        }
    }
}
