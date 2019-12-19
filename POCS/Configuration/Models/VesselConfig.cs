using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration.Models
{
    public class VesselConfig
    {
        public string VesselName { get; set; }
        public int Capacity { get; set; }
        public int Speed { get; set; }

        public VesselConfig(string vesselName, int capacity, int speed) 
        {
            VesselName = vesselName;
            Capacity = capacity;
            Speed = speed;
        }
    }
}
