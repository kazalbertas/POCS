using Configuration;
using Grains.Grains.Messages;
using Grains.Grains.Route;
using Grains.Grains.TimeActor;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private int Intake { get; set; }
        private int TicksToWait { get; set; }

        //private List<Container> containers = new List<Containers>();
        private int Speed { get; set; }
        private int DistanceTraveledOnRoute { get; set; } = 0;
        private string Route { get; set; } = "";
        public List<(string, int)> companyAlloc = new List<(string, int)>();

        public async Task SetRoute(string route) 
        {
            Route = route;
        }

        public async Task Depart(DateTime time)
        {
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

        public async Task AddAllocationToVessel(int allocation, string company, DateTime ts)
        {
            companyAlloc.Add((company, allocation));
            var i = new AllocationModel();
            i.Allocation = allocation;
            i.VesselCode = VesselName;
            i.DepartureDate = ts;
            i.Company = company;
            var obj = JsonConvert.SerializeObject(i);
            await new KafkaProducer().SendToKafka(obj, "alloc");
            //_logger.LogInformation(obj);
        }

        public async Task<int> GetTicksToWait()
        {
            return TicksToWait;
        }

        public async Task SetTicksToWait(int ticks)
        {
            TicksToWait = ticks;
        }
    }
}
