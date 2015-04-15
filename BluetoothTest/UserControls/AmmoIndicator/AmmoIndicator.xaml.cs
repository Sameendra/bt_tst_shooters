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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BluetoothTest.UserControls.AmmoIndicator
{
    public sealed partial class AmmoIndicator : UserControl
    {

        /*
        private int ammoLeft;

        public int AmmoLeft
        {
            get { return ammoLeft; }
            set 
            { 
                ammoLeft = value;
                updateUI();
            }
        }
        */


        public int AmmoLeft
        {
            get { return (int)GetValue(AmmoLeftProperty); }
            set { 
                SetValue(AmmoLeftProperty, value);
                updateUI();
            }
        }

        // Using a DependencyProperty as the backing store for AmmoLeft2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AmmoLeftProperty =
            DependencyProperty.Register("AmmoLeft", typeof(int), typeof(AmmoIndicator), new PropertyMetadata(0));



        private BitmapImage[] digits;
        

        public AmmoIndicator()
        {
            this.InitializeComponent();
            digits = new BitmapImage[10];
            
            for(int x = 0; x < 10; x++) 
            {
                BitmapImage digit = new BitmapImage(new Uri("ms-appx:///UserControls/AmmoIndicator/Digits/" + x + ".png"));
                digits[x] = digit;
            }

            AmmoLeft = 0;

            
        }

        public void reduceAmmobyOne()
        {
            AmmoLeft -= 1;
        }

        private void updateUI()
        {
            int placeValue100 = AmmoLeft / 100;
            int placeValue10 = (AmmoLeft % 100) / 10;
            int placeValue1 = (AmmoLeft % 100) % 10;
            
            pv1.Source = digits[placeValue1];
            pv10.Source = digits[placeValue10];
            pv100.Source = digits[placeValue100];
        }
    }
}
