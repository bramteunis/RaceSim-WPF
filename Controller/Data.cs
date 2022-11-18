using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition? Competitie { get; set; }
        public static Race CurrentRace { get; private set; }
        private static bool ConsoleWritten;

        public static void Initialize(bool console) {
            ConsoleWritten = console;
            Competitie = new Competition();
            Competitie.Participants = new List<IParticipant>();
            Competitie.Tracks = new Queue<Track>();


            VoegDriverToe("Verstappen",TeamColors.Blue);
            VoegDriverToe("Perez", TeamColors.Blue);
            VoegDriverToe("Leclerc", TeamColors.Red);
            foreach (IParticipant driver in Competitie.Participants)
            {
                Competitie.timesBrokenDown.Add(driver, 0);
            }
            /*
            VoegDriverToe("Sainz", TeamColors.Red);
            VoegDriverToe("Djonkovich", TeamColors.Red);
            */
            SectionTypes[] Zandvoort = {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner, };
            VoegTrackToe("Zandvoort", Zandvoort);
            /*
            SectionTypes[] Monza = {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
            };
            VoegTrackToe("Monza", Monza);
            */
            SectionTypes[] Monaco = {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner, };
            VoegTrackToe("Monaco", Monaco);
            SectionTypes[] Spa = { SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.Straight, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner };
            VoegTrackToe("Spa", Spa);

            CurrentRace = new Race(Competitie.Tracks.Dequeue(), Competitie.Participants);        
                }
        public static void VoegDriverToe(String naam, TeamColors kleur) {
            Competitie.Participants.Add(new Driver(naam,kleur));
        }
        public static void VoegTrackToe(string naam, SectionTypes[] sections)
        {
            Competitie.Tracks.Enqueue(new Track(naam, sections));
        }
        public static void NextRace() {
            Track NextTrack = Competitie.Tracks.Dequeue();
            /*
            if (ConsoleWritten) Console.Clear();
            */
            if (NextTrack != null)
            {
                CurrentRace = new Race(NextTrack, Competitie.Participants);
            }
            else
            {
                CurrentRace = null;
                Console.WriteLine("No more races");
            }
                /*
                CurrentRace.Track = Competitie.NextTrack();
                */
            
        }



    }
}
