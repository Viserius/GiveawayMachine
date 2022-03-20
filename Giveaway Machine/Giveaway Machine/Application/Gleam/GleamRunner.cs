using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace Giveaway_Machine.Application.Gleam
{
    class GleamRunner : AbstractRunner
    {
        public override string Name { get; }
        private GleamFetcher gleamFetcher;
        private bool hasStarted = false;

        public GleamRunner(Facade f) : base(f)
        {
            Name = "gleam";
        }

        public override void Run()
        {
            logger.Info("The runner: " + Name + " is now running!");

            if (!facade.Twitter.twitterAuthenticated())
            {
                logger.Warn("Aborting Gleam Runner, because no connection to Twitter could be established.");
                return;
            }

            gleamFetcher = new GleamFetcher(facade);
            gleamFetcher.getProcessor().Process("https://gleam.io/Y9cEC/seandnguyen-keysmart-pro-giveaway", 10);
            gleamFetcher.startFetching();
        }

        public override void Stop()
        {
            if(hasStarted && gleamFetcher != null)
            {
                hasStarted = false;
                logger.Info("Stopping the Gleam Runner...");
                gleamFetcher.Stop();
                logger.Info("The Gleam Runner has stopped...");
            }
        }
    }
}
