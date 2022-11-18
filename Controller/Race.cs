using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;


namespace Controller
{
    public class Race
    {
        public Track Track { get; set; } = new Track();
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        private Random _random;
        public Dictionary<Section, SectionData> _positions { get; private set; }
        private System.Timers.Timer Rondetijd;
        public event EventHandler<DriversChangedEventArgs> DriverChanged;
        public event EventHandler<NextRaceEventsArgs> NextRace;
        public int gehaald = 0;
        delegate bool FinishedTrue(Section section);
        public Dictionary<IParticipant, int> registerRounds ;
        public Dictionary<IParticipant, int> Roundtimes;

        public int rounds { get; set; }
        private Dictionary<IParticipant, int> finishingposition = new Dictionary<IParticipant, int>();
        private IParticipant? DriverRight = null;
        public void CleanEventhandlers()
        {
            if (DriverChanged.GetInvocationList() != null)
            {
                foreach (var handler in DriverChanged.GetInvocationList())
                {
                    DriverChanged -= (EventHandler<DriversChangedEventArgs>)handler;
                }
            }
        }
        public IParticipant CheckIfDriverBroken(IParticipant driver) {
            if (driver.Name == "broken") {
                return null;
            }
            if (driver.Equipment.IsBroken)
            {
                Data.Competitie.timesBrokenDown[driver] += 1;
                
                return new Driver("broken",TeamColors.Grey);
            }
            else { 
                return driver;
            }
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e, Track track, Dictionary<Section, SectionData> _positions, List<IParticipant> Participants)
        {
            void LoadPositions(){
                var FilterBrokenCars = Participants.Where(p => p.Equipment.IsBroken == false);
                /*
                OnClick();
                */
                if (FilterBrokenCars.Count() == 0)
                {
                    Random rnd = new Random();
                    int r = rnd.Next(Participants.Count);
                    Participants[r].Equipment.IsBroken = false;
                    Participants[r].Equipment.Speed = (int)(Participants[r].Equipment.Speed/0.75);
                    PlaceDrivers();
                }
                foreach (KeyValuePair<Section, SectionData> entry in _positions)
                {
                    try
                    {
                        if (entry.Key.SectionType == SectionTypes.Finish && entry.Value.Left != null) {

                            if (registerRounds[entry.Value.Left] == rounds)
                            {
                                if (!finishingposition.ContainsKey(entry.Value.Left)) {
                                    finishingposition.Add(entry.Value.Left, finishingposition.Values.Max() + 1);
                                    AddPointsToDriver(entry.Value.Left, finishingposition[entry.Value.Left]);
                                    entry.Value.Left = null;
                                    if (finishingposition.Count - 1 == Participants.Where(x => x.Equipment.IsBroken == false).Count())
                                    {
                                        CleanEventhandlers();
                                        try
                                        {
                                            Stop();
                                        }
                                        catch { 
                                        }
                                    }
                                }
                            }
                            else {
                                registerRounds[entry.Value.Left] += 1;
                            }
                        }

                        IParticipant? gebruiker = entry.Value.Left;
                        //Alleen links coureur en geen in de wachtrij
                        if (entry.Value.Right == null && entry.Value.Left != null && DriverRight == null)
                        {
                            DriverRight = CheckIfDriverBroken(entry.Value.Left);
                            entry.Value.Left = null;
                        }
                        //Alleen links coureur en een in de wachtrij
                        else if (entry.Value.Right == null && entry.Value.Left != null && DriverRight != null)
                        {
                            DriverRight = CheckIfDriverBroken(entry.Value.Left);
                            entry.Value.Left = null;
                            entry.Value.Right = CheckIfDriverBroken(DriverRight);
                            DriverRight = null;
                        }
                        //Links en Rechts coureur en een in de wachtrij
                        else if (entry.Value.Right != null && entry.Value.Left != null && DriverRight != null)
                        {
                            IParticipant currentDriverLeft = CheckIfDriverBroken(entry.Value.Left);
                            entry.Value.Left = CheckIfDriverBroken(entry.Value.Right);
                            entry.Value.Right = CheckIfDriverBroken(DriverRight);
                            DriverRight = currentDriverLeft;
                        }
                        //Links en Rechts coureur en geen in de wachtrij
                        else if (entry.Value.Right != null && entry.Value.Left != null && DriverRight == null)
                        {
                            IParticipant currentDriverLeft = CheckIfDriverBroken(entry.Value.Left);
                            entry.Value.Left = CheckIfDriverBroken(entry.Value.Right);
                            entry.Value.Right = null;
                            DriverRight = currentDriverLeft;
                        }
                        //Alleen rechts coureur en geen in de wachtrij
                        else if (entry.Value.Right != null && entry.Value.Left == null && DriverRight == null)
                        {
                            entry.Value.Left = CheckIfDriverBroken(entry.Value.Right);
                            entry.Value.Right = null;
                        }
                        //Alleen rechts coureur en een in de wachtrij
                        else if (entry.Value.Right != null && entry.Value.Left == null && DriverRight != null)
                        {
                            entry.Value.Left = CheckIfDriverBroken(entry.Value.Right);
                            entry.Value.Right = CheckIfDriverBroken(DriverRight);
                            DriverRight = null;
                        }
                        else if(entry.Value.Right == null && entry.Value.Left == null && DriverRight != null)
                        {
                            entry.Value.Right = CheckIfDriverBroken(DriverRight);
                            DriverRight = null;
                        }
                        gehaald = 1;
                    }
                    catch (Exception b){ }
                }
                foreach (IParticipant coureur in Participants) {
                    if (!coureur.Equipment.IsBroken) { 
                        Random rand = new Random();
                        if (rand.Next(1, coureur.Equipment.Quality*6) < 2) { 
                            coureur.Equipment.IsBroken = true;

                        }
                    }
                }
            }
            LoadPositions();
            if (DriverRight != null) {
                LoadPositions();
            }
            OnClick();
        }
        public void Start() {
            Rondetijd.Enabled = true;
        }
        public void Stop()
        {
            Rondetijd.Enabled = false;
            Rondetijd.Stop();
            NextRace?.Invoke(this, new NextRaceEventsArgs(Track));
        }
        public SectionData GetSectionData(Section sectie) {
            SectionData returnstring = new SectionData();
            foreach (KeyValuePair<Section, SectionData> kvp in _positions)
            {
                if (kvp.Key == sectie) {
                    returnstring =  kvp.Value;
                }
            }
            return returnstring;
        }
        public void OnClick() {
            DriversChangedEventArgs args = new DriversChangedEventArgs();
            args.baantjes = Track;
            DriverChanged?.Invoke(this, args);
        }
        public Race(Track track, List<IParticipant> participants)
        {   
            Random RandomRound = new Random();
            rounds = RandomRound.Next(2,5);
            registerRounds = new Dictionary<IParticipant, int>();
            finishingposition.Add(new Driver("dummydriver",TeamColors.Grey), 0);
            foreach (IParticipant coureur in participants) {
                registerRounds.Add(coureur, 0);
                /*
                Data.Competitie.points[coureur] =0;
                */
            }
            _positions = new Dictionary<Section, SectionData>();
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            PlaceDrivers();
            Rondetijd = new System.Timers.Timer(500);
            Rondetijd.Elapsed += (sender, e) => OnTimedEvent(sender, e, track,_positions, Participants);
            Rondetijd.Interval = 500;
            Start();
        }
        public void RandomizeEquipment() {
            foreach (IParticipant auto in Participants) { 
                auto.Equipment.Quality = _random.Next(8,20);
                auto.Equipment.Performance = _random.Next(5,15);
            }
        }

