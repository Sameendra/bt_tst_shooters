using BluetoothTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothTest
{
    class CommunicationHandler
    {

        public async Task commitPlayerUpdateAsync(Player player)
        {
            //push the player object with ID/Name and coordinates
        }

        public async Task commitShootUpdateAsync(string playerName, BluetoothTest.Model.Weapon.ShotStatus shotStatus)
        {

        }

        public async Task recievePlayerUpdatesAsync()
        {
            //the updated player
            Player player = new Player();

            OnPlayerUpdateReceivedEvent(this, player);
        }
 
        // on update recieved
        public delegate void OnPlayerUpdateRecievedDelegate(object sender, Player player);
        public event OnPlayerUpdateRecievedDelegate PlayerUpdateRecieved;
        private void OnPlayerUpdateReceivedEvent(object sender, Player player)
        {
            if (PlayerUpdateRecieved != null)
            {
                PlayerUpdateRecieved(sender, player);
            }
                
        }
    }
}
