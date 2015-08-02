using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiParser.lib.MidiEvent
{
    /// <summary>
    /// Abstract Voice class
    /// </summary>
    public abstract class ChannelVoice : Event
    {
        public int Channel { get; private set; }

        public ChannelVoice(int time, int channel)
            : base(time)
        {
            this.Channel = channel;
        }
    }
}
