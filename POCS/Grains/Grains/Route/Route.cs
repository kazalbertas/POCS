using Grains.Grains.Ports;
using Grains.Grains.TimeActor;
using Grains.Grains.Vessels;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains.Route
{
    public class Route : Grain, IRoute
    {

        private readonly ILogger _logger;

        #region Getters and Setters
        private string _name = "";
        private string _portA = "";
        private string _portB = "";
        private int _distance = 0;

        private List<string> vessels = new List<string>();

        public Task<string> GetName()
        {
            return Task.FromResult(_name);
        }

        public Task<string> GetPortA()
        {
            return Task.FromResult(_portA);
        }

        public Task<string> GetPortB()
        {
            return Task.FromResult(_portB);
        }

        public Task SetName(string x)
        {
            _name = x;
            return Task.CompletedTask;
        }

        public Task SetPortA(string x)
        {
            _portA = x;
            return Task.CompletedTask;
        }

        public Task SetPortB(string x)
        {
            _portB = x;
            return Task.CompletedTask;
        }

        public Task SetAll(string name, string portA, string portB, int distance)
        {
            _name = name;
            _portA = portA;
            _portB = portB;
            _distance = distance;
            return Task.CompletedTask;
        }

        #endregion

        public Route(ILogger<Route> logger) 
        {
            _logger = logger;
        }

        public async Task<bool> DoAction(DateTime startTS, DateTime endTS)
        {
            //int range = (endTS - startTS).
            var taskList = new List<Task<string>>();
            foreach (var vessel in vessels) 
            {
                var v = GrainFactory.GetGrain<IVessel>(vessel);
                int dist = await v.GetDistanceOnRoute();
                taskList.Add(VesselDistanceCheck(dist,vessel,startTS));
            }

            if (taskList.Count != 0)
            {
                var remove = await Task.WhenAll(taskList.ToArray());

                foreach (var r in remove) 
                {
                    vessels.Remove(r);
                    //if (vessels.Remove(r)) _logger.LogInformation("Success REMOVE FROM ROUTE");
                }
            }

            return true;
        }

        private async Task<string> VesselDistanceCheck(int v, string vesselName, DateTime ts) 
        {
            //_logger.LogInformation("Vessel " + vesselName + " on route from:" + _portA + " to: " + _portB + " ROUTE_ID" + _name);
            if ( v >= _distance)
            {
                var p = GrainFactory.GetGrain<IPort>(_portB);
                await p.VesselArrived(vesselName, ts);
                return  vesselName;
            }
            return "";
        }

        public async Task AddVessel(string vessel,DateTime ts)
        {
            vessels.Add(vessel);
            //await KafkaProducer.SendToKafka(("Vessel " + vessel + " departed at " + ts.ToString()), "depart");
            //_logger.LogInformation("Vessel " + vessel + " departed at " + ts.ToString());
            //return Task.CompletedTask;
        }
    }
}
