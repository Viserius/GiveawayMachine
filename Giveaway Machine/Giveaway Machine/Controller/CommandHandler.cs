using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Controller
{
    class CommandHandler
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Dictionary<string, Command> commands;
        private Facade facade;

        // Prepare the parser
        public CommandHandler(Facade facade)
        {
            logger.Info("Initializing CommandHandler");
            this.facade = facade;
            logger.Info("Initializing CommandHandler completed.");
        }

        public bool ParseCommand(string args)
        {
            // Split the arguments
            if(args == null)
            {
                return false;
            }
            List<string> arguments = args.Split(' ').ToList();
            string commandName = arguments[0];
            arguments = arguments.Skip(1).ToList();

            // Execute the command
            Command currentCommand;
            if(getCommands().TryGetValue(commandName, out currentCommand))
            {
                currentCommand.Execute(arguments);
                return currentCommand.isExit;
            }
            else
            {
                Console.Error.WriteLine($"Command with name: \"{commandName}\" was not found.");
                return false;
            }
        }

        public Dictionary<string, Command> getCommands()
        {
            if (commands == null)
            {
                commands = Command.CreateCommands(facade);
            }
            return commands;
        }
    }
}
