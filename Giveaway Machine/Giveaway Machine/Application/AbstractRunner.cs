using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Application
{
    abstract class AbstractRunner
    {
        protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        abstract public string Name { get; }
        abstract public void Run();
        protected Facade facade;


        public AbstractRunner(Facade facade)
        {
            this.facade = facade;
        }

        abstract public void Stop();
    }
}