        public void AddPointsToDriver(IParticipant driver, int positie)
        {
            if (Data.Competitie.points.ContainsKey(driver))
            {
                Data.Competitie.points[driver] += 10 - positie;
            }
            else
            {
                Data.Competitie.points.Add(driver, 10 - positie);
            }
        }
        public bool PlaceDrivers() {
            int containsFinish = Track.Sections.Where(x => x.SectionType == SectionTypes.Finish).Count();
            int containsStartGrid = Track.Sections.Where(x => x.SectionType == SectionTypes.StartGrid).Count();
            
            int counter_for_place_in_track = 0;
            int counter_drivernumber = 0;
            int Counter_For_SectionNumber = 0;
            int ForeachCounter = 0;
            SectionData[] TrackPlaces = new SectionData[Track.Sections.Count];
            /*
            List<SectionData> TrackPlaces = new List<SectionData>();
            */
            foreach (Section sectie in Track.Sections) {
                if (sectie.SectionType == SectionTypes.Finish) {
                    double amount_of_start_places_needed = counter_for_place_in_track - Participants.Count / 2;
                    double start = Math.Floor(amount_of_start_places_needed);
                        for (int i = counter_for_place_in_track; i >= 0; i--)
                        {
                        if (i == counter_for_place_in_track) {
                        }
                            try
                            {
                                SectionData Opstelling = new SectionData();
                                if (!Participants[counter_drivernumber].Equipment.IsBroken) {
                                    Opstelling.Left = Participants[counter_drivernumber];
                                }
                                if (!Participants[counter_drivernumber+1].Equipment.IsBroken)
                                {
                                    Opstelling.Right = Participants[counter_drivernumber+1];
                                }
                                Opstelling.DistanceLeft = 2;
                                Opstelling.DistanceRight = 2;
                                TrackPlaces[i] = Opstelling;
                            }
                            catch{
                                try
                                {
                                    SectionData Opstelling = new SectionData();
                                    if (!Participants[counter_drivernumber].Equipment.IsBroken)
                                    {
                                        Opstelling.Left = Participants[counter_drivernumber];
                                    }
                                    Opstelling.DistanceLeft = 2;
                                    TrackPlaces[i] = Opstelling;
                                }
                                catch{}
                            }
                        counter_drivernumber++;
                        counter_drivernumber++;
                        }
                }
                if (TrackPlaces[Counter_For_SectionNumber] == null) {
                    TrackPlaces[Counter_For_SectionNumber] = new SectionData();
                }

                counter_for_place_in_track++;
                Counter_For_SectionNumber++;

            }
            
            foreach (SectionData Opstelling in TrackPlaces)
            {
                if (Opstelling != null && !_positions.ContainsKey(Track.Sections.ElementAt(ForeachCounter)))
                {
                    _positions.Add(Track.Sections.ElementAt(ForeachCounter), Opstelling);
                    ForeachCounter++;
                }
                else if (Opstelling != null)
                {
                    _positions[Track.Sections.ElementAt(ForeachCounter)] = Opstelling;
                    ForeachCounter++;
                }
                else
                {
                    _positions.Add(Track.Sections.ElementAt(ForeachCounter), new SectionData());
                    ForeachCounter++;
                }
            }

            if (containsFinish == 0 || containsStartGrid == 0)
            {
                return false;
            }
            else { return true; }

        }
    }
}
