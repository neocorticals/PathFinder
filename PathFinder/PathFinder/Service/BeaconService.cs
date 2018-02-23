using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using System.Threading;
using PathFinder.Abstractions;
using Plugin.LocalNotifications;
using Plugin.Vibrate;

namespace PathFinder
{
    public class BeaconService
    {
        static IEnumerable<object> beaconCollection;
        static IBeaconLocater beaconLocater;
        static TimerHolder startTimer;
        static string previousStationName = "";
        static DateTime datetime45;
        public static bool IsBusy { get; set; }
         

        public static void Start()
        {
            IsBusy = false;
            
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
 

                    beaconLocater = DependencyService.Get<IBeaconLocater>();
                    beaconCollection = new ObservableCollection<BeaconItem>();

                    PermissionStatus status = PermissionStatus.Unknown;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        status = await CrossPermissions.Current
                                            .CheckPermissionStatusAsync(Permission.Location);

                        if (status != PermissionStatus.Granted)
                        {
                            if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                            {
                                await App.Current.MainPage.DisplayAlert("Location", "Please switch on Location", "OK");
                            }

                            var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                            status = results[Permission.Location];
                        }
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        status = await CrossPermissions.Current
                                            .CheckPermissionStatusAsync(Permission.Bluetooth);

                        if (status != PermissionStatus.Granted)
                        {
                            if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Bluetooth))
                            {
                                await App.Current.MainPage.DisplayAlert("Bluetooth", "Please switch on Bluetooth", "OK");
                            }

                            var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Bluetooth);
                            status = results[Permission.Bluetooth];
                        }

                        if (!CrossPermissions.Current.IsBluetoothEnabled())
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                var canBluetoothEnabled = await App.Current.MainPage.DisplayAlert("Bluetooth", "Please enable device Bluetooth to locate Beacons", "OK", "Cancel");
                                if (canBluetoothEnabled)
                                {
                                    CrossPermissions.Current.OpenBluetoothSettings();
                                }
                            });
                        } 
                    }

                    if (status == PermissionStatus.Granted)
                    {
                        startTimer = new TimerHolder(5000, async () =>
                        {
                            try
                            {
                                var beaconCollection = beaconLocater.GetAvailableBeacons();
                                     
                                if (beaconCollection != null && beaconCollection.Count() > 0)
                                {
                                    CrossLocalNotifications.Current.Show("PathFinder", "Wrong Direction");
                                    CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(1));
                                } 
                            }
                            catch (Exception)
                            {
                                BeaconService.IsBusy = false;
                            }
                        });
                        startTimer.Start();
                    }

                });
            }
            catch (Exception)
            {
            } 
        } 
    }
}
