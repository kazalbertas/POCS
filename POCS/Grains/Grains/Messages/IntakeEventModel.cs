using System;
using System.Collections.Generic;
using System.Text;

namespace Grains.Grains.Messages
{
    public class IntakeEventModel
    {
        public string VesselCode { get; set; }
        public DateTime DepartureDate { get; set; }
        public float Intake { get; set; }
    }
}
