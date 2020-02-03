using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Locations;
using System.Linq;
using Android.Widget;
using System.Collections.Generic;
using System;
using Context = Android.Content.Context;
using System.Threading;
using System.Threading.Tasks;

namespace ReverseGeoCode
{
    [Activity(Label = "@string/ReverseGeoCode", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        //   Massachusetts lat/long = 42.37419, -71.120639   
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var but = FindViewById<Android.Widget.Button>(Resource.Id.button);
            var but1 = FindViewById<Android.Widget.Button>(Resource.Id.button1);
            var but2 = FindViewById<Android.Widget.Button>(Resource.Id.button2);
            EditText house = FindViewById<Android.Widget.EditText>(Resource.Id.houseNo);
            EditText ave = FindViewById<Android.Widget.EditText>(Resource.Id.street);
            EditText city = FindViewById<Android.Widget.EditText>(Resource.Id.city);
            EditText state = FindViewById<Android.Widget.EditText>(Resource.Id.state);
            EditText zip = FindViewById<Android.Widget.EditText>(Resource.Id.zipCode);
            EditText lat = FindViewById<Android.Widget.EditText>(Resource.Id.lat);
            EditText lon = FindViewById<Android.Widget.EditText>(Resource.Id.lon);
            TextView addressText = FindViewById<TextView>(Resource.Id.street);
            /**********************************************************************************/
            but.Click += async (sender, e) =>
            {
                Context con = this;
                if ((house.Text == "") && (city.Text == "") && (state.Text == ""))
                {    // get address from lat/long
                    double theLatitude;
                    double theLongitude;
                    theLatitude = Convert.ToDouble(lat.Text);
                    theLongitude = Convert.ToDouble(lon.Text);
                    var geo = new Geocoder(this);
                    IList<Address> addresses = geo.GetFromLocation(theLatitude, theLongitude, 1);
                    await MyAsyncMethod();
                    if (addresses.Any())
                    {
                        UpdateAddressFields(addresses.First());
                    }
                    else
                    {
                        Toast.MakeText(this.ApplicationContext, "Did not find address", ToastLength.Short).Show();
                    }
                }
                else   // get lat/long from address
                {
                    string addr = house.Text + " " + ave.Text + " " + city.Text + " " + state.Text + " " + zip.Text;
                    IList<Address> addresses;
                    var geo = new Geocoder(con);
                    addresses = geo.GetFromLocationName(addr, 1);
                    await MyAsyncMethod();
                    if (!addresses.Any())
                    {
                        Toast.MakeText(this.ApplicationContext, "Did not find LatLong", ToastLength.Short).Show();
                    }
                    double a = addresses.First().Latitude;
                    lat.Text = a.ToString();
                    double b = addresses.First().Longitude;
                    lon.Text = b.ToString();
                }
            };
            /**************************************************************************************/
            but2.Click += (sender, e) =>
            {
                house.Text = "";
                ave.Text = "";
                city.Text = "";
                state.Text = "";
                zip.Text = "";
            };
            /**************************************************************************************/
            but1.Click += (sender, e) =>
            {
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            };
            /**************************************************************************************/
            void UpdateAddressFields(Address addr)
            {
                house.Text = addr.FeatureName;
                state.Text = addr.AdminArea;
                zip.Text = addr.PostalCode;
                ave.Text = addr.Thoroughfare;
                city.Text = addr.Locality;
            }
        }
        /**************************************************************************************/
        private static Task MyAsyncMethod()
        {
            Thread.Sleep(2000);
            return Task.CompletedTask;
             
        }
    }
}
