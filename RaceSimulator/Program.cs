using Controller;
using Model;
using RaceSimulator;

public class Program
{
    public static void Main(string[] args)
    {


        /*
        Console.Write(Data.CurrentRace.Track.naam);
        */

        
        SectionTypes[] Zandvoort = { 
            SectionTypes.RightCorner,SectionTypes.Finish,SectionTypes.Straight, SectionTypes.StartGrid, SectionTypes.RightCorner
             , SectionTypes.RightCorner, SectionTypes.Straight,SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner };
        Visualize.Initialize();
        


        /*
        Visualize.DrawTrack2(new Track("spa", Zandvoort));


        
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.SetCursorPosition(10,3);
        /*
        Console.WriteLine("Lengte in meter: ");
        double lengte = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Gewicht");
        double gewicht = Convert.ToDouble(Console.ReadLine());
        try
        {
            Console.WriteLine("BMI = " + gewicht / (lengte * lengte));
        }
        catch (Exception ex) { 
            Console.WriteLine("Zorg dat je geldige waardes invoerd!");
        }
        */

        for (; ; )
        {
            Thread.Sleep(100);
        }
        
    }
}