using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothTest
{
    class GameController
    {

        public static void Bluetooth_MessageReceived(object sender, string message)
        {
            System.Diagnostics.Debug.WriteLine(message);

            ArduinoController.extractSensorReadings(message);
            

        }

        
    }
}
