using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains.TimeActor
{
    public interface ITimeGrain : IGrainWithStringKey
    {
        Task InitialiseSimulation(DateTime start);
        Task<bool> NextTick();
        Task<bool> IncreaseTimeStamps();
    }
}
