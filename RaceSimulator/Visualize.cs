using Controller;
using Model;
using System.Runtime.CompilerServices;
using static System.Collections.Specialized.BitVector32;
using Section = Model.Section;

namespace RaceSimulator
{
    public static class Visualize 
    {
        private static Direction _direction { get; set; }
        private static int Yposition=7,Xposition=7,size;
        #region graphics
        private static string[] _finishHorizontal = { 
            "       ",
            "═══════",
            "    1 }",
            "      }",
            " 2    }",
            "═══════",
            "       "};
        private static string[] _finishVertical = {
            " ║+x+║ ",
            " ║x+x║ ",
            " ║   ║ ",
            " ║   ║ ",
            " ║ 12║ ",
            " ║   ║ ",
            " ║   ║ "};
        private static string[] startgrid = { 
            "       ",
            "═══════",
            "    1  ",
            "       ",
            " 2     ",
            "═══════",
            "       " };
        private static string[] startgridVertical = {
            " ║   ║ ",
            " ║   ║ ",
            " ║ 1 ║ ",
            " ║   ║ ",
            " ║   ║ ",
            " ║ 2 ║ ",
            " ║   ║ "};
        private static string[] turnDownRight = { 
            "       ",
            "═════╗ ",
            "     ║ ",
            "  2  ║ ",
            "     ║ ",
            "═╗ 1 ║ ",
            " ║   ║ " };
        /*
        private static string[] turnDownLeft = turn_left(turnDownRight);
        */
        private static string[] turnDownLeft = {
            "       ",
            " ╔═════",
            " ║     ",
            " ║  1  ",
            " ║     ",
            " ║ 2 ╔═",
            " ║   ║ " };
        private static string[] turnRightHorizontal = {
            " ║   ║ ",
            "═╝ 2 ║ ",
            "     ║ ",
            "  1  ║ ",
            "     ║ ",
            "═════╝ ", 
            "       " };
        /*
        private static string[] turnRightUp = turn_left(turnRightHorizontal);
        */
        private static string[] turnRightUp = {
            " ║   ║ ",
            " ║   ╚═",
            " ║ 1   ",
            " ║  2  ",
            " ║     ",
            " ╚═════",
            "       " };
        private static string[] straight = {
            "       ",
            "═══════",
            "    2  ",
            "       ",
            " 1     ",
            "═══════",
            "       " };
        private static string[] straightVertical = {
            " ║   ║ ",
            " ║   ║ ",
            " ║1  ║ ",
            " ║   ║ ",
            " ║   ║ ",
            " ║  2║ ",
            " ║   ║ "};
        private static string[] straightVerticalReverse = {
            " ║   ║ ",
            " ║   ║ ",
            " ║2  ║ ",
            " ║   ║ ",
            " ║   ║ ",
            " ║  1║ ",
            " ║   ║ "};
        public static string[] turn_left(string[] rightTurn) {
            String[] result = new String[7];
            int counter = 0;
            foreach (string lijn in rightTurn)
            {
                result[counter] = Reverse(lijn);
                counter++;
            }
            return result;
        }

