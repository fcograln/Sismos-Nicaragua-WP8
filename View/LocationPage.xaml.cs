using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using SismoNica_WP8._0.ViewModel;
using System;
using System.Device.Location;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace SismoNica_WP8._0.View
{
    public partial class LocationPage : PhoneApplicationPage
    {
        private QuakeViewModel tempViewModel;
        public LocationPage()
        {
            InitializeComponent();
        }
      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                string selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
                {
                    int index = int.Parse(selectedIndex);
                    if (index==-1)
                    {
                        index = 0;
                    }
                    tempViewModel = (QuakeViewModel)Application.Current.Resources["GlobalQuakeViewModel"];
                    DataContext = tempViewModel.AllQuakeObjects[index];
                    Dispatcher.BeginInvoke(() =>
                    {
                        BitmapImage Temp = new BitmapImage();
                        Temp.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                        Temp.UriSource = new Uri("/Assets/InAppAssets/pin.png", UriKind.RelativeOrAbsolute);
                        if (map.Layers.Count != 0)
                        {
                            map.Layers.Clear();
                        }
                        MapLayer layer = new MapLayer();
                        MapOverlay Overlay = new MapOverlay()
                        {
                            GeoCoordinate = new GeoCoordinate(Double.Parse(tempViewModel.AllQuakeObjects[index].latitude, CultureInfo.InvariantCulture), (Double.Parse(tempViewModel.AllQuakeObjects[index].longitude, CultureInfo.InvariantCulture)*-1)),
                            Content = new Image() { Source = Temp, Width = 50 }
                        };

                        layer.Add(Overlay);
                        map.Layers.Add(layer);
                    });
                    
                }
            }

        }
    }
}