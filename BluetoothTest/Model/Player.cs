using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluetoothTest.Model
{
    public class Player
    {
        public Player(String name)
        {
            this.Name = name;
            this.Points = 0;
            this.Coordinates = new Coordinates();
            this.Weapon = null;
        }

        public Player()
        {
            // TODO: Complete member initialization
        }

        public String Name { get; set; }

        public int Points { get; set; }

        public Coordinates Coordinates { get; set; }

        public Weapon Weapon { get; set; }

        public Boolean isShot(Player shootingPlayer, double shootingAngle)
        {
            
            double estimatedAngle = this.Coordinates.getAngleInRelationToNorth(shootingPlayer.Coordinates);
            System.Diagnostics.Debug.WriteLine("Estimated Angle: " +estimatedAngle);
            System.Diagnostics.Debug.WriteLine("Actual Angle: " + shootingAngle);

            if (Math.Abs((estimatedAngle - shootingAngle)) < 0.1)
            {
                return true;
            }
            return false;
     
        }
    }
}
