using Giveaway_Machine.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Controller
{
    class Facade
    {
        public Twitter Twitter { get; }
        public Parser Parser { get; }

        public Facade()
        {
            Twitter = new Twitter();
            Parser = new Parser();
        }
    }
}
