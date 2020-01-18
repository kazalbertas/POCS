using Grains.Grains.TimeActor;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains.Ports
{
    public interface IPort : IGrainWithStringKey, ITick
    {
        Task<string> GetName();
        Task SetName();
        Task SetAll(string name, int intake, List<string> routes, List<string> startingVessels);

        Task VesselArrived(string vessel, DateTime time);
    }
}
