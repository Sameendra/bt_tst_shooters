using Shooters.Bluetooth;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace BluetoothTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<DeviceInformation> deviceList;
        private BluetoothConnectionManager BTConnManager;
        
        
        public MainPage()
        {
            this.InitializeComponent();
            spinTarget.Begin();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            //App.BluetoothManager.MessageReceived += ArduinoController.Bluetooth_MessageReceived;
            App.BluetoothManager.ExceptionOccured += ArduinoController.Bluetooth_ExceptionOccured;


            deviceList = new ObservableCollection<DeviceInformation>();

            // initializing objects

            BTConnManager = App.BluetoothManager;
            
            device_List.ItemsSource = deviceList;



            
             
        }

        private async void refreshDeviceListAsync()
        {
            deviceList.Clear();
            await BTConnManager.EnumerateDevicesAsync(deviceList);

            
        }

     

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            refreshDeviceListAsync();


        }

        private void refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            refreshDeviceListAsync();
        }

        private async void device_List_ItemClick(object sender, ItemClickEventArgs e)
        {
            DeviceInformation selectedDevice = (DeviceInformation)e.ClickedItem;

            await BTConnManager.ConnectToServiceAsync(selectedDevice);

            if (BTConnManager.State == BluetoothConnectionState.Connected)
            {
                MessageDialog msg = new MessageDialog("Connected to device \"" +selectedDevice.Name +"\" successfully", "Connection Successful!");
                msg.Commands.Add(new UICommand("Okay", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                await msg.ShowAsync();
            
            } 
            
            
            
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            Frame.Navigate(typeof(testPage));
            
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePlay));
        }

    }
}
