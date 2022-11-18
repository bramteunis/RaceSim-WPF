using Controller;
using Model;
using RaceSimulator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private data driverStatisticsWindowScreen;
        private DriverData raceStatisticsWindowScreen;
        public MainWindow()
        {
            Data.Initialize(true);
            Data.CurrentRace.NextRace += NextRaceHandler;
            Data.CurrentRace.DriverChanged += OnDriverChanged;
            InitializeComponent();
            DataContext data = new DataContext();
        }
        public void NextRaceHandler(object s, NextRaceEventsArgs e)
        {
            // Zorgt ervoor dat er een nieuwe race word gestart
            Images.ClearBitmap();
            Data.NextRace();
            Visualisatie.DrawTrack(Data.CurrentRace.Track);
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriverChanged += OnDriverChanged;
                Data.CurrentRace.NextRace += NextRaceHandler;
            }
        }
        public void OnDriverChanged(Object source, DriversChangedEventArgs e)
        {
            // Functie zorgt ervoor dat de baan opnieuw getekend word
            Sectie1.Dispatcher.BeginInvoke(
            DispatcherPriority.Render,
            new Action(() =>
            {
                Sectie1.Source = null;
                Sectie1.Source = Visualisatie.DrawTrack(e.baantjes);
            }));
        }
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            // Sluit de applicatie
            Application.Current.Shutdown();
        }
        private void RaceStatistics(object sender, RoutedEventArgs e)
        {
            // Toont scherm met statestiek over race
            driverStatisticsWindowScreen = new data();
            driverStatisticsWindowScreen.Show();
        }
        private void Driverstatistics(object sender, RoutedEventArgs e)
        {
            // Toont scherm met statestiek over coureur
            raceStatisticsWindowScreen = new DriverData();
            raceStatisticsWindowScreen.Show();
        }
    }
}
