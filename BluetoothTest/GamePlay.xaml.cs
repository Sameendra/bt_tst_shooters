using BluetoothTest.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BluetoothTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePlay : Page
    {
        private DisplayRequest dispRequest;
        private static MediaCapture mediaCapture;
        private bool isPreviewing;

        private CommunicationHandler commHandle;

        private Geolocator geolocator;

        private double[] magReading;
        //private double magReading2;
        private double tempAngle;
        //private DispatcherTimer dtimer;


        private Player player;
        private PlayerList opponents;
        private PlayerList teamMates;

        public GamePlay()
        {
            
            this.InitializeComponent();
            App.Current.Resuming += Current_Resuming;
            App.Current.Suspending += Current_Suspending;
            App.BluetoothManager.MessageReceived += Bluetooth_MessageReceived;
            shotIndicator.onEmptyHealth += shotIndicator_onEmptyHealth;
            dispRequest = null;

            initGeolocator();
            geolocator.PositionChanged += geolocator_PositionChanged;

            commHandle = new CommunicationHandler();
            commHandle.PlayerGetShot += commHandle_PlayerGetShot;
            commHandle.PlayerUpdateRecieved += commHandle_PlayerUpdateRecieved;

            opponents = new PlayerList();
            teamMates = new PlayerList();

            player = new Player("Sameendra", 5);
            player.Shot += player_Shot;

            player.Coordinates.Longitude = 0;
            player.Coordinates.Latitude = 0;
            player.Weapon = new Pistol(gunSoundPlayer);
            ammoIndicator.AmmoLeft = player.Weapon.AmmoLeft;
            player.Weapon.PropertyChanged += Weapon_PropertyChanged;

            

            //Player opponent = new Player("Nimantha", 5);
            //opponent.Coordinates.Longitude = 0;
            //opponent.Coordinates.Latitude = 5.0/111000;
            //opponents.Players.Add(opponent);

            //System.Diagnostics.Debug.WriteLine(player.Coordinates.getAngleInRelationToNorth(opponent.Coordinates));
            
        }


        async void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                player.Coordinates.Latitude = args.Position.Coordinate.Point.Position.Latitude;
                player.Coordinates.Longitude = args.Position.Coordinate.Point.Position.Longitude;
                player.Coordinates.Altitude = args.Position.Coordinate.Point.Position.Altitude;

            });

            commHandle.updatePlayerLocationAsync(player);
        }

        async void commHandle_PlayerUpdateRecieved(object sender, Player player)
        {
            if(!opponents.contains(player)) {

                opponents.Players.Add(player);
                return;
            }

            foreach (Player opponentPlayer in opponents.Players)
            {

                if(opponentPlayer.ConnectionID.Equals(player.ConnectionID)) {

                    lock (opponentPlayer.Coordinates)
                    {
                        opponentPlayer.Coordinates = player.Coordinates;
                    }
                    break;
                }
            } 
        }

        void commHandle_PlayerGetShot()
        {
            shotIndicator.gotShot();
        }

        void Weapon_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ammoIndicator.AmmoLeft = (sender as Weapon).AmmoLeft;
        }

        private void changeWeapon(Weapon weapon)
        {

            player.Weapon = weapon;
            ammoIndicator.DataContext = weapon;
        }

        void player_Shot(object sender, double shootingAngle)
        {
            
            Player shotPlayer = opponents.getTheShotPlayer(sender as Player, shootingAngle);

            if(shotPlayer != null) {

                commHandle.commitShootUpdateAsync(shotPlayer.ConnectionID);
            }
            else
            {
                state_label.Text = "Missed!";
            }
        }

        

        void shotIndicator_onEmptyHealth(object sender)
        {
            dead_status.Text = "You are so DEAD!";
        }


        async void Current_Resuming(object sender, object e)
        {
            await RefreshPreview();

            if (dispRequest == null)
            {

                // Activate a display-required request. If successful, the screen is 
                // guaranteed not to turn off automatically due to user inactivity.
                dispRequest = new DisplayRequest();
                dispRequest.RequestActive();
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            await RefreshPreview();

            if (dispRequest == null)
            {

                // Activate a display-required request. If successful, the screen is 
                // guaranteed not to turn off automatically due to user inactivity.
                dispRequest = new DisplayRequest();
                dispRequest.RequestActive();

            }

            try
            {
                commHandle.initializeConnection(player);
            }
            catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }


            //spinTarget.Begin();


            /*
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {

                Storyboard sb = spinTarget;
                Task.Delay(10000);
                sb.Begin();

            });
              
             */

        }



        public void Bluetooth_MessageReceived(object sender, string message)
        {
            //System.Diagnostics.Debug.WriteLine(message);

            int sIndex = message.IndexOf("S");
            String zoomLevelstr = "";
            if (sIndex > 0)
            {

                zoomLevelstr = message.Substring(sIndex + 1);
                System.Diagnostics.Debug.WriteLine(zoomLevelstr);
                double rawZoomLevel = Convert.ToInt32(zoomLevelstr);
                System.Diagnostics.Debug.WriteLine(rawZoomLevel);
                double zoomLevel = 0;
                if (rawZoomLevel < 1150)
                {
                    zoomLevel = 1;
                }
                else
                {
                    zoomLevel = rawZoomLevel / 1000;
                }

                System.Diagnostics.Debug.WriteLine(zoomLevel);

                //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                //{
                ScaleTransform sclTrans = new ScaleTransform();
                sclTrans.ScaleX = zoomLevel;
                sclTrans.ScaleY = zoomLevel;
                capturePreview.RenderTransform = sclTrans;

                //});


            }

            //else if (magReading == null)
            //{

            //    magReading = ArduinoController.extractSensorReadings(message);
            //    System.Diagnostics.Debug.WriteLine(magReading[0]);

            //}


            else
            {

                //double currentMagReading = ArduinoController.extractSensorReadings(message)[0];
                //System.Diagnostics.Debug.WriteLine("Captured mag Reading :" + magReading[0]);
                //System.Diagnostics.Debug.WriteLine("Current mag Reading :" + currentMagReading);

                //if (Math.Abs(magReading[0] - currentMagReading) < 0.05)
                //{

                //    state_label.Text = "Shot!";

                //    shoot();

                //}
                //else
                //{
                //    state_label.Text = "Missed!";
                //    shoot();
                //}

                player.shoot(ArduinoController.extractSensorReadings(message)[0]);
            }

        }


        


        private async Task RefreshPreview()
        {
            await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
            ApplicationView.GetForCurrentView().SuppressSystemOverlays = true;

            mediaCapture = new MediaCapture();
            await mediaCapture.InitializeAsync();
            mediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
            capturePreview.Source = mediaCapture;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            await mediaCapture.StartPreviewAsync();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            magReading = null;
            state_label.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            shotIndicator.gotShot();
        }


        async void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {

            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            await CleanupCaptureResources();

            deferral.Complete();

            if (dispRequest != null)
            {

                // Deactivate the display request and set the var to null.
                dispRequest.RequestRelease();
                dispRequest = null;
            }
        }


        private void initGeolocator() {
            
            geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;
            geolocator.ReportInterval = 1;
            geolocator.MovementThreshold = 0.1;
        }


        public async Task CleanupCaptureResources()
        {

            if (isPreviewing && mediaCapture != null)
            {
                await mediaCapture.StopPreviewAsync();
                isPreviewing = false;
            }

            if (mediaCapture != null)
            {
                if (capturePreview != null)
                {
                    capturePreview.Source = null;
                }
                mediaCapture.Dispose();
            }
        }


    }
}
