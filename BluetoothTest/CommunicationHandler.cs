using BluetoothTest.Model;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothTest
{
    class CommunicationHandler
    {

        private IHubProxy shServer;
        private const String CONNECTION_STRING = "http://localhost:7644";

        public async void initializeConnection(Player player)
        {
            
            var hubConnection = new HubConnection(CONNECTION_STRING);
            shServer = hubConnection.CreateHubProxy("PlayerHub");

            shServer.On<HubPlayer>("initPlayer", (newHubPlayer) => addNewPlayer(newHubPlayer));
            shServer.On<String>("getShot", (connectionID) => OnPlayergetShotEvent());
            shServer.On<HubPlayer>("updateCoordinates", (updatedHubPlayer) => recievePlayerUpdatesAsync(updatedHubPlayer.ConnectionID, updatedHubPlayer.Coordinates));
            
            await hubConnection.Start();

            HubPlayer hubPlayer = new HubPlayer();
            player.ConnectionID = hubConnection.ConnectionId;
            hubPlayer.ConnectionID = player.ConnectionID;
            hubPlayer.UserName = player.Name;
            hubPlayer.Coordinates = player.Coordinates;
            await shServer.Invoke("connect", hubPlayer);
            

        }

        public async void updatePlayerLocationAsync(Player player)
        {
            HubPlayer hubPlayer = new HubPlayer();
            hubPlayer.UserName = player.Name;
            hubPlayer.ConnectionID = player.ConnectionID;
            hubPlayer.Coordinates = player.Coordinates;

            await shServer.Invoke("updateCoordinatesChange", hubPlayer);

        }

        public async void commitShootUpdateAsync(string connectionID)
        {
            await shServer.Invoke("sendShotStatus", connectionID);
        }

        public async Task recievePlayerUpdatesAsync(String connectionID, Coordinates coordinates)
        {
            //the updated player
            Player player = new Player();
            player.ConnectionID = connectionID;
            player.Coordinates = coordinates;

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

        //on Player get Shot
        public delegate void OnPlayerGetShotDelegate();
        public event OnPlayerGetShotDelegate PlayerGetShot;
        private void OnPlayergetShotEvent()
        {
            if (PlayerGetShot != null)
            {
                PlayerGetShot();
            }

        }

        public async void addNewPlayer(HubPlayer hubPlayer)
        {

            Player player = new Player();
            player.ConnectionID = hubPlayer.ConnectionID;
            player.Name = hubPlayer.UserName;
            player.Coordinates = hubPlayer.Coordinates;

            OnPlayerUpdateReceivedEvent(this, player);

        }
    }
}
