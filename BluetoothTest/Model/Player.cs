using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.Devices.Geolocation;

namespace BluetoothTest.Model
{
    public class Player : INotifyPropertyChanged
    {

        private readonly int pointUpdateLevel;

        public Player(String name, int pointUpdateLevel)
        {
            this.Name = name;
            this.points = 0;
            this.Coordinates = new Coordinates();
            this.Weapon = null;
            this.pointUpdateLevel = pointUpdateLevel;
        }

        public Player()
        {
            // TODO: Complete member initialization
        }

        public String Name { get; set; }

        public String ConnectionID { get; set; }

        private int points;

        public int Points
        {
            get { return points; }
        }


        public Coordinates Coordinates { get; set; }

        public Weapon Weapon { get; set; }

        public Boolean isShot(Player shotPlayer, double shootingAngle)
        {

            const double TOLERENCE_DISTANCE = 0.3;

            double distance = this.Coordinates.DistanceTo(shotPlayer.Coordinates);

            if(distance > this.Weapon.GetMaxShootingDistance) {
                return false;
            }
            
            double actualAngle = this.Coordinates.getAngleInRelationToNorth(shotPlayer.Coordinates);
            System.Diagnostics.Debug.WriteLine("Actual Angle: " +actualAngle);
            System.Diagnostics.Debug.WriteLine("Shooting Angle: " + shootingAngle);

            double angleDifference = Math.Abs(actualAngle - shootingAngle) % 360;
            double angleDistance = angleDifference > 180 ? 360 - angleDifference : angleDifference;


            if (angleDistance < Math.Asin(TOLERENCE_DISTANCE/distance))
            {
                return true;
            }
            return false;
     
        }

        public void shoot(double shootingAngle)
        {
            try
            {
                Weapon.fire();
                OnShootingEvent(this, shootingAngle);
            }
            catch(EmptyAmmoException ex) {

                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            
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

        public delegate void OnShootingEventDelegate(object sender, double shootingAngle);
        public event OnShootingEventDelegate Shot;
        private void OnShootingEvent(object sender, double shootingAngle)
        {
            if (Shot != null)
                Shot(sender, shootingAngle);
        }
        

    }
}
