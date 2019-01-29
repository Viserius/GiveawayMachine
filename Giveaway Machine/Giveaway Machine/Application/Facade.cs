using Giveaway_Machine.API;
using Giveaway_Machine.Application;
using Giveaway_Machine.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine
{
    class Facade
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public Twitter Twitter { get; }
        public CommandHandler CommandHandler { get; }
        public RunnerHandler RunnerHandler { get; }

        public Facade()
        {
            logger.Info("A new Facade is created.");
            Twitter = new Twitter(this);
            CommandHandler = new CommandHandler(this);
            RunnerHandler = new RunnerHandler(this);
        }
    }
}
