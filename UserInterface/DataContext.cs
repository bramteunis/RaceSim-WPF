using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string TrackName => Data.CurrentRace?.Track.naam;
        public List<string> Equipment => Data.Competitie.Participants.Select(x => $"{x.Name} - Speed: {x.Equipment.Speed}").ToList();
        public List<string> Points => Data.Competitie.points.Select(x => $"{x.Key.Name} - Points: {x.Value}").ToList();
        public string NextTrack => Data.Competitie.Tracks.Count != 0 ? Data.Competitie.Tracks.Peek().naam : "N/A";
        public string StartTime => Data.CurrentRace?.StartTime.ToString("dddd, dd MMMM HH:mm:ss");
        public List<string> TimesBrokenDown => Data.Competitie.timesBrokenDown.Select(x => $"{x.Key.Name} - broken down: {x.Value}x").ToList();
        public string Amount_of_laps => Data.CurrentRace.registerRounds.Values.Max()+ " / " + Data.CurrentRace.rounds.ToString();
        public List<string> LapTimes => Data.Competitie.Participants.Select(x => string.Format("{0,-30}{1}", x.Name +":" ,string.Format("{1:D2}:{2:D2}:{3:D2}",TimeSpan.FromSeconds(x.Equipment.Speed * 0.05 * Data.CurrentRace.Track.Sections.Count).Hours,TimeSpan.FromSeconds(x.Equipment.Speed * 0.05 * Data.CurrentRace.Track.Sections.Count).Minutes,TimeSpan.FromSeconds(x.Equipment.Speed * 0.05 * Data.CurrentRace.Track.Sections.Count).Seconds,TimeSpan.FromSeconds(x.Equipment.Speed * 0.05 * Data.CurrentRace.Track.Sections.Count).Milliseconds))).ToList();

        public DataContext() {
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriverChanged += DriverChangedHandler;
                Data.CurrentRace.NextRace += HandlersClearedhandler;
            }
        }
        public void HandlersClearedhandler(object s, NextRaceEventsArgs e)
        {
            // Zorgt ervoro dat nadat de eventhandlers opgeschoond zijn deze toegevoegd worden
            Data.CurrentRace.DriverChanged += DriverChangedHandler;
            Data.CurrentRace.NextRace += HandlersClearedhandler;
        }
        public void DriverChangedHandler(object s, DriversChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
