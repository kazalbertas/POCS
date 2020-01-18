using Configuration;
using Configuration.Models;
using Grains.Grains.Ports;
using Grains.Grains.Route;
using Grains.Grains.Vessels;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Timers;
using System.Diagnostics;
using System.IO;

namespace Grains.Grains.TimeActor
{
    public class TimeGrain : Grain, ITimeGrain
    {
        private readonly ILogger logger;
        public DateTime OldTimeStamp { get; set; }
        public DateTime NewTimeStamp { get; set; }

       // private List<ITick> actors = new List<ITick>();

        private List<string> portActors = new List<string>();
        private List<string> routeActors = new List<string>();
        private List<string> vesselActors = new List<string>();

        public TimeGrain(ILogger<TimeGrain> _logger) 
        {
            logger = _logger;
        }

        public async Task InitialiseSimulation(DateTime start)
        {
            OldTimeStamp = start;
            NewTimeStamp = start.AddSeconds(Constants.TickSizeSeconds);

            List<Task> taskList = new List<Task>();
            foreach (var vessel in Constants.Vessels) 
            {
                var v = GrainFactory.GetGrain<IVessel>(vessel.VesselName);
                var t = v.SetAll(vessel.VesselName, vessel.Capacity, vessel.Speed);
                
                vesselActors.Add(vessel.VesselName);
                taskList.Add(t);
            }

            foreach (var route in Constants.Routes)
            {
                var r = GrainFactory.GetGrain<IRoute>(route.RouteId);
                var t = r.SetAll(route.RouteId,route.SourcePortId,route.DestinationPortId,route.Distance);
                
                routeActors.Add(route.RouteId);
                taskList.Add(t);

            }

            foreach (var port in Constants.PortIds)
            {
                var p = GrainFactory.GetGrain<IPort>(port.PortId);
                var routes = Constants.Routes.Where(x => x.SourcePortId == port.PortId).Select(x=>x.RouteId).ToList();
                var t = p.SetAll(port.PortId,port.Intake,routes,port.startingVessels);
                
                portActors.Add(port.PortId);
                taskList.Add(t);
            }
            await Task.WhenAll(taskList.ToArray());

        }

        public async Task<bool> NextTick()
        {
            Stopwatch t = new Stopwatch();
            t.Start();
            var tasks = new List<Task<bool>>();
            foreach (var actor in vesselActors) 
            {
                var grain = GrainFactory.GetGrain<IVessel>(actor);
                var r = grain.DoAction(OldTimeStamp,NewTimeStamp);
                tasks.Add(r);
                    
            }
            var results = await Task.WhenAll(tasks.ToArray());

            tasks = new List<Task<bool>>();
            foreach (var actor in routeActors)
            {
                var grain = GrainFactory.GetGrain<IRoute>(actor);
                var r = grain.DoAction(OldTimeStamp, NewTimeStamp);
                tasks.Add(r);

            }
            results = await Task.WhenAll(tasks.ToArray());

            tasks = new List<Task<bool>>();
            foreach (var actor in portActors)
            {
                var grain = GrainFactory.GetGrain<IPort>(actor);
                var r = grain.DoAction(OldTimeStamp, NewTimeStamp);
                tasks.Add(r);

            }
            results = await Task.WhenAll(tasks.ToArray());

            OldTimeStamp = OldTimeStamp.AddSeconds(Constants.TickSizeSeconds);
            NewTimeStamp = NewTimeStamp.AddSeconds(Constants.TickSizeSeconds);
            t.Stop();



            using (StreamWriter w = File.AppendText("result.txt"))
            {
                w.WriteLine(t.ElapsedMilliseconds);
            }


            logger.LogInformation("Tick time: " + t.Elapsed.ToString());
            return true;
        }

        public Task<bool> IncreaseTimeStamps() 
        {
            OldTimeStamp = OldTimeStamp.AddSeconds(Constants.TickSizeSeconds);
            NewTimeStamp = NewTimeStamp.AddSeconds(Constants.TickSizeSeconds);
            return Task.FromResult(true);
        }
    }
}
