using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PathFinder.Abstractions;
using RadiusNetworks.IBeaconAndroid; 
using Xamarin.Forms;
using PathFinder.Droid; 

[assembly: Dependency(typeof(BeaconLocater))] 
namespace PathFinder.Droid
{
    public class BeaconLocater : Java.Lang.Object, IBeaconLocater, IBeaconConsumer
    { 
        IBeaconManager iBeaconManager;
        MonitorNotifier monitorNotifier;
        RangeNotifier rangeNotifier;
        Region monitoringRegion;
        Region rangingRegion;
        Context context;
        bool paused;
        List<BeaconItem> beacons;
       

        public BeaconLocater()
        {
            beacons = new List<BeaconItem>();
            context = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;

            iBeaconManager = IBeaconManager.GetInstanceForApplication(context);
            monitorNotifier = new MonitorNotifier();
            rangeNotifier = new RangeNotifier();
            //DMI beacon UUId - b9407f30-f5f8-466e-aff9-25556b57fe6d
            monitoringRegion = new Region("ID", "6A1A7B3B-2B8C-6A4C-9A7C-0D1403107060", null, null);
            rangingRegion = new Region("ID", "6A1A7B3B-2B8C-6A4C-9A7C-0D1403107060", null, null);

            iBeaconManager.Bind(this);

            rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;

        }

        //public List<BeaconItem> GetAvailableBeacons()
        public IEnumerable<object> GetAvailableBeacons()
        {
            return !paused ? beacons : null;
        }

        public void PauseTracking()
        {
            paused = true;
        }

        public void ResumeTracking()
        {
            paused = false;
        } 

        void RangingBeaconsInRegion(object sender, RangeEventArgs e)
        {
           // beacons = new List<BeaconItem>();
            if (e.Beacons.Count > 0)
            {
                foreach (var b in e.Beacons)
                {
                    if ((ProximityType)b.Proximity != ProximityType.Unknown)
                    {

                        var exists = false;
                        for (int i = 0; i < beacons.Count; i++)
                        {
                            if (beacons[i].Minor.Equals(b.Minor.ToString()))
                            {
                                beacons[i].CurrentDistance = Math.Round(b.Accuracy, 2);
                                SetProximity(b, beacons[i]);
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            var newBeacon = new BeaconItem
                            {
                                Major = b.Major.ToString(),
                                Minor = b.Minor.ToString(),
                                Name = "",
                                CurrentDistance = Math.Round(b.Accuracy, 2)
                            };
                            SetProximity(b, newBeacon);
                            beacons.Add(newBeacon);
                        }
                    }
                }
            }
        }
        void SetProximity(IBeacon source, BeaconItem dest)
        {

            Proximity p = Proximity.Unknown;

            switch ((ProximityType)source.Proximity)
            {
                case ProximityType.Immediate:
                    p = Proximity.Immediate;
                    break;
                case ProximityType.Near:
                    p = Proximity.Near;
                    break;
                case ProximityType.Far:
                    p = Proximity.Far;
                    break;
            }

            if (p > dest.Proximity || p < dest.Proximity)
            {
                dest.ProximityChangeTimestamp = DateTime.Now;
            }

            dest.Proximity = p;
        }

        public void OnIBeaconServiceConnect()
        {
            iBeaconManager.SetMonitorNotifier(monitorNotifier);
            iBeaconManager.SetRangeNotifier(rangeNotifier);

            iBeaconManager.StartMonitoringBeaconsInRegion(monitoringRegion);
            iBeaconManager.StartRangingBeaconsInRegion(rangingRegion);
        }

        public Context ApplicationContext
        {
            get { return this.context; }
        }

        public bool BindService(Intent intent, IServiceConnection connection, Bind bind)
        {
            return context.BindService(intent, connection, bind);
        }

        public void UnbindService(IServiceConnection connection)
        {
            context.UnbindService(connection);
        }
    }
}