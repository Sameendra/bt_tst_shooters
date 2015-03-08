using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace BluetoothTest
{
    class ArduinoController
    {

        public static void Bluetooth_MessageReceived(object sender, string message)
        {
            System.Diagnostics.Debug.WriteLine(message);

            foreach(double val in extractSensorReadings(message))
            {
                System.Diagnostics.Debug.WriteLine(val);
            }


        }

        public static async void Bluetooth_ExceptionOccured(object sender, Exception ex)
        {
            var md = new MessageDialog(ex.Message, "We've got a problem with bluetooth");
            md.Commands.Add(new UICommand("Okay"));
            md.DefaultCommandIndex = 0;
            var result = await md.ShowAsync();
        }

        public static double[] extractSensorReadings(String message)
        {
            List<double> sensorReadings = new List<double>();

            int equIndex;

            while((equIndex = message.IndexOf("&")) > 0) {
                
                try {
                    sensorReadings.Add(Convert.ToDouble(message.Substring(0,equIndex)));
                }
                catch(FormatException ex) {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                
                message = message.Substring(equIndex+1);
            }


            return sensorReadings.ToArray();
        }
    }
}
