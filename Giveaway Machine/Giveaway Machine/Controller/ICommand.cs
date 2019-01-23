using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Controller
{
    abstract class Command
    {
        // The name of the command, as used in the console
        abstract public string Name { get; }

        // The description of the command, e.g. what it does.
        abstract public string Description { get; }

        // Whether this command is the exit command
        abstract public bool isExit { get; }

        abstract public void Execute(List<string> arguments);

        // Used to dynamically get all command classes
        private static IEnumerable<Type> getCommandTypes()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.StartsWith("Giveaway_Machine.Controller.Commands"));
        }

        public static Dictionary<string, Command> getAllCommandsDictionary()
        {
            // Obtain the clases
            IEnumerable<Type> types = getCommandTypes();

            // Add types to the Dictionary
            Dictionary<string, Command> commands = new Dictionary<string, Command>();
            foreach (Type t in types)
            {
                Command command = (Command)Activator.CreateInstance(t);
                commands.Add(command.Name, command);
            }

            return commands;
        }

        public static List<Command> getAllCommands()
        {
            // Obtain the clases
            IEnumerable<Type> types = getCommandTypes();

            // Add types to the list
            List<Command> commands = new List<Command>();
            foreach (Type t in types)
            {
                Command command = (Command)Activator.CreateInstance(t);
                commands.Add(command);
            }

            return commands;
        }

    }
}
