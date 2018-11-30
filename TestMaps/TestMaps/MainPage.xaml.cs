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
        Xamarin.Forms.Maps.Pin[] array = new Xamarin.Forms.Maps.Pin[5];

        public MainPage()
        {
            InitializeComponent();
#pragma warning disable CS4014
            StartListening();
#pragma warning restore CS4014
            DirSalida.Completed += DirSalida_Completed;
            DirDestino.Completed += DirDestino_Completed;
            MyMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(
            new Xamarin.Forms.Maps.Position(37, -122), Distance.FromMiles(1)));
        }

        private async void DirDestino_Completed(object sender, EventArgs e)
        {
            var location = await CrossGeolocator.Current.GetPositionsForAddressAsync(DirDestino.Text);
            var firstlocation = location.FirstOrDefault();
            MyMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(
            new Xamarin.Forms.Maps.Position(firstlocation.Latitude, firstlocation.Longitude), Distance.FromKilometers(1)));
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Xamarin.Forms.Maps.Position(firstlocation.Latitude, firstlocation.Longitude),
                Label = "Destino"
            };
            array[0] = pin;
            MyMap.Pins.Add(pin);
        }

        private async void DirSalida_Completed(object sender, EventArgs e)
        {
            var location = await CrossGeolocator.Current.GetPositionsForAddressAsync(DirSalida.Text);
            var firstlocation = location.FirstOrDefault();
            MyMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(
            new Xamarin.Forms.Maps.Position(firstlocation.Latitude, firstlocation.Longitude), Distance.FromKilometers(1)));
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Xamarin.Forms.Maps.Position(firstlocation.Latitude, firstlocation.Longitude),
                Label = "Origen"
            };
            array[1] = pin;
            MyMap.Pins.Add(pin);
        }

        private async Task StartListening()
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
            Latitude.Text = "Latitude: " + e.Position.Latitude.ToString();
            Longitude.Text = "Longitude: " + e.Position.Longitude.ToString();
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Xamarin.Forms.Maps.Position(e.Position.Latitude, e.Position.Longitude),
                Label = "Mi Posicion"
            };
            MyMap.Pins.Clear();
            MyMap.Pins.Add(pin);
            if (array[0] != null)
            {
                MyMap.Pins.Add(array[0]);
            }
            if (array[1] != null)
            {
                MyMap.Pins.Add(array[1]);
            }
        }
    }
}
