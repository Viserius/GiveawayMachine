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
        abstract public string Name { get; }

        abstract public void Run();

        public static List<AbstractRunner> getRunners()
        {
            IEnumerable<Type> types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.StartsWith("Giveaway_Machine.Application"))
                .Where(t => t.Name.Contains("Runner"))
                .Where(t => !t.Name.Equals("AbstractRunner"));

            List<AbstractRunner> runners = new List<AbstractRunner>();
            foreach(Type t in types)
            {
                runners.Add((AbstractRunner)Activator.CreateInstance(t));
            }

            return runners;
        }
    }
}
