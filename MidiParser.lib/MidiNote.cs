namespace MidiParser.lib
{
    public class MidiNote
    {
        /// <summary>
        /// The midi track that contains this note
        /// </summary>
        public MidiTrack Parent { get; private set; }

        /// <summary>
        /// Absolute start time in ticks
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Absolute end tiome in ticks
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// Absolute start time in milliseconds
        /// </summary>
        public double StartMS {
            get {
                return (this.Start * this.Parent.Tempo) / (double)(this.Parent.Division * 1000);
            }
        }

        /// <summary>
        /// Absolute end time in milliseconds
        /// </summary>
        public double EndMS{
            get{
                return (this.End * this.Parent.Tempo) / (double)(this.Parent.Division * 1000);
            }
        }

        /// <summary>
        /// Creates a new note at a specific start and end time
        /// </summary>
        /// <param name="parent">MidiFile this is part of</param>
        /// <param name="start">Absolute start time in ticks</param>
        /// <param name="end">Absolute end time in ticks</param>
        public MidiNote(MidiTrack parent, int start, int end)
        {
            this.Parent = parent;
            this.Start = start;
            this.End = end;
        }
    }
}
