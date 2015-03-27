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

namespace BluetoothTest.UserControls

{
    public sealed partial class HealthIndicator : UserControl
    {
        private int health;
        private int energy;
        private const int hBarDistance = -66;
        private const int eBarDistance = -48;
        private List<Image> eBars;
        private List<Image> hBars;
       

        public HealthIndicator()
        {
            this.InitializeComponent();
            health = 100;
            energy = 100;

            initBars();

        }

        private void initBars()
        {

            eBars = new List<Image>();
            hBars = new List<Image>();




            for(int x = 0; x < 8; x++) {

                Image HBar = createImage(new Uri("ms-appx:///UserControls/Bars_Images/Rectangle 1 copy 71.png"), 107, 135);             
                
                Canvas.SetLeft(HBar, 0);
                Canvas.SetTop(HBar, 0);
                HBar.RenderTransformOrigin = new Point(0.5, 0.5);

                CompositeTransform trfBar = new CompositeTransform();
                trfBar.TranslateX = x * hBarDistance;
                HBar.RenderTransform = trfBar;
                

                Health_Bars.Children.Add(HBar);
                hBars.Add(HBar);
            }

            for (int x = 0; x < 8; x++)
            {


                Image EBar = createImage(new Uri("ms-appx:///UserControls/Bars_Images/Rectangle 1 copy 7.png"), 90, 111);
                Canvas.SetLeft(EBar, 0);
                Canvas.SetTop(EBar, 0);
                EBar.RenderTransformOrigin = new Point(0.5, 0.5);

                CompositeTransform trfBar = new CompositeTransform();
                trfBar.TranslateX = x * eBarDistance;
                EBar.RenderTransform = trfBar;


                Energy_Bars.Children.Add(EBar);
                eBars.Add(EBar);
            }
        }

        private Image createImage(Uri uri, int height, int width)
        {
            Image image = new Image();
            BitmapImage bmpImage = new BitmapImage(uri);
            image.Source = bmpImage;

            image.Height = height;
            image.Width = width;

            return image;
        }
        


    }
}
