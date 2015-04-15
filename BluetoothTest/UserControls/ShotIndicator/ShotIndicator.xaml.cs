using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BluetoothTest.UserControls.ShotIndicator
{
    public sealed partial class ShotIndicator : UserControl
    {

        private int health;
        private DispatcherTimer healTimer;
     

        public ShotIndicator()
        {
            this.InitializeComponent();
            health = 100;
            healTimer = new DispatcherTimer();
            healTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            healTimer.Tick += healTimer_Tick;
        }

        void healTimer_Tick(object sender, object e)
        {
            if(health < 100) {
                health += 1;
                rectangle.Opacity -= 0.01;
            }
            else
            {
                healTimer.Stop();
            }
            
        }

        public void gotShot()
        {
            if(healTimer.IsEnabled) 
            {
                healTimer.Stop();
            }

            healTimer.Start();


            if(health > 0) {

                health -= 20;
                initialState.Value = 1 - health / 100.0;
                finalState.Value = 1 - health / 100.0;

                shotAnimation.Begin();
            }
            else
            {
                healTimer.Stop();
                onEmptyHealthEvent(this);
            }
            
          
            
        }

        public delegate void onEmptyHealthDelegate(Object sender);
        public event onEmptyHealthDelegate onEmptyHealth;
        private void onEmptyHealthEvent(Object sender)
        {
            if (onEmptyHealth != null)
            {
                onEmptyHealth(sender);
            }
        }

        

    }
}
