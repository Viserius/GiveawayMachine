using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Giveaway_Machine.Controller
{
    abstract class Command
    {
        protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        // The name of the command, as used in the console
        abstract public string Name { get; }

        // The description of the command, e.g. what it does.
        abstract public string Description { get; }

        // Whether this command is the exit command
        abstract public bool isExit { get; }

        abstract public void Execute(List<string> arguments);

        protected Facade facade;

        public Command(Facade facade)
        {
            this.facade = facade;
        }

        // Used to dynamically get all command classes
        private static IEnumerable<Type> getCommandTypes()
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.StartsWith("Giveaway_Machine.Controller.Commands"));
        }

        public static Dictionary<string, Command> CreateCommands(Facade facade)
        {
            logger.Info("Attempting to register all commands...");

            // Obtain the clases
            logger.Info("Fetching Command Types Dynamically");
            IEnumerable<Type> types = getCommandTypes();
            logger.Info("Command Types successfully fetched");

            // Add types to the Dictionary
            Dictionary<string, Command> commands = new Dictionary<string, Command>();
            foreach (Type t in types)
            {
                logger.Info("Now Registering Command: " + t.Name);
                Command command = (Command)Activator.CreateInstance(t, facade);
                commands.Add(command.Name, command);
            }

            logger.Info($"{commands.Count} Commands are now successfully registered.");

            return commands;
        }
    }
}
