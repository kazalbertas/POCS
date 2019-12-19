using Grains.Grains.Route;
using Grains.Grains.TimeActor;
using Grains.Grains.Vessels;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains.Ports
{
    public class Port : Grain, IPort
    {
        private string Name { get; set; }
        private List<string> vessels = new List<string>();
        private readonly ILogger _logger;
        private List<string> routes = new List<string>();
        private int PortIntake { get; set; }

        public Task<string> GetName()
        {
            throw new NotImplementedException();
        }

        public Task SetName()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DoAction(DateTime startTS, DateTime endTS)
        {
            var taskList = new List<Task<string>>();
            
            foreach (var vessel in vessels)
            {
                var v = GrainFactory.GetGrain<IVessel>(vessel);
                taskList.Add(Depart(v, vessel, startTS));
            }

            if (taskList.Count != 0) 
            {
                var remove = await Task.WhenAll(taskList.ToArray());
                foreach (var r in remove) 
                {
                    if (vessels.Remove(r)) _logger.LogInformation("Success remove from port");
                }
                
            }
            
            return true;
        }

        private async Task<string> Depart(IVessel vessel, string vesselName, DateTime ts) 
        {
            var r = new Random();
            int routeIndex = r.Next(0, routes.Count-1);
            AssignAllocationAsync(vessel);
            await vessel.Depart(routes[routeIndex], ts);
            await vessel.ResetDistanceOnRoute();
            return vesselName;
        }

        
        public Port(ILogger<Port> logger)
        {
            _logger = logger;
        }

        public async Task AssignAllocationAsync(IVessel v) 
        {
            int capacity = await v.GetCapacity();
            var r = new Random();
            var range = r.Next(80, 120);
            var percentage = range / 100f;
            var percentagePerCompany = percentage / 3f;

            var companies = Configuration.Constants.Companies.OrderBy(x => r.Next()).Take(3);

            foreach (var c in companies) 
            {
                var a = (int)Math.Round(capacity * percentagePerCompany);
                await v.AddAllocationToVessel(a,c);
            }
        }

        public Task VesselArrived(string vessel, DateTime time)
        {
            var log = vessel + " arrived at " + time.ToString();
            _logger.LogInformation(log);
            vessels.Add(vessel);
            return Task.CompletedTask;
        }

        public Task SetAll(string name, int portIntake, List<string> routes, List<string> startingVessels)
        {
            Name = name;
            PortIntake = portIntake;
            this.routes.AddRange(routes);
            vessels.AddRange(startingVessels);
            return Task.CompletedTask;
        }
    }
}
