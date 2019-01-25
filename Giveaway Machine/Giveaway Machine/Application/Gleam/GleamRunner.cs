using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Application.Gleam
{
    class GleamRunner : AbstractRunner
    {
        public override string Name { get; }

        public GleamRunner()
        {
            Name = "gleam";
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
