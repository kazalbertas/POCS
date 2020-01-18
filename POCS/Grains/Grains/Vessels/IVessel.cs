using Grains.Grains.Route;
using Grains.Grains.TimeActor;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains.Vessels
{
    public interface IVessel : IGrainWithStringKey, ITick
    {

        Task ResetDistanceOnRoute();
        Task<int> GetDistanceOnRoute();

        Task Depart(DateTime time);
        Task SetRoute(string route);
        Task SetAll(string name, int capacity, int speed);

        Task<int> GetCapacity();

        Task<int> GetTicksToWait();

        Task SetTicksToWait(int ticks);

        Task AddAllocationToVessel(int allocation, string company, DateTime ts);
    }
}
