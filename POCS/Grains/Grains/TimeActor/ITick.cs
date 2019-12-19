using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains.TimeActor
{
    public interface ITick : IGrainWithStringKey
    {
        Task<bool> DoAction(DateTime startTS, DateTime endTS);
    }
}
