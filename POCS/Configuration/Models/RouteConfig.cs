using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration.Models
{
    public class RouteConfig
    {

        public string RouteId { get; set; }
        public string SourcePortId { get; set; }
        public string DestinationPortId { get; set; }
        public int Distance { get; set; }

        public RouteConfig(string routeId, string sourcePortId, string destinationPortId, int distance)
        {
            RouteId = routeId;
            SourcePortId = sourcePortId;
            DestinationPortId = destinationPortId;
            Distance = distance;
        }
    }
}
