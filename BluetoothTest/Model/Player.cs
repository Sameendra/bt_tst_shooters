using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BluetoothTest.Model
{
    public class Player : INotifyPropertyChanged
    {

        private readonly int pointUpdateLevel;

        public Player(String name, int pointUpdateLevel)
        {
            this.Name = name;
            this.Points = 0;
            this.Coordinates = new Coordinates();
            this.Weapon = null;
            this.pointUpdateLevel = pointUpdateLevel;
        }

        public Player()
        {
            // TODO: Complete member initialization
        }

        public String Name { get; set; }

        private int points;

        public int Points
        {
            get { return points; }
        }


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

        public void updatePoints()
        {
            points += pointUpdateLevel;
            NotifyPropertyChanged("Points");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (this.PropertyChanged != null)
            {

                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }


    }
}
