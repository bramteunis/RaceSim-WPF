using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    public class RaceTest
    {
        private Race race;
        private Track track;

        [SetUp]
        public void SetUp()
        {
            race = new Race(new Track("dubai",new SectionTypes[1]),new List<IParticipant>());
            Data.Competitie = new Competition();
            Data.Competitie.Participants = new List<IParticipant>();
            Data.Competitie.Tracks = new Queue<Track>();
            Data.Initialize(false);
        }
        [Test]
        public void TrackNotNull()
        {
            var Result = race.Track;
            Assert.IsNotNull(Result);
        }
        [Test]
        public void registerRoundsNotNull()
        {
            var Result = race.registerRounds;
            Assert.IsNotNull(Result);
        }
        [Test]
        public void AddPointsToDriver()
        {
            Data.Competitie = new Competition();
            Data.Competitie.Participants = new List<IParticipant>();
            Data.Competitie.Tracks = new Queue<Track>();

            race.AddPointsToDriver(new Driver("test", TeamColors.Green),1);
            Assert.IsNotNull(Data.Competitie.points);
        }
        [Test]
        public void RandomizeEquipment()
        {
            race.Participants = new List<IParticipant>();
            race.Participants.Add(new Driver("test",TeamColors.Blue));
            race.RandomizeEquipment();
            Assert.IsNotNull(race.Participants[0].Equipment);
        }

        [Test]
        public void CheckForMultipleFinish()
        {
            
            Data.Competitie.Participants.Add(new Driver("test", TeamColors.Green));
            Data.Competitie.Participants.Add(new Driver("test2", TeamColors.Green));
            SectionTypes[] Monaco = {
                SectionTypes.StartGrid,
                SectionTypes.Finish,};
            track = new Track("Zandvoort", Monaco);
            Data.Competitie.Tracks.Enqueue(track);

            Driver japie = new Driver("test", TeamColors.Green);
            Data.Competitie.points = new Dictionary<IParticipant, int>();
            race.AddPointsToDriver(japie, 1);
            race.AddPointsToDriver(japie, 1);
            Assert.AreNotEqual(Data.Competitie.points.Count,2);
        }
        [Test]
        public void CheckIfStartgridExists() {
            SectionTypes[] Monaco = {
                SectionTypes.Straight,
                SectionTypes.Finish,};
            track = new Track("Zandvoort", Monaco);
            Race race2 = new Race(track, new List<IParticipant>());
            Assert.IsFalse(race2.PlaceDrivers());
        }
        [Test]
        public void CheckIfFinishExists()
        {
            SectionTypes[] Monaco = {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,};
            track = new Track("Zandvoort", Monaco);
            Race race2 = new Race(track, new List<IParticipant>());
            Assert.IsFalse(race2.PlaceDrivers());
        }
        [Test]
        public void PlaceMoreDriversThanPossible() {
            Data.Competitie.Participants.Add(new Driver("test", TeamColors.Green));
            Data.Competitie.Participants.Add(new Driver("test2", TeamColors.Green));
            Data.Competitie.Participants.Add(new Driver("test3", TeamColors.Green));
            Data.Competitie.Participants.Add(new Driver("test4", TeamColors.Green));
            Data.Competitie.Participants.Add(new Driver("test5", TeamColors.Green));
            Data.Competitie.Participants.Add(new Driver("test6", TeamColors.Green));
            Data.Competitie.Participants.Add(new Driver("test7", TeamColors.Green));
            race.PlaceDrivers();
            Assert.AreEqual(race._positions.Select(x => x.Value.Left != null).Count() + race._positions.Select(x => x.Value.Right != null).Count(), 2);
        }
        public void RoundsAreMade() {
            race = new Race(new Track("dubai", new SectionTypes[1]), new List<IParticipant>());

            Console.WriteLine(race.registerRounds);

        }
        [Test]
        public void Race_GetSectionData_ShouldReturnObject()
        {
            Section section = track.Sections.First?.Value;

            var result = race.GetSectionData(section);

            Assert.IsInstanceOf<SectionData>(result);
            Assert.IsNotNull(result);
        }
        [Test]
        public void Race_RandomizeEquipment()
        {
            Data.Competitie.Participants.Add(new Driver("test", TeamColors.Green));
            Data.Competitie.Participants.Add(new Driver("test2", TeamColors.Green));
            Data.Competitie.Participants.Add(new Driver("test3", TeamColors.Green));
            Data.Competitie.Participants[0].Equipment.Quality = 32;
            Data.Competitie.Participants[0].Equipment.Performance = 32;
            
            Data.CurrentRace.RandomizeEquipment();
            var resultQuality = Data.Competitie.Participants[0].Equipment.Quality;
            var resultPerformance = Data.Competitie.Participants[0].Equipment.Performance;

            Assert.GreaterOrEqual(resultQuality, 8);
            Assert.LessOrEqual(resultQuality, 20);
            Assert.GreaterOrEqual(resultPerformance, 5);
            Assert.LessOrEqual(resultPerformance, 15);
        }
    }
}
