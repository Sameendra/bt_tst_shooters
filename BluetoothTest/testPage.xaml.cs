using BluetoothTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BluetoothTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class testPage : Page
    {
        private Geolocator geolocator;
        private PlayerList nearByPlayerList;
        private Player player;

        public testPage()
        {
            this.InitializeComponent();

            this.nearByPlayerList = App.PlayerList;
            this.player = App.Player;
            //App.BluetoothManager.MessageReceived += Bluetooth_MessageReceived;

            
        }

        public void Bluetooth_MessageReceived(object sender, string message)
        {
            System.Diagnostics.Debug.WriteLine(message);

            Player shotPlayer;

            if((shotPlayer = this.nearByPlayerList.getTheShotPlayer(player, ArduinoController.extractSensorReadings(message)[0])) != null) {

                status_text.Text = "Shot!";
            }
            else {
                status_text.Text = "Missed!";
            }

        }
        
        

        private async void simulateCordinateChangeAsync() {

            for (int x = 0; x < 1000; x++)
            {
                player.Coordinates.Latitude = x;
                player.Coordinates.Longitude = x;
                player.Coordinates.Altitude = x;

                await Task.Delay(100);
            } 
        }
            
        
        
    
         
        void Coordinates_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(this.player.Coordinates.Altitude);
            System.Diagnostics.Debug.WriteLine(this.player.Coordinates.Longitude);
            System.Diagnostics.Debug.WriteLine(this.player.Coordinates.Latitude);
        }
         


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            player = new Player("Sameendra", 5);
            player.Coordinates.PropertyChanged += Coordinates_PropertyChanged;

            altitudeText.DataContext = player.Coordinates;
            latitudeText.DataContext = player.Coordinates;
            longitudeText.DataContext = player.Coordinates;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //simulateCordinateChangeAsync();

            geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;
            geolocator.ReportInterval = 1;
            geolocator.MovementThreshold = 0.1;
            //geolocator.DesiredAccuracyInMeters = 1;
            

            try
            {
                //Geoposition geoposition = await geolocator.GetGeopositionAsync();
                //player.Coordinates.Altitude = geoposition.Coordinate.Point.Position.Altitude;
                //player.Coordinates.Latitude = geoposition.Coordinate.Point.Position.Latitude;
                //player.Coordinates.Longitude = geoposition.Coordinate.Point.Position.Longitude;


                geolocator.PositionChanged += GeoPositionChanged_EventHandler;
      

                //With this 2 lines of code, the app is able to write on a Text Label the Latitude and the Longitude, given by {{Icode|geoposition}}
                //geolocation.Text = "GPS:" + geoposition.Coordinate.Latitude.ToString("0.00") + ", " + geoposition.Coordinate.Longitude.ToString("0.00");
            }
            //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest.
            //The second is that the user doesn't turned on the Location Services
            catch (Exception ex)
            {
                //exception
            } 
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            player.Coordinates.unSubscribePropertyChanged();
            nearByPlayerList.Players.Clear();
            nearByPlayerList.Players.Add(player);
            this.player = new Player("Nimantha", 5);
            App.Player = this.player;
            player.Coordinates.PropertyChanged += Coordinates_PropertyChanged;
            
        }

        private async void GeoPositionChanged_EventHandler(Geolocator sender, PositionChangedEventArgs args)
        {

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                player.Coordinates.Latitude = args.Position.Coordinate.Point.Position.Latitude;
                player.Coordinates.Longitude = args.Position.Coordinate.Point.Position.Longitude;
                player.Coordinates.Altitude = args.Position.Coordinate.Point.Position.Altitude;

            });

        }
        

        
    }
}
