using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class NextRaceEventsArgs : EventArgs
    {
        public Track Track { get; set; }
        public NextRaceEventsArgs(Track track)
        {
            Track = track;
        }
    }
}