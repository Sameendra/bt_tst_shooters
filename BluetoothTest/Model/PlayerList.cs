﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothTest.Model
{
    public class PlayerList
    {

        public PlayerList()
        {
            this.Players = new ObservableCollection<Player>();
        }
        public ObservableCollection<Player> Players { get; set; }

        public Player getTheShotPlayer(Player shootingPlayer, double shootingAngle)
        {
            foreach (Player player in Players)
            {
                if (shootingPlayer.isShot(player, shootingAngle))
                {
                    return player;
                }
            }
            return null;
        }

        public bool contains(Player player)
        {
            foreach (Player opponetPlayer in Players)
            {
                if (opponetPlayer.ConnectionID.Equals(player.ConnectionID))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
