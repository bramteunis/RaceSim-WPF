using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class TrackTest
    {
        private Track track;
        private string tracknaam = "dubai";
        private SectionTypes[] emptysections = new SectionTypes[1];
        [SetUp]
        public void SetUp()
        {
            track = new Track(tracknaam, emptysections);
        }
        [Test]
        public void NameIsSet()
        {
            var Result = track.naam;
            Assert.AreEqual(Result, tracknaam);
        }
        [Test]
        public void SectionsIsSet()
        {
            var Result = track.Sections;
            Assert.IsNotNull(Result);
        }

    }
}
