using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MidiParser.lib.MidiEvent;

namespace MidiParser.lib
{
    public class MidiFile
    {
        /// <summary>
        /// Division of this file. Ticks per quarter note
        /// </summary>
        public long Division { get { return _division; } private set { _division = value; } }
        /// <summary>
        /// Tempo of this file. us per quarter note
        /// </summary>
        public long Tempo { get { return _tempo; } private set { _tempo = value; } }


        private byte[] _data;
        long _division = 0;
        long _tempo = 500000;
        List<MidiTrack> _tracks = new List<MidiTrack>();
        
        public List<MidiTrack> Tracks
        {
            get { return this._tracks; }
        }

        /// <summary>
        /// Creates a new Midi file
        /// </summary>
        /// <param name="data">Byte data of the midi file</param>
        public MidiFile(byte[] data)
        {
            this._data = data;
            this.ReadFile();
        }

        private void ReadFile()
        {
            //Set division
            byte[] div = { _data[13], _data[12], 0, 0};
            this.Division = BitConverter.ToInt32(div, 0);

            //Scan for tracks to read
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data[i] == 0x4D && _data[i + 1] == 0x54)   //MT
                {
                    //If MTRK
                    if (_data[i + 2] == 0x72 && _data[i + 3] == 0x6B) //rk
                    {
                        //Get Length
                        byte[] lengthb = new byte[4];
                        Array.Copy(_data, i + 4, lengthb, 0, 4);
                        Array.Reverse(lengthb);
                        int length = BitConverter.ToInt32(lengthb, 0);

                        //Parse Track
                        this.ParseTrack(i+8, length);

                        //Advance cursor
                        i += 8 + length -1; //(-1 coz of i++ in loop)
                    }
                }
            }
        }

        /// <summary>
        /// Parse's MTrk Chunk
        /// </summary>
        /// <param name="start"Index where the Chunk starts (excluding MTrk)</param>
        /// <param name="length">Length of the track</param>
        private void ParseTrack(int start, int length)
        {
            MidiTrack midiTrack = new MidiTrack(this);

            int ticks = 0;
            for (int i = start; i < start + length;)
            {
                //Update ticks
                int[] delta = GetDelta(i);
                ticks += MidiDecTime2normalTime(delta);

                //Move cursor
                i += delta.Length;

                if (_data[i] == 0x90 || _data[i] == 0x80)
                {
                    ChannelVoice voice = ChannelVoiceFactory.Create(ticks, _data, i);
                    midiTrack.Voice.Add(voice);

                    i += 3;
                }
                else if (_data[i] == 0xFF)
                {
                    i += ProcessMetaEvent(midiTrack, i);
                }
            }

            //Set total ticks
            midiTrack.Ticks = ticks;

            this._tracks.Add(midiTrack);
        }

        /// <summary>
        /// Process meta events
        /// </summary>
        /// <param name="start">index of event</param>
        /// <returns>Length of event in bytes</returns>
        private int ProcessMetaEvent(MidiTrack midiTrack, int n)
        {
            byte flag = _data[n + 1];
            byte len = 0;

            if (flag == 0x00)                       //FF 00 02 ss ss
            {
                len = 5;
            }
            else if(flag == 0x03)                   //Set Track name
            {
                len = _data[n + 2];
                midiTrack.Name = Encoding.UTF8.GetString(_data, n+3, len);
                len += 3;
            }
            else if (flag >= 0x01 && flag <= 0x07)  //FF nn len txt
            {
                len = _data[n + 2];
                len += 3;
            }
            else if (flag == 0x20)                  //FF 20 01 cc
            {
                len = 5;
            }
            else if (flag == 0x2F)                  //FF 2F 00 (End of track)
            {
                len = 3;
            }
            else if (flag == 0x51)                  //FF 51 03 tt tt tt (Set tempo)
            {
                len = 6;
                byte[] tempo = { 0x00, _data[n + 3], _data[n + 4], _data[n + 5] };
                Array.Reverse(tempo);
                this.Tempo = BitConverter.ToInt32(tempo, 0);
            }
            else if (flag == 0x54)                  //FF 54 05 hh mm ss fr ff
            {
                len = 8;
            }
            else if (flag == 0x58)                  //FF 58 04 nn dd cc bb
            {
                len = 7;
            }
            else if (flag == 0x59)                  //FF 58 04 nn dd cc bb
            {
                len = 7;
            }
            else if (flag == 0x7F)                  //FF 7F <len> <id> <data>
            {
                len = _data[n + 2];
                len += 3;
            }

            return len;
        }

        
        /// <summary>
        /// Gets midi delta time in bytes
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        int[] GetDelta(int start)
        {
            List<int> time = new List<int>();

            do
            {
                time.Add(_data[start]);
            } while ((_data[start++] & 0x80) != 0x0);

            return time.ToArray();
        }

        /// <summary>
        /// Converts Midi time in byte to number of ticks
        /// </summary>
        /// <param name="n">byte array</param>
        /// <returns></returns>
        static int MidiDecTime2normalTime(int[] n)
        {
            int l = n.Length; int t = 0;
            for (int i = 0; i < l - 1; i++)
            {
                t += (int)((n[i] - 128) * Math.Pow(2, 7 * (l - i - 1)));
            }
            t += n[l - 1];
            return t;
        }
    }
}
