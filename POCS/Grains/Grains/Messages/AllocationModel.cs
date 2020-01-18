using System;
using System.Collections.Generic;
using System.Text;

namespace Grains.Grains.Messages
{
    public class AllocationModel
    {
        
        public string VesselCode { get; set; }
        public DateTime DepartureDate { get; set; }
        public float Allocation { get; set; }
        public string Company { get; set; }
        }
}
