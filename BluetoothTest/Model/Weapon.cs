using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;

namespace BluetoothTest.Model
{
    public abstract class Weapon
    {

        protected int MAX_SHOOTING_DISTANCE;       
        protected int MAX_MAGAZINES_POSSIBLE;

        protected MediaElement gunSoundPlayer;

        private List<Magazine> magazines;

        private Uri emptyFireSoundSourse;

        protected Uri gunfireSoundSourse;

        protected Weapon()
        {
            magazines = new List<Magazine>();
            magazinesLeft = 0;
            emptyFireSoundSourse = new Uri("ms-appx:///Assets/SoundClips/emptyGunClick.mp3");
        }

        public int GetMaxShootingDistance
        {
            get { return MAX_SHOOTING_DISTANCE; }
        }


        public int AmmoLeft
        {
            get { 
                return magazines.Count > 0 ? magazines[magazinesLeft-1].AmmoLeft : 0; 
            }
        }


        protected int magazinesLeft;
        
        public int MagazinesLeft
        {
            get { return magazinesLeft; }
           
        }

        
        public void reload()
        {
            if(magazines.Count > 0) {
                magazines.RemoveAt(MagazinesLeft - 1);
                magazinesLeft--;
                gunSoundPlayer.Source = gunfireSoundSourse;
            }
            
        }


        public void collectAmmo(List<Magazine> magazines)
        {
            foreach(Magazine mag in magazines) 
            {
                this.magazines.Add(mag);
            }
        }

        public void fire()
        {
            if (AmmoLeft > 0)
            {

                magazines[magazinesLeft - 1].releaseAmmo();
                NotifyPropertyChanged("AmmoLeft");
                playGunSound();
                
              
                
            }
            else
            {
                if (gunSoundPlayer.Source == gunfireSoundSourse)
                {

                    gunSoundPlayer.Stop();
                    gunSoundPlayer.Source = emptyFireSoundSourse;
                } 
                playGunSound();
                throw new EmptyAmmoException("Ammo remaining : " +AmmoLeft +" ");
            }
              
        }



        private async void playGunSound()
        {
            if (gunSoundPlayer.CurrentState == MediaElementState.Playing)
            {

                gunSoundPlayer.Pause();
                gunSoundPlayer.Position = TimeSpan.Zero;

            }
            gunSoundPlayer.Play();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (this.PropertyChanged != null)
            {

                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }

        public enum ShotStatus
        {
            Missed,
            Poor,
            Good,
            Perfect,
        }
    }
}
