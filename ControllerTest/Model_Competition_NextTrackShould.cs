using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;
        [SetUp]
        public void SetUp() {
            _competition = new Competition();
        }
        [Test]
        public void NextTrack_EmptyQueue_ReturnNull() {
            var Result = _competition.NextTrack();
            Assert.IsNull(Result);
        }
        [Test]
        public void NextTrack_OneInQueue_ReturnTrack() {
            _competition.Tracks = new Queue<Track>();
            SectionTypes[] Spa = { SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Finish };
            Track zandvoort = new Track("Zandvoort", Spa);
            _competition.Tracks.Enqueue(zandvoort);
            Track result = _competition.NextTrack();
            Assert.AreEqual(result, zandvoort);

        }
        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue() {
            _competition.Tracks = new Queue<Track>();
            SectionTypes[] Spa = { SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Finish };
            Track zandvoort = new Track("Zandvoort", Spa);
            _competition.Tracks.Enqueue(zandvoort);
            _competition.NextTrack();
            Track result = _competition.NextTrack();
            Assert.AreEqual(result, null);
        }
        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack() {
            _competition.Tracks = new Queue<Track>();
            SectionTypes[] Spa = { SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Finish };
            Track zandvoort = new Track("Zandvoort", Spa);
            _competition.Tracks.Enqueue(zandvoort);
            Track spa = new Track("Spa", Spa);
            _competition.Tracks.Enqueue(spa);
            _competition.NextTrack();
            Track result = _competition.NextTrack();
            Assert.AreEqual(result, spa);
        }
    }
}
