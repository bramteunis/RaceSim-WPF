using Controller;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using static RaceSimulator.Visualize;

namespace UserInterface
{
    public static class Visualisatie
    {
        private enum Direction
        {
            North, East, South, West
        }

        private static Direction directions;
        private static int  _x,_y ;
        private const int _size = 150;

        private const string AutoBlauw = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\Auto-Blauw.png";
        private const string AutoCyan = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\Auto-Cyan.png";
        private const string AutoGroen = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\Auto-Groen.png";
        private const string AutoOrange = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\Auto-Orange.png";
        private const string Finish = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\Finish.png";
        private const string StartGrid = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\StartGrid.png";
        private const string Straight = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\Straight.png";
        private const string Turn = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\Turn-Right.png";
        private const string TurnLeft = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\Turn.png";
        private const string Decoration = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\Decoration.png";
        private const string Fire = "C:\\Users\\SKIKK\\source\\repos\\RaceSimulator\\UserInterface\\Sections\\Fire.png";

        public static BitmapSource DrawTrack(Model.Track track) {
            if (track != null)
            {
                _x = -1; 
                directions = Direction.East;
                Bitmap emptyImg = Images.MakeBitmap(2000,2000);
                Bitmap trackImg = PlaceSections(track, emptyImg);
                Bitmap trackImgWithDrivers = PlaceDrivers(track, trackImg);
                return Images.CreateBitmapSourceFromGdiBitmap(trackImgWithDrivers);
            }
            return Images.CreateBitmapSourceFromGdiBitmap(Images.MakeBitmap(256, 256));
        }
        private static void RotateDrivers(Bitmap canvas, Direction Richting, int Originalx, int Addedx, int Originaly, int Addedy, Graphics g) {
            switch(Richting){
                case Direction.East:
                    break;
                case Direction.North:
                    canvas.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case Direction.South:
                    canvas.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Direction.West:
                    canvas.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
            }
            var drawpoint = new Point(Originalx * _size + Addedx, Originaly * _size + Addedy);
            g.DrawImage(canvas, drawpoint);
        }
        private static Bitmap GenerateDriver(Bitmap bm, Direction r, int x, int y, Section section)
        {
            // bepaalt locatie van driver op sectie op basis van directie
            int ToevoegenxLeft = 0;
            int ToevoegenyLeft = 0;
            int ToevoegenxRight = 0;
            int ToevoegenyRight = 0;
            switch (r)
            {
                case Direction.North:
                    ToevoegenxRight = 70;
                    ToevoegenyRight = 90;
                    ToevoegenxLeft = 35;
                    ToevoegenyLeft = 40;
                    break;
                case Direction.East:
                    ToevoegenxLeft = 84;
                    ToevoegenyLeft = 40;
                    ToevoegenxRight = 26;
                    ToevoegenyRight = 67;

                    break;
                case Direction.South:
                    ToevoegenxLeft = 70;
                    ToevoegenxLeft = 90;
                    ToevoegenxRight = 35;
                    ToevoegenxRight = 40;
                    break;
                case Direction.West:
                    ToevoegenxRight = 84;
                    ToevoegenyRight = 43;
                    ToevoegenxLeft = 26;
                    ToevoegenyLeft = 70;
                    break;
            }

            IParticipant d1 = Data.CurrentRace?.GetSectionData(section).Left;
            IParticipant d2 = Data.CurrentRace?.GetSectionData(section).Right;
            Graphics g = Graphics.FromImage(bm);
            if (d1 != null && d1.Name != "broken")
            {
                Bitmap d1Bitmap = new Bitmap(Images.LoadBitmap(GetCarColorPath(d1.TeamColor)));
                RotateDrivers(d1Bitmap, r, x, ToevoegenxLeft, y, ToevoegenyLeft, g);
            }if (d2 != null && d2.Name != "broken")
            {
                Bitmap d2Bitmap = new Bitmap(Images.LoadBitmap(GetCarColorPath(d2.TeamColor)));
                RotateDrivers(d2Bitmap, r, x, ToevoegenxRight, y, ToevoegenyRight, g);
            }if (d1 != null && d1.Name == "broken") {
                Bitmap d1Bitmap = new Bitmap(Images.LoadBitmap(Fire));
                RotateDrivers(d1Bitmap, r, x, ToevoegenxLeft, y, ToevoegenyLeft, g);
            }
            if (d2 != null && d2.Name == "broken")
            {
                Bitmap d2Bitmap = new Bitmap(Images.LoadBitmap(Fire));
                RotateDrivers(d2Bitmap, r, x, ToevoegenxRight, y, ToevoegenyRight, g);
            }

            return bm;
        }
        private static Bitmap PlaceDrivers(Model.Track t, Bitmap bm)
        {
            int x = -_x;
            int y = -_y;
            directions = Direction.East;
            // Loop door alle secties zodat drivers worden geplaatst op juiste sectie
            foreach (Section section in t.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.StartGrid:
                        GenerateDriver(bm, directions, x, y, section);
                        break;
                    case SectionTypes.Straight:
                        GenerateDriver(bm, directions, x, y, section);
                        break;
                    case SectionTypes.LeftCorner:
                        GenerateDriver(bm, directions, x, y, section);
                        SetNewDirection(SectionTypes.LeftCorner);
                        break;
                    case SectionTypes.RightCorner:
                        GenerateDriver(bm, directions, x, y, section);
                        SetNewDirection(SectionTypes.RightCorner);
                        break;
                    case SectionTypes.Finish:
                        GenerateDriver(bm, directions, x, y, section);
                        break;
                }

                switch (directions)
                {
                    case Direction.North:
                        y--;
                        break;
                    case Direction.East:
                        x++;
                        break;
                    case Direction.South:
                        y++;
                        break;
                    case Direction.West:
                        x--;
                        break;
                }
            }
            return bm;
        }
        public static string GetCarColorPath(TeamColors color)
        {
            // Linkt de Teamcolors aan de juiste autofiles
            switch (color)
            {
                case TeamColors.Blue:
                    return AutoBlauw;
                case TeamColors.Green:
                    return AutoGroen;
                case TeamColors.Grey:
                    return AutoOrange;
                case TeamColors.Yellow:
                    return AutoCyan;
                case TeamColors.Red:
                    return AutoOrange;
            }
            return "";
        }
        public static Bitmap PlaceSections(Model.Track t, Bitmap bitmap)
        {
            int x = -_x;
            int y = -_y;
            Graphics graphics = Graphics.FromImage(bitmap);
            // For loop zodat het scherm word gevuld met decoratie
            for (int checkx = 0; checkx < 10; checkx++){
                for (int checky = 0; checky < 10; checky++)
                {
                    DrawSection(Images.LoadBitmap(Decoration), directions, checkx, checky, graphics, 360,"decoration");
                }
            }
            // For loop zodat alle secties met bijbehordende afbeeldingen worden geladen
            foreach (Section section in t.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.StartGrid:
                        DrawSection(Images.LoadBitmap(StartGrid), directions, x, y, graphics, 360, "straight");
                        break;
                    case SectionTypes.Straight:
                        DrawSection(Images.LoadBitmap(Straight), directions, x, y, graphics, 360, "straight");
                        break;
                    case SectionTypes.LeftCorner:
                        DrawSection(Images.LoadBitmap(Turn), directions, x, y, graphics,360, "turnleft");
                        SetNewDirection(SectionTypes.LeftCorner);
                        break;
                    case SectionTypes.RightCorner:
                        DrawSection(Images.LoadBitmap(Turn), directions, x, y, graphics,360, "turnright");
                        SetNewDirection(SectionTypes.RightCorner);
                        break;
                    case SectionTypes.Finish:
                        DrawSection(Images.LoadBitmap(Finish), directions, x, y, graphics,360,"straight");
                        break;
                }
                switch (directions)
                {
                    case Direction.North:
                        y--;
                        break;
                    case Direction.East:
                        x++;
                        break;
                    case Direction.South:
                        y++;
                        break;
                    case Direction.West:
                        x--;
                        break;
                }
            }
            directions = Direction.East;
            return bitmap;
        }
        private static void TurnSections(Bitmap bm,int x, int y,Graphics g,int flip) {
            // Deze functie draait de afbeeldingen de aagevraagde waarde door drawsection() en schrijft ze naar het scherm
            switch (flip){
                case 90:
                    bm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 180:
                    bm.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 270:
                    bm.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }
            g.DrawImage(bm, new Point(x * _size, y * _size));
        }
        private static void DrawSection(Bitmap bitmap, Direction r, int x, int y, Graphics g,int flip,string soort)
        {
            Bitmap bm = new(bitmap);
            switch (r)
            {
                // De weg gaat verder in noorderlijke richting
                case Direction.North:
                    // Op basis van het sectietype word de afbeelding gedraaid
                    switch (soort)
                    {
                        case "straight":
                            TurnSections(bm, x, y, g, 90); break;
                        case "turnleft":
                            TurnSections(bm, x, y, g, 180); break;
                        case "turnright":
                            TurnSections(bm, x, y, g, 270); break;
                        case "decoration":
                            TurnSections(bm, x, y, g, 0); break;
                    }
                    break;
                // De weg gaat verder in oostelijke richting
                case Direction.East:
                    // Op basis van het sectietype word de afbeelding gedraaid
                    switch (soort)
                    {
                        case "straight":
                            TurnSections(bm, x, y, g, 0); break;
                        case "turnleft":
                            TurnSections(bm, x, y, g, 90); break;
                        case "turnright":
                            TurnSections(bm, x, y, g, 0); break;
                        case "decoration":
                            TurnSections(bm, x, y, g, 0); break;
                    }
                    break;
                // De weg gaat verder in zuidelijke richting
                case Direction.South:
                    // Op basis van het sectietype word de afbeelding gedraaid
                    switch (soort)
                    {
                        case "straight":
                            TurnSections(bm, x, y, g, 90); break;
                        case "turnleft":
                            TurnSections(bm, x, y, g, 180); break;
                        case "turnright":
                            TurnSections(bm, x, y, g, 90); break;
                        case "decoration":
                            TurnSections(bm, x, y, g, 0); break;
                    }
                    break;
                // De weg gaat verder in westelijke richting
                case Direction.West:
                    // Op basis van het sectietype word de afbeelding gedraaid
                    switch (soort)
                    {
                        case "straight":
                            TurnSections(bm, x, y, g, 0); break;
                        case "turnleft":
                            TurnSections(bm, x, y, g, 270); break;
                        case "turnright":
                            TurnSections(bm, x, y, g, 180); break;
                        case "decoration":
                            TurnSections(bm, x, y, g, 0); break;
                    }
                    break;
            }
        }
        private static void SetNewDirection(SectionTypes sectionType)
        {
            // Op basis van het sectietype (Bocht links of rechts) word de directie aangepast
            switch (sectionType)
            {
                case SectionTypes.LeftCorner:
                    directions = directions == Direction.North ? Direction.West : directions - 1;
                    break;
                case SectionTypes.RightCorner:
                    directions = directions == Direction.West ? Direction.North : directions + 1;
                    break;
            }
        }
    }
}
