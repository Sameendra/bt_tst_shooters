﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
            double lattitudeDifference = this.latitude - coordinates.Latitude;
            double longitudeDifference = this.longitue - coordinates.Longitude;

            return Math.Atan2(longitudeDifference, lattitudeDifference);
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

        public void GeoPositionChanged_EventHandler(Geolocator sender, PositionChangedEventArgs args)
        {

            this.Latitude = args.Position.Coordinate.Point.Position.Latitude;
            this.Longitude = args.Position.Coordinate.Point.Position.Longitude;
            this.Altitude = args.Position.Coordinate.Point.Position.Altitude;


        }
    }
}
