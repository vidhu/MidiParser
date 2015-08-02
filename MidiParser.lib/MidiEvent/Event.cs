using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiParser.lib.MidiEvent
{
    public abstract class Event
    {
        /// <summary>
        /// Absolute time Stamp in ticks
        /// </summary>
        public int Time { get; private set; }

        /// <summary>
        /// Absolute time Stamp in ticks
        /// </summary>
        /// <param name="time"></param>
        public Event(int time)
        {
            this.Time = time;
        }
    }
}
