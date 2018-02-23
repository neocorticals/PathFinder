using System;
using System.Collections.Generic;
using System.Text;

namespace PathFinder.Abstractions
{
    public interface IBeaconLocater
	{ 
        IEnumerable<object> GetAvailableBeacons(); 
        void PauseTracking();
		void ResumeTracking();
	}
}
