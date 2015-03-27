using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.Capture;
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

        private double[] magReading;
        //private double magReading2;
        private double tempAngle;
        private DispatcherTimer dtimer;

        public GamePlay()
        {
            this.InitializeComponent();
            App.Current.Resuming += Current_Resuming;
            App.BluetoothManager.MessageReceived += Bluetooth_MessageReceived;

        }


        async void Current_Resuming(object sender, object e)
        {
            await RefreshPreview();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            await RefreshPreview();
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



        public async void Bluetooth_MessageReceived(object sender, string message)
        {
            //System.Diagnostics.Debug.WriteLine(message);

            int sIndex = message.IndexOf("S");
            String zoomLevelstr = "";
            if (sIndex > 0)
            {
                zoomLevelstr = message.Substring(sIndex + 1);
                int rawZoomLevel = Convert.ToInt32(zoomLevelstr);
                double zoomLevel = rawZoomLevel / 1000;

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                {
                    ScaleTransform sclTrans = new ScaleTransform();
                    sclTrans.ScaleX = zoomLevel;
                    sclTrans.ScaleY = zoomLevel;
                    capturePreview.RenderTransform = sclTrans;

                });


            }
            else if (dtimer == null)
            {
                dtimer = new DispatcherTimer();
                dtimer.Interval = new TimeSpan(0, 0, 0, 0, 2000);
                dtimer.Tick += dtimer_Tick;
                dtimer.Start();

                if(magReading == null) {
                    magReading = ArduinoController.extractSensorReadings(message);
                }
                else if (Math.Abs(magReading[0] - ArduinoController.extractSensorReadings(message)[0]) < 0.1)
                {
                    state_label.Text = "Shot!";
                }
                else
                {
                    state_label.Text = "Missed!";
                }
            }

        }



        void dtimer_Tick(object sender, object e)
        {
            dtimer = null;

        }


        private async Task RefreshPreview()
        {
            await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
            ApplicationView.GetForCurrentView().SuppressSystemOverlays = true;

            App.MediaCapture = new MediaCapture();
            await App.MediaCapture.InitializeAsync();
            App.MediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
            capturePreview.Source = App.MediaCapture;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            await App.MediaCapture.StartPreviewAsync();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dtimer != null)
            {
                dtimer.Stop();
            }
            dtimer = null;
            magReading = null;
            state_label.Text = "";
        }
    }
}
