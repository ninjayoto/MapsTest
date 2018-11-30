using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms.Maps;

namespace TestMaps
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            StartListening();
            MyMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(
            new Xamarin.Forms.Maps.Position(37, -122), Distance.FromMiles(1)));
        }

        async Task StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
                return;
            
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true, new ListenerSettings
            {
                ActivityType = ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = true,
                DeferralDistanceMeters = 1,
                DeferralTime = TimeSpan.FromSeconds(1),
                ListenForSignificantChanges = true,
                PauseLocationUpdatesAutomatically = false
            });

            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
            
        }

        private void Current_PositionChanged(object sender, PositionEventArgs e)
        {
            Latitude.Text = e.Position.Latitude.ToString();
            Longitude.Text = e.Position.Longitude.ToString();
            MyMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(
            new Xamarin.Forms.Maps.Position(e.Position.Latitude,e.Position.Longitude), Distance.FromKilometers(1)));
            var pin = new Pin
            {
                Type = PinType.Place,
                Position =  new Xamarin.Forms.Maps.Position(e.Position.Latitude, e.Position.Longitude),
                Label = "Mi Posicion"
            };
            MyMap.Pins.Clear();
            MyMap.Pins.Add(pin);

        }
    }
}
