using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Giveaway_Machine.Application
{
    class RunnerHandler
    {
        private Facade facade;
        private Dictionary<string, AbstractRunner> runners;
        protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Dictionary<string, Thread> runnerThreads = new Dictionary<string, Thread>();

        public RunnerHandler(Facade f)
        {
            facade = f;
        }

        public Dictionary<string,AbstractRunner> getRunners()
        {
            if (runners != null)
            {
                return runners;
            }

            logger.Info("Dynamically fetching all runners...");
            IEnumerable<Type> types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.StartsWith("Giveaway_Machine.Application"))
                .Where(t => t.Name.Contains("Runner"))
                .Where(t => !t.Name.Equals("AbstractRunner"))
                .Where(t => !t.Name.Equals("RunnerHandler"));

            runners = new Dictionary<string, AbstractRunner>();
            foreach (Type t in types)
            {
                logger.Info("Runner: " + t.Name + " is being created...");
                AbstractRunner runner = (AbstractRunner)Activator.CreateInstance(t, facade);
                runners.Add(runner.Name, runner);
            }

            return runners;
        }

        internal void exitApplication()
        {
            logger.Info("Stopping all runners...");
            foreach(KeyValuePair<string, AbstractRunner> runner in runners)
            {
                runner.Value.Stop();
            }

        }

        internal void startRunner(string runnerName)
        {
            logger.Info("Attempting to start runner: " + runnerName);
            if(!getRunners().ContainsKey(runnerName))
            {
                logger.Error("Runner with name: " + runnerName + " does not exist.");
                return;
            }

            // If it already exists, run the existing runner
            if (runnerThreads.ContainsKey(runnerName))
            {
                if(runnerThreads[runnerName].IsAlive)
                {
                    logger.Warn("Attempted to start thread for: " + runnerName + ", but thread is still running.");
                    return;
                }
                runnerThreads.Remove(runnerName);
            }


            AbstractRunner runner = getRunners()[runnerName];

            logger.Debug("Creating ThreadStart for runner: " + runner.Name);
            ThreadStart newThread = new ThreadStart(runner.Run);

            logger.Debug("Creating new Thread for runner: " + runner.Name);
            Thread threadRunner = new Thread(newThread);
            runnerThreads.Add(runner.Name, threadRunner);

            logger.Debug("Starting new Thread for runner: " + runner.Name);
            threadRunner.Start();
        }
    }
}
