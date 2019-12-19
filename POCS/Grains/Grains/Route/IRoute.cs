using Grains.Grains.TimeActor;
using Grains.Grains.Vessels;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains.Route
{
    public interface IRoute : IGrainWithStringKey, ITick
    {
        Task<string> GetName();
        Task<string> GetPortA();
        Task<string> GetPortB();

        Task SetName(string x);
        Task SetPortA(string x);
        Task SetPortB(string x);

        Task SetAll(string name, string portA, string portB, int distance);

        Task AddVessel(string vessel,DateTime ts);

    }
}
