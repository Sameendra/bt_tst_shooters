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


    }
}
