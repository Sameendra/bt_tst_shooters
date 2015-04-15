using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace BluetoothTest.Model
{
    public class Coordinates : INotifyPropertyChanged
    {

        public Coordinates()
        {

            this.Altitude = 0;
            this.Latitude = 0;
            this.Longitude = 0;
        }

        private double latitude;

        public double Latitude
        {
            get { return latitude; }
            set 
            { 
                if(this.latitude != value) {
                    latitude = value;
                    NotifyPropertyChanged("Latitude");
                }  
            }
        }

        private double longitue;

        public double Longitude
        {
            get { return longitue; }

            set 
            { 
                if(this.longitue != value) {
                    longitue = value;
                    NotifyPropertyChanged("Longitude");
                }
                longitue = value; 
            }
        }

        private double altitude;

        public double Altitude
        {
            get { return altitude; }
            set 
            {
                if (this.altitude != value)
                {
                    altitude = value;
                    NotifyPropertyChanged("Altitude");
                }
                
            }
        }

        public double getAngleInRelationToNorth(Coordinates coordinates)
        {
            double lattitudeDifference = coordinates.Latitude - this.latitude;
            double longitudeDifference = coordinates.Longitude - this.longitue;

            double heading = Math.Atan2(lattitudeDifference, longitudeDifference);
            //heading *= -1;

            if(heading < 0) 
            {
                heading += 2 * Math.PI;
            }

            return heading;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged(String propertyName)
        {
            if (this.PropertyChanged != null)
            {
               
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
           
            }
        }

        public void unSubscribePropertyChanged()
        {
            this.PropertyChanged = null;
        }

        public async void GeoPositionChanged_EventHandler(Geolocator sender, PositionChangedEventArgs args)
        {

             await Task.Factory.StartNew(() => 
             {
                 this.Latitude = args.Position.Coordinate.Point.Position.Latitude;
                 this.Longitude = args.Position.Coordinate.Point.Position.Longitude;
                 this.Altitude = args.Position.Coordinate.Point.Position.Altitude;
             
             });

        }

        public double DistanceTo(Coordinates coordinates)
        {
            double rlat1 = Math.PI * this.latitude / 180;
            double rlat2 = Math.PI * coordinates.latitude / 180;
            double theta = this.longitue - coordinates.longitue;
            double rtheta = Math.PI * theta / 180;
            
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist * 1609.344;
        }

    }
}
