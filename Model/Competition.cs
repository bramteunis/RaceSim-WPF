using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }
        public Dictionary<IParticipant, int> points { get; set; }
        public Dictionary<IParticipant, int> timesBrokenDown { get; set; }

        public Track NextTrack() {
            try
            {
                Track currentTrack = Tracks.Peek();
                Track track = Tracks.Dequeue();
                if (track == null)
                {
                    return null;
                }
                else
                {
                    return currentTrack;
                }
            }
            catch {
                return null;
            }
        }
        public Competition() {
            timesBrokenDown = new Dictionary<IParticipant, int>();
            points = new Dictionary<IParticipant, int>();
            
        }
    }
}
