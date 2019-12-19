using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration.Models
{
    public class PortConfig
    {
        public string PortId { get; set; }
        public int Intake { get; set; }
        public List<string> startingVessels { get; set; }

        public PortConfig(string portId, int intake, List<string> vs) 
        {
            PortId = portId;
            Intake = intake;
            startingVessels = vs;
        }
    }
}
