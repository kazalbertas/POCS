using System;
using System.Collections.Generic;
using Configuration.Models;
using Orleans;
namespace Configuration
{
    public static class Constants
    {

        public static void Main(string[] args)
        {
            
        }
        
        static Constants()
        { 
            //Load configuration from files.
        }

        public static string TimeActorId { get; } = "TimeActorId";

        public static int TickSizeSeconds { get; } = 60 * 60;

        public static List<VesselConfig> Vessels { get; } = new List<VesselConfig>() 
        {
            new VesselConfig("Ship1",100,100),
            new VesselConfig("Ship2",100,50),
            new VesselConfig("Ship3",100,75),
            new VesselConfig("Ship4",100,30),
            new VesselConfig("Ship5",100,150)
        };

        public static List<PortConfig> PortIds { get; } = new List<PortConfig>()
        {
            new PortConfig("Port1",150,new List<string>(){ "Ship1" }),
            new PortConfig("Port2",150,new List<string>(){ "Ship2" }),
            new PortConfig("Port3",100,new List<string>(){ "Ship3" }),
            new PortConfig("Port4",100,new List<string>(){ "Ship4" }),
            new PortConfig("Port5",100,new List<string>(){ "Ship5" })
        };

        public static List<RouteConfig> Routes { get; } = new List<RouteConfig>
        {
            new RouteConfig("Route1_A","Port1","Port2",400),
            new RouteConfig("Route1_B","Port2","Port1",400),
            new RouteConfig("Route2_A","Port2","Port3",800),
            new RouteConfig("Route2_B","Port3","Port2",800),
            new RouteConfig("Route3_A","Port3","Port4",700),
            new RouteConfig("Route3_B","Port4","Port3",700),
            new RouteConfig("Route4_A","Port3","Port5",1500),
            new RouteConfig("Route4_B","Port5","Port3",1500),
            new RouteConfig("Route5_A","Port5","Port4",2222),
            new RouteConfig("Route5_B","Port4","Port5",2222)
        };

        public static List<string> Companies { get; } = new List<string>()
        {
            "Maersk",
            "Aviva",
            "Apple",
            "Girteka",
            "Icarus",
            "SOHO",
            "UPS",
            "DHL",
            "Vores",
            "Myprotein"
        };

    }
}
