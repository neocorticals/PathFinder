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
        public static bool IsBusy { get; set; } 
        public static string LastLocation { get; set; }
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
                        startTimer?.Stop();
                        startTimer = new TimerHolder(1000,() =>
                        {
                            try
                            {
                                var beaconCollection = beaconLocater.GetAvailableBeacons();
                                var _BaseViewModel = Application.Current.MainPage.BindingContext as BaseViewModel;


                                if (beaconCollection != null && beaconCollection.Count() > 0
                                && (
                                       Helpers.Settings.SubCategorySettings == "Neuro"
                                    || Helpers.Settings.SubCategorySettings == "Emergency"

                                ))
                                {

                                   // _BaseViewModel.ImageName = "hospitalMap.png";
                                    
                                    var closestBeacon = ((List<BeaconItem>)beaconCollection).OrderBy(b => b.CurrentDistance).First();
                                    CrossLocalNotifications.Current.Show("PathFinder", "Wrong Direction", 101);
                                    CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(1));
                                }
                               else if (beaconCollection != null && beaconCollection.Count() > 0)
                                {
                                  

                                    var closestBeacon = ((List<BeaconItem>)beaconCollection).OrderBy(b => b.CurrentDistance).First();
                                    if (closestBeacon != null)
                                    {
                                        switch (closestBeacon.Minor)
                                        {
                                            case "10":
                                               
                                                if (LastLocation != "10")
                                                {
                                                    CrossLocalNotifications.Current.Show("PathFinder", "You are at conference room projector", 101);

                                                    Helpers.Settings.CurrentLocation = "projector";
                                                    _BaseViewModel.ImageName = "pj.png";
                                                }
                                                LastLocation = "10";
                                                break;
                                            case "15":
                                               
                                                if (LastLocation != "15")
                                                {
                                                    CrossLocalNotifications.Current.Show("PathFinder", "You are at conference room tv", 101);

                                                    Helpers.Settings.CurrentLocation = "tv";
                                                    _BaseViewModel.ImageName = "tv.png";
                                                }
                                                LastLocation = "15";
                                                break;
                                            case "20":
                                               
                                                if (LastLocation != "20")
                                                {
                                                    CrossLocalNotifications.Current.Show("PathFinder", "You are at server room", 101);

                                                    Helpers.Settings.CurrentLocation = "server";
                                                    _BaseViewModel.ImageName = "sr.png";
                                                }
                                                LastLocation = "20";
                                                break;
                                        }
                                    } 
                                   // CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(1));
                                }
                                else
                                {
                                   // _BaseViewModel.ImageName = "hospitalMap.png";
                                    CrossLocalNotifications.Current.Cancel(101);
                                    CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(0));
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
