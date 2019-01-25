using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Application
{
    interface IFetcher
    {
        List<IGiveaway> fetch();
    }
}
