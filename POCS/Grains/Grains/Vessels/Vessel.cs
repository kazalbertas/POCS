using Configuration;
using Grains.Grains.Route;
using Grains.Grains.TimeActor;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains.Vessels
{
    public class Vessel : Grain, IVessel
    {
        private readonly ILogger _logger;
        public Vessel(ILogger<Vessel> logger) 
        {
            _logger = logger;
        }


        private string VesselName { get; set; }
        private int Capacity { get; set; }
        //private List<Container> containers = new List<Containers>();
        private int Speed { get; set; }
        private int DistanceTraveledOnRoute { get; set; } = 0;
        private string Route { get; set; } = "";
        public List<(string, int)> companyAlloc = new List<(string, int)>();

        public async Task Depart(string route, DateTime time)
        {

            //TODO: WRITE COMMENTS FOR CODE!
            Route = route;
            var r = GrainFactory.GetGrain<IRoute>(Route);
            await r.AddVessel(VesselName,time);
        }

        public Task<bool> DoAction(DateTime startTS, DateTime endTS)
        {
            if (Route != "") 
            {
                DistanceTraveledOnRoute += Convert.ToInt32(Math.Round(Speed * Constants.TickSizeSeconds / (float)3600));
            }
            return Task.FromResult(true);
        }

        public Task<int> GetCapacity()
        {
            return Task.FromResult(Capacity);
        }

        public Task<int> GetDistanceOnRoute()
        {
            return Task.FromResult(DistanceTraveledOnRoute);
        }

        public Task ResetDistanceOnRoute()
        {
            DistanceTraveledOnRoute = 0;
            return Task.CompletedTask;
        }

        public Task SetAll(string name, int capacity, int speed)
        {
            VesselName = name;
            Capacity = capacity;
            Speed = speed;
            return Task.CompletedTask;
        }

        public Task AddAllocationToVessel(int allocation, string company)
        {
            companyAlloc.Add((company, allocation));
            var r = company + " allocated " + allocation + " tons";
            _logger.LogInformation(r);
            return Task.CompletedTask;
        }
    }
}