        #endregion
        public enum Direction
        {
            North, East, South, West
        }
        public static void OnDriverChanged(Object source, DriversChangedEventArgs e)
        {
            Console.Clear();
            DrawTrack3(e.baantjes);
        }
        public static string Reverse(string Input)
        {
            char[] charArray = Input.ToCharArray();
            string reversedString = String.Empty;
            for (int i = charArray.Length - 1; i > -1; i--)
            {
                reversedString += charArray[i];
            }
            return reversedString;
        }
        public static string placeDrivers(String lijn, IParticipant coureur1, IParticipant coureur2) {
            string nieuwe_lijn;
            if (coureur1 != null && coureur2 !=null)
            {
                nieuwe_lijn = lijn.Replace("1", coureur1.Name[0].ToString()).Replace("2", coureur2.Name[0].ToString());
            }
            else { 
                nieuwe_lijn = lijn.Replace("1"," ").Replace("2", coureur2.Name[0].ToString());
            }
            return nieuwe_lijn;
        }
        public static string placeDrivers(String lijn, IParticipant coureur1)
        {
            string nieuwe_lijn = lijn.Replace("1", coureur1.Name[0].ToString());
            return nieuwe_lijn;
        }
        public static void DrawSectionToConsole(string[] SectionString, Section sectie) {
            Yposition -= size;
            foreach (string SectionRow in SectionString) { 
                string NewSectionRow = SectionRow;
                if (Data.CurrentRace.GetSectionData(sectie) != null && Data.CurrentRace.GetSectionData(sectie).Right != null)
                {
                    NewSectionRow = placeDrivers(SectionRow, Data.CurrentRace.GetSectionData(sectie).Left, Data.CurrentRace.GetSectionData(sectie).Right);
                }
                else if (Data.CurrentRace.GetSectionData(sectie) != null && Data.CurrentRace.GetSectionData(sectie).Left != null)
                {
                    NewSectionRow = placeDrivers(SectionRow, Data.CurrentRace.GetSectionData(sectie).Left);
                    NewSectionRow = NewSectionRow.Replace("[2]", "   ").Replace("2", " ");
                }
                else
                {
                    NewSectionRow = SectionRow.Replace("[1]", "   ").Replace("[2]", "   ").Replace("1", " ").Replace("2", " ");
                }
                try
                {
                    Console.SetCursorPosition(Xposition, Yposition);
                }
                catch { }
                Console.WriteLine(NewSectionRow);
                Yposition++;
            }
        }
        public static void DrawTrack3(Track baantje)
        {
            Xposition = 7;Yposition = 7;
            foreach (Section section in baantje.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.StartGrid:
                        if (_direction == Direction.East || _direction == Direction.West)
                            DrawSectionToConsole(startgrid, section);
                        else
                            DrawSectionToConsole(startgridVertical, section);
                        break;
                    case SectionTypes.Straight:
                        if (_direction == Direction.East || _direction == Direction.West)
                            DrawSectionToConsole(straight, section);
                        else if(_direction == Direction.South)
                            DrawSectionToConsole(straightVerticalReverse, section);
                        else
                            DrawSectionToConsole(straightVertical, section);
                        break;
                    case SectionTypes.LeftCorner:
                        if (_direction == Direction.North)
                            DrawSectionToConsole(turnDownRight, section);
                        else if (_direction == Direction.East)
                            DrawSectionToConsole(turnRightHorizontal, section);
                        else if (_direction == Direction.South)
                            DrawSectionToConsole(turnRightUp, section);
                        else
                            DrawSectionToConsole(turnDownLeft, section);
                        _direction = SetNewDirection(SectionTypes.LeftCorner, _direction);
                        break;
                    case SectionTypes.RightCorner:
                        if (_direction == Direction.North)
                            DrawSectionToConsole(turnDownLeft, section);
                        else if (_direction == Direction.East)
                            DrawSectionToConsole(turnDownRight, section);
                        else if (_direction == Direction.South)
                            DrawSectionToConsole(turnRightHorizontal, section);
                        else
                            DrawSectionToConsole(turnRightUp, section);
                        _direction = SetNewDirection(SectionTypes.RightCorner, _direction);
                        break;
                    case SectionTypes.Finish:
                        if (_direction == Direction.East || _direction == Direction.West)
                            DrawSectionToConsole(_finishHorizontal, section);
                        else
                            DrawSectionToConsole(_finishVertical, section);
                        break;
                }

                switch (_direction)
                {
                    case Direction.North:
                        Yposition -= size;
                        break;
                    case Direction.East:
                        Xposition += size;
                        break;
                    case Direction.South:
                        Yposition += size;
                        break;
                    case Direction.West:
                        Xposition -= size;
                        break;
                }
                try
                {
                    Console.SetCursorPosition(Xposition, Yposition);
                }
                catch { }
            }
            Console.WriteLine(baantje.naam + " | "+ Data.CurrentRace.registerRounds.First().Value + " / " + Data.CurrentRace.rounds);
            Console.SetCursorPosition(0, 21);
            Console.WriteLine("Uitgevallen Coureurs: ");
            foreach (IParticipant coureur in Data.CurrentRace.Participants) {
                if (coureur.Equipment.IsBroken) { 
                    Console.WriteLine(coureur.Name);
                }
            }
        }

            public static Direction SetNewDirection(SectionTypes sectionType, Direction r)
        {
            switch (sectionType)
            {
                case SectionTypes.LeftCorner:
                    return r == Direction.North ? Direction.West : r - 1;
                case SectionTypes.RightCorner:
                    return r == Direction.West ? Direction.North : r + 1;
            }
            return Direction.East;
        }
        public static void NextRaceHandler(object s, NextRaceEventsArgs e)
        {
            Data.NextRace();
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.NextRace += NextRaceHandler;
                Data.CurrentRace.DriverChanged += (s, DriversChangedEventArgs) => OnDriverChanged(s, (DriversChangedEventArgs)DriversChangedEventArgs);

                DrawTrack3(Data.CurrentRace.Track);
            }
        }
        public static void Initialize() {
            _direction = Direction.East;
            Data.Initialize(true);
            size = 7;
            /*
            Data.NextRace();
            */
            Data.CurrentRace.NextRace += NextRaceHandler;
            Data.CurrentRace.DriverChanged += (s, DriversChangedEventArgs) => OnDriverChanged(s, (DriversChangedEventArgs)DriversChangedEventArgs);

            DrawTrack3(Data.CurrentRace.Track);
            /*
            for (int i = 0; i < Data.CurrentRace.Participants.Count; i++) { 
                Console.WriteLine(Data.CurrentRace.Participants[i].Name[0] + " = " + Data.CurrentRace.Participants[i].Name);
            }
            */
            /*
            while (Data.CurrentRace.gehaald == 0) {
                Console.WriteLine("Nog niet gehaald");
            }
            */

            /*
            Data.CurrentRace.OnClick();
            */
            /*
            Console.WriteLine(Data.CurrentRace.GetSectionData(Data.CurrentRace.Track.Sections.ElementAt(4)).Left.Name[0].ToString() + " = " + Data.CurrentRace.GetSectionData(Data.CurrentRace.Track.Sections.ElementAt(4)).Left.Name);
            Console.WriteLine(Data.CurrentRace.GetSectionData(Data.CurrentRace.Track.Sections.ElementAt(4)).Right.Name[0].ToString() + " = " + Data.CurrentRace.GetSectionData(Data.CurrentRace.Track.Sections.ElementAt(4)).Right.Name);
            Console.WriteLine(Data.CurrentRace.GetSectionData(Data.CurrentRace.Track.Sections.ElementAt(5)).Left.Name[0].ToString() + " = " + Data.CurrentRace.GetSectionData(Data.CurrentRace.Track.Sections.ElementAt(5)).Left.Name);
            Console.WriteLine(Data.CurrentRace.GetSectionData(Data.CurrentRace.Track.Sections.ElementAt(5)).Right.Name[0].ToString()+ " = " + Data.CurrentRace.GetSectionData(Data.CurrentRace.Track.Sections.ElementAt(5)).Right.Name);
            */
        }

    }
}
